namespace ProCenter.Domain.OrganizationModule.Event
{
    using System;
    using CommonModule;

    public class AssessmentDefinitionRemovedEvent: CommitEventBase
    {
        public AssessmentDefinitionRemovedEvent(Guid key, int version, Guid assessmentDefinitionKey)
            : base(key, version)
        {
            AssessmentDefinitionKey = assessmentDefinitionKey;
        }

        public Guid AssessmentDefinitionKey { get; private set; }
    }
}