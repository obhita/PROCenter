namespace ProCenter.Service.Message.Message
{
    #region

    using System;
    using Agatha.Common;

    #endregion

    public class UpdateAssessmentReminderRequest : Request
    {
        public AssessmentReminderDto AssessmentReminderDto { get; set; }

        public Guid AssessmentReminderKey { get; set; }
        public int DayDelta { get; set; }
    }
}