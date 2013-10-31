namespace ProCenter.Domain.OrganizationModule.Event
{
    using System;
    using CommonModule;

    public class AssessmentDefinitionAddedEvent: CommitEventBase
    {
        public AssessmentDefinitionAddedEvent(Guid key, int version, Guid assessmentDefinitionKey) : base(key, version)
        {
            AssessmentDefinitionKey = assessmentDefinitionKey;
        }

        public Guid AssessmentDefinitionKey { get; private set; }
    }
}