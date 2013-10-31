namespace ProCenter.Service.Handler.Assessment
{
    #region Using Statements

    using Common;
    using Domain.AssessmentModule;
    using Domain.CommonModule;
    using Infrastructure.Domain;
    using Service.Message.Assessment;

    #endregion

    public class CreateAssessmentRequestHandler : ServiceRequestHandler<CreateAssessmentRequest, CreateAssessmentResponse>
    {
        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;

        public CreateAssessmentRequestHandler(IAssessmentDefinitionRepository assessmentDefinitionRepository)
        {
            _assessmentDefinitionRepository = assessmentDefinitionRepository;
        }

        protected override void Handle(CreateAssessmentRequest request, CreateAssessmentResponse response)
        {
            var assessmentDefinition = _assessmentDefinitionRepository.GetByKey(request.AssessmentDefinitionKey);
            var assessmentInstanceFactory = new AssessmentInstanceFactory();
            var assessmentInstance = assessmentInstanceFactory.Create(request.AssessmentDefinitionKey, request.PatientKey, assessmentDefinition.CodedConcept.Name);

            if (assessmentInstance != null)
            {
                if ( request.ForSelfAdministration )
                {
                    assessmentInstance.AllowSelfAdministration ();
                }
                if (request.WorkflowKey.HasValue)
                {
                    assessmentInstance.AddToWorkflow(request.WorkflowKey.Value);
                }
                response.AssessmentInstanceKey = assessmentInstance.Key;
            }
        }
    }
}