namespace ProCenter.Service.Handler.Patient
{
    #region Using Statements

    using Common;
    using Domain.CommonModule;
    using Domain.PatientModule;
    using Service.Message.Common;
    using Service.Message.Patient;
    using global::AutoMapper;

    #endregion

    /// <summary>
    ///     Create patient request handler.
    /// </summary>
    public class CreatePatientRequestHandler : ServiceRequestHandler<CreatePatientRequest, SaveDtoResponse<PatientDto>>
    {
        #region Fields

        private readonly ILookupProvider _lookupProvider;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreatePatientRequestHandler" /> class.
        /// </summary>
        /// <param name="lookupProvider">The lookup provider.</param>
        public CreatePatientRequestHandler ( ILookupProvider lookupProvider )
        {
            _lookupProvider = lookupProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( CreatePatientRequest request, SaveDtoResponse<PatientDto> response )
        {
            var patientFactory = new PatientFactory ();
            var patient = patientFactory.Create (request.PatientDto.OrganizationKey, request.PatientDto.Name, request.PatientDto.DateOfBirth, _lookupProvider.Find<Gender> ( request.PatientDto.Gender.Code ) );

            if ( patient != null )
            {
                if ( request.PatientDto.Religion != null && !string.IsNullOrEmpty ( request.PatientDto.Religion.Code ) )
                {
                    patient.ReviseReligion ( _lookupProvider.Find<Religion> ( request.PatientDto.Religion.Code ) );
                }
                if ( request.PatientDto.Ethnicity != null && !string.IsNullOrEmpty ( request.PatientDto.Ethnicity.Code ) )
                {
                    patient.ReviseEthnicity ( _lookupProvider.Find<Ethnicity> ( request.PatientDto.Ethnicity.Code ) );
                }

                var patientDto = Mapper.Map<Patient, PatientDto> ( patient );

                response.DataTransferObject = patientDto;
            }
        }

        #endregion
    }
}