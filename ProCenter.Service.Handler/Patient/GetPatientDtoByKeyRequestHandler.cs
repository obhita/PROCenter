namespace ProCenter.Service.Handler.Patient
{
    #region

    using System;
    using System.Linq;
    using Common;
    using Dapper;
    using Domain.PatientModule;
    using Domain.SecurityModule;
    using Infrastructure.Service.ReadSideService;
    using Service.Message.Patient;
    using Service.Message.Security;
    using global::AutoMapper;

    #endregion

    public class GetPatientDtoByKeyRequestHandler :
        ServiceRequestHandler<GetPatientDtoByKeyRequest, GetPatientDtoResponse>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ISystemAccountRepository _systemAccountRepository;
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public GetPatientDtoByKeyRequestHandler(IPatientRepository patientRepository, ISystemAccountRepository systemAccountRepository, IDbConnectionFactory dbConnectionFactory)
        {
            _patientRepository = patientRepository;
            _systemAccountRepository = systemAccountRepository;
            _dbConnectionFactory = dbConnectionFactory;
        }

        protected override void Handle(GetPatientDtoByKeyRequest request, GetPatientDtoResponse response)
        {
            var patient = _patientRepository.GetByKey(request.PatientKey);
            var patientDto = Mapper.Map<Patient, PatientDto>(patient);

            //get system account associated with staff
            Guid? systemAccountKey;
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                systemAccountKey =
                    connection.Query<Guid?>("SELECT SystemAccountKey FROM SecurityModule.SystemAccount WHERE PatientKey=@PatientKey", new {request.PatientKey}).FirstOrDefault();
            }
            if (systemAccountKey.HasValue)
            {
                var systemAccount = _systemAccountRepository.GetByKey(systemAccountKey.Value);
                var systemAccountDto = Mapper.Map<SystemAccount, SystemAccountDto>(systemAccount);
                //if (systemAccount.RoleKeys.Any())
                //{
                //    var roleKeys = string.Join(", ", systemAccount.RoleKeys);
                //    roleKeys = "'" + roleKeys.Replace(", ", "', '") + "'";
                //    var query = string.Format("SELECT RoleKey as 'Key', Name FROM SecurityModule.Role WHERE RoleKey IN ({0})", roleKeys);
                //    using (var connection = _dbConnectionFactory.CreateConnection())
                //    {
                //        var roleDtos = connection.Query<RoleDto>(query).OrderBy(r => r.Name);
                //        systemAccountDto.Roles = roleDtos;
                //    }
                //}

                patientDto.SystemAccount = systemAccountDto;
            }
            response.DataTransferObject = patientDto;
        }
    }
}