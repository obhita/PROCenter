namespace ProCenter.Service.Handler.Security
{
    #region Using Statements

    using Common;
    using Domain.PatientModule;
    using Domain.SecurityModule;
    using Service.Message.Security;

    #endregion

    public class ValidatePatientAccountResponseHandler : ServiceRequestHandler<ValidatePatientAccountRequest, ValidatePatientAccountResponse>
    {
        #region Fields

        private readonly IPatientRepository _patientRepository;
        private readonly ISystemAccountRepository _systemAccountRepository;

        #endregion

        #region Constructors and Destructors

        public ValidatePatientAccountResponseHandler ( ISystemAccountRepository systemAccountRepository, IPatientRepository patientRepository )
        {
            _systemAccountRepository = systemAccountRepository;
            _patientRepository = patientRepository;
        }

        #endregion

        #region Methods

        protected override void Handle ( ValidatePatientAccountRequest request, ValidatePatientAccountResponse response )
        {
            var systemAccount = _systemAccountRepository.GetByKey ( request.SystemAccountKey );
            var patient = _patientRepository.GetByKey ( systemAccount.PatientKey.Value );
            var validationResult = patient.ValidateInfo ( systemAccount, request.PatientIdentifier, request.DateOfBirth );
            if ( validationResult == ValidationStatus.Locked )
            {
                response.IsLocked = true;
            }
            if ( validationResult == ValidationStatus.Valid )
            {
                response.Validated = true;
            }
        }

        #endregion
    }
}