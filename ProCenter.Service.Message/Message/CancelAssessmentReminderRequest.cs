namespace ProCenter.Service.Message.Message
{
    #region

    using System;
    using Agatha.Common;

    #endregion

    public class CancelAssessmentReminderRequest : Request
    {
        public Guid AssessmentReminderKey { get; set; }
    }
}