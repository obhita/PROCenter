namespace ProCenter.Service.Handler.Organization
{
    using System;
    using System.Linq;
    using Common;
    using Dapper;
    using Domain.OrganizationModule;
    using Domain.SecurityModule;
    using Infrastructure.Service.ReadSideService;
    using Service.Message.Organization;
    using Service.Message.Security;
    using global::AutoMapper;

    public class GetStaffDtoByKeyRequestHandler:ServiceRequestHandler<GetStaffDtoByKeyRequest,GetStaffDtoResponse>
    {
        private readonly IStaffRepository _staffRepository;
        private readonly ISystemAccountRepository _systemAccountRepository;
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public GetStaffDtoByKeyRequestHandler(IStaffRepository staffRepository, ISystemAccountRepository systemAccountRepository, IDbConnectionFactory dbConnectionFactory)
        {
            _staffRepository = staffRepository;
            _systemAccountRepository = systemAccountRepository;
            _dbConnectionFactory = dbConnectionFactory;
        }

        protected override void Handle(GetStaffDtoByKeyRequest request, GetStaffDtoResponse response)
        {
            var staff = _staffRepository.GetByKey(request.Key);
            var staffDto = Mapper.Map<Staff, StaffDto>(staff);

            //get system account associated with staff
            Guid? systemAccountKey;
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                systemAccountKey = connection.Query<Guid?>("SELECT SystemAccountKey FROM SecurityModule.SystemAccount WHERE StaffKey=@StaffKey", new { StaffKey = request.Key }).FirstOrDefault();
            }
            if (systemAccountKey.HasValue)
            {
                var systemAccount = _systemAccountRepository.GetByKey(systemAccountKey.Value);
                var systemAccountDto = Mapper.Map<SystemAccount, SystemAccountDto>(systemAccount);
                if (systemAccount.RoleKeys.Any())
                {
                    var roleKeys = string.Join(", ", systemAccount.RoleKeys);
                    roleKeys = "'" + roleKeys.Replace(", ", "', '") + "'";
                    var query = string.Format("SELECT RoleKey as 'Key', Name FROM SecurityModule.Role WHERE RoleKey IN ({0})", roleKeys);
                    using (var connection = _dbConnectionFactory.CreateConnection())
                    {
                        var roleDtos = connection.Query<RoleDto>(query).OrderBy(r=>r.Name);
                        systemAccountDto.Roles = roleDtos;
                    }
                }
                
                staffDto.SystemAccount = systemAccountDto;
            }
            response.DataTransferObject = staffDto;
        }
    }
}