namespace ProCenter.Service.Handler.Patient
{
    #region Using Statements

    using Common;
    using Domain.CommonModule;
    using Domain.PatientModule;
    using Pillar.Domain.Primitives;
    using Service.Message.Common;
    using Service.Message.Patient;
    using global::AutoMapper;

    #endregion

    /// <summary>
    ///     Save Patient request handler.
    /// </summary>
    public class SavePatientDtoRequestHandler : ServiceRequestHandler<SaveDtoRequest<PatientDto>, SaveDtoResponse<PatientDto>>
    {
        #region Fields

        private readonly ILookupProvider _lookupProvider;
        private readonly IPatientRepository _patientRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SavePatientDtoRequestHandler" /> class.
        /// </summary>
        /// <param name="patientRepository">The patient repository.</param>
        /// <param name="lookupProvider">The lookup provider.</param>
        public SavePatientDtoRequestHandler ( IPatientRepository patientRepository, ILookupProvider lookupProvider )
        {
            _patientRepository = patientRepository;
            _lookupProvider = lookupProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( SaveDtoRequest<PatientDto> request, SaveDtoResponse<PatientDto> response )
        {
            var patient = _patientRepository.GetByKey ( request.DataTransferObject.Key );
            if ( patient != null )
            {
                patient.ReviseName ( request.DataTransferObject.Name );
                patient.ReviseDateOfBirth ( request.DataTransferObject.DateOfBirth );
                patient.ReviseGender ( _lookupProvider.Find<Gender> ( request.DataTransferObject.Gender.Code ) );
                if ( request.DataTransferObject.Ethnicity != null && !string.IsNullOrEmpty ( request.DataTransferObject.Ethnicity.Code ) )
                {
                    patient.ReviseEthnicity ( _lookupProvider.Find<Ethnicity> ( request.DataTransferObject.Ethnicity.Code ) );
                }
                if ( request.DataTransferObject.Religion != null && !string.IsNullOrEmpty ( request.DataTransferObject.Religion.Code ) )
                {
                    patient.ReviseReligion ( _lookupProvider.Find<Religion> ( request.DataTransferObject.Religion.Code ) );
                }
                patient.ReviseEmail(string.IsNullOrWhiteSpace(request.DataTransferObject.Email) ? null : new Email(request.DataTransferObject.Email));

                response.DataTransferObject = Mapper.Map<Patient, PatientDto> ( patient );
            }
        }

        #endregion
    }
}