#region Licence Header
// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/
#endregion
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