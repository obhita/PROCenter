namespace ProCenter.Service.Handler.Organization
{
    #region

    using Agatha.Common;
    using Common;
    using Domain.OrganizationModule;
    using Service.Message.Organization;

    #endregion

    public class ActivateDeactivateAssessmentRequestHandler : ServiceRequestHandler<ActivateDeactivateAssessmentRequest, Response>
    {
        private readonly IOrganizationRepository _organizationRepository;

        public ActivateDeactivateAssessmentRequestHandler(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        protected override void Handle(ActivateDeactivateAssessmentRequest request, Response response)
        {
            var orgnaization = _organizationRepository.GetByKey(request.OrganizationKey);
            if (orgnaization != null)
            {
                if (request.IsActivating)
                {
                    orgnaization.AddAssessmentDefinition(request.AssessmentDefinitionKey);
                }
                else
                {
                    orgnaization.RemoveAssessmentDefinition(request.AssessmentDefinitionKey);
                }
            }
        }
    }
}