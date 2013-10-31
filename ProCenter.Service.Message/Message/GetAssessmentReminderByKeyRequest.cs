namespace ProCenter.Service.Message.Message
{
    #region

    using System;
    using Agatha.Common;

    #endregion

    public class GetAssessmentReminderByKeyRequest : Request
    {
        public Guid AssessmentReminderKey { get; set; }
    }
}