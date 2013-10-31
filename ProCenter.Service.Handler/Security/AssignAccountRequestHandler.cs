namespace ProCenter.Service.Handler.Security
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using Common;
    using Domain.SecurityModule;
    using Infrastructure.Service.ReadSideService;
    using NLog;
    using Pillar.Domain.Primitives;
    using Service.Message.Common;
    using Service.Message.Security;
    using global::AutoMapper;
    using Dapper;

    public class AssignAccountRequestHandler : ServiceRequestHandler<AssignAccountRequest, AssignAccountResponse> 
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ISystemAccountRepository _systemAccountRepository;
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public AssignAccountRequestHandler(ISystemAccountRepository systemAccountRepository, IDbConnectionFactory dbConnectionFactory)
        {
            _systemAccountRepository = systemAccountRepository;
            _dbConnectionFactory = dbConnectionFactory;
        }

        protected override void Handle(AssignAccountRequest request, AssignAccountResponse response)
        {
            if (request.SystemAccountDto.CreateNew)
            {
                var systemAccount = _systemAccountRepository.GetByIdentifier(request.SystemAccountDto.Identifier);
                if (systemAccount != null) // account existing
                {
                    var dataErrorInfo = new DataErrorInfo(string.Format("Cannot create account because an account with the email {0} already exists.", request.SystemAccountDto.Identifier), ErrorLevel.Error);
                    response.SystemAccountDto = request.SystemAccountDto;
                    response.SystemAccountDto.AddDataErrorInfo(dataErrorInfo);
                }
                else
                {
                    // 1. create member login in Identity server
                    // 2. Create System account in domain
                    // 3. assign system account to the new staff or patient
                    // 4. error handling: if the login/account is taken or cannot create new login
                    using (var httpClient = new HttpClient {BaseAddress = new Uri(request.BaseIdentityServerUri)})
                    {
                        httpClient.SetToken("Session", request.Token);
                        var httpResponseMessage = httpClient.GetAsync("api/membership/Create/" + request.SystemAccountDto.Username + "?email=" + request.SystemAccountDto.Email).Result;
                        if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                        {
                            var membershipUserDto = httpResponseMessage.Content.ReadAsAsync<MembershipUserDto>().Result;
                            var systemAccountFactory = new SystemAccountFactory();
                            systemAccount = systemAccountFactory.Create(request.OrganizationKey, membershipUserDto.NameIdentifier, new Email(membershipUserDto.Email));
                            if (request.StaffKey != Guid.Empty)
                            {
                                systemAccount.AssignToStaff(request.StaffKey);
                            }
                            if (request.PatientKey != Guid.Empty)
                            {
                                systemAccount.AssignToPatient(request.PatientKey);

                                Guid? portalRoleKey;
                                using (var connection = _dbConnectionFactory.CreateConnection())
                                {
                                    portalRoleKey = connection.Query<Guid?>("SELECT [RoleKey] FROM [SecurityModule].[Role] WHERE Name=@Name", new {Name = "Patient Portal"}).FirstOrDefault();
                                }
                                if (portalRoleKey.HasValue)
                                {
                                    systemAccount.AddRole(portalRoleKey.Value);
                                }
                                else
                                {
                                    Logger.Error("Cannot find Patient portal built in role.");
                                }
                            }
                            var systemAccountDto = Mapper.Map<SystemAccount, SystemAccountDto>(systemAccount);
                            response.SystemAccountDto = systemAccountDto;
                        }
                        else
                        {
                            var result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                            var dataErrorInfo = new DataErrorInfo(result, ErrorLevel.Error);
                            response.SystemAccountDto = request.SystemAccountDto;
                            response.SystemAccountDto.AddDataErrorInfo(dataErrorInfo);
                        }
                    }
                }
            }
            else
            {
                var systemAccount = _systemAccountRepository.GetByIdentifier(request.SystemAccountDto.Identifier);
                if (systemAccount != null) // account existing
                {
                    if (systemAccount.StaffKey == null)
                    {
                        systemAccount.AssignToStaff(request.StaffKey);
                        var systemAccountDto = Mapper.Map<SystemAccount, SystemAccountDto>(systemAccount);
                        response.SystemAccountDto = systemAccountDto;
                    }
                    else
                    {
                        var dataErrorInfo = new DataErrorInfo(string.Format("Cannot link account because an account with the email {0} has been assigned to another staff.", request.SystemAccountDto.Identifier), ErrorLevel.Error);
                        response.SystemAccountDto = request.SystemAccountDto;
                        response.SystemAccountDto.AddDataErrorInfo(dataErrorInfo);
                    }
                }
                else
                {
                    var dataErrorInfo = new DataErrorInfo(string.Format("Cannot link account because an account with the email {0} does not exist.", request.SystemAccountDto.Identifier), ErrorLevel.Error);
                    response.SystemAccountDto = request.SystemAccountDto;
                    response.SystemAccountDto.AddDataErrorInfo(dataErrorInfo);
                }
            }
        }
    }
}