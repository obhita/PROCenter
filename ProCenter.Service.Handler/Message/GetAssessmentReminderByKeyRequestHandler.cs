namespace ProCenter.Service.Handler.Message
{
    #region

    using System.Linq;
    using Common;
    using Dapper;
    using Domain.AssessmentModule;
    using Domain.MessageModule;
    using Domain.PatientModule;
    using Infrastructure.Service.ReadSideService;
    using ProCenter.Common;
    using Service.Message.Common;
    using Service.Message.Message;
    using global::AutoMapper;

    #endregion

    public class GetAssessmentReminderByKeyRequestHandler : ServiceRequestHandler<GetAssessmentReminderByKeyRequest, DtoResponse<AssessmentReminderDto>>
    {
        private readonly IAssessmentReminderRepository _assessmentReminderRepository;
        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IResourcesManager _resourcesManager;

        public GetAssessmentReminderByKeyRequestHandler( 
            IAssessmentReminderRepository assessmentReminderRepository, 
            IAssessmentDefinitionRepository assessmentDefinitionRepository, 
            IPatientRepository patientRepository, 
            IResourcesManager resourcesManager )
        {
            _assessmentReminderRepository = assessmentReminderRepository;
            _assessmentDefinitionRepository = assessmentDefinitionRepository;
            _patientRepository = patientRepository;
            _resourcesManager = resourcesManager;
        }

        protected override void Handle(GetAssessmentReminderByKeyRequest request, DtoResponse<AssessmentReminderDto> response)
        {
            var assessmentReminder = _assessmentReminderRepository.GetByKey ( request.AssessmentReminderKey );
            if ( assessmentReminder != null )
            {
                var patient = _patientRepository.GetByKey ( assessmentReminder.PatientKey );
                var assessmentDefinition = _assessmentDefinitionRepository.GetByKey ( assessmentReminder.AssessmentDefinitionKey );
                var assessmentReminderDto = Mapper.Map<AssessmentReminder, AssessmentReminderDto> ( assessmentReminder );
                assessmentReminderDto.AssessmentName = _resourcesManager.GetResourceManagerByName(assessmentDefinition.CodedConcept.Name).GetString("_" + assessmentDefinition.CodedConcept.Code);
                assessmentReminderDto.PatientFirstName = patient.Name.FirstName;
                assessmentReminderDto.PatientLastName = patient.Name.LastName;
                response.DataTransferObject = assessmentReminderDto;
            }
        }
    }
}