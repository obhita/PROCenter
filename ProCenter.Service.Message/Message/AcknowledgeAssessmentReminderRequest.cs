namespace ProCenter.Service.Message.Message
{
    using System;
    using Agatha.Common;

    public class AcknowledgeAssessmentReminderRequest : Request
    {
        public Guid Key { get; set; }
    }
}
