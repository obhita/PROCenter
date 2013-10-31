namespace ProCenter.Service.Message.Organization
{
    using System;
    using Agatha.Common;

    public class ActivateDeactivateAssessmentRequest : Request
    {
        public bool IsActivating { get; set; }

        public Guid OrganizationKey { get; set; }

        public Guid AssessmentDefinitionKey { get; set; }
    }
}