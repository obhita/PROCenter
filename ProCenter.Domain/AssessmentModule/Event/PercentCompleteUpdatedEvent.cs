namespace ProCenter.Domain.AssessmentModule.Event
{
    #region

    using System;
    using CommonModule;

    #endregion

    public class PercentCompleteUpdatedEvent : CommitEventBase
    {
        public PercentCompleteUpdatedEvent(Guid key, int version, double percentComplete) : base(key, version)
        {
            PercentComplete = percentComplete;
        }

        public Double PercentComplete { get; set; }
    }
}