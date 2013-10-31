namespace ProCenter.Domain.MessageModule.Event
{
    #region

    using System;

    #endregion

    public class AssessmentReminderCreatedEvent : MessageEventBase
    {
        public AssessmentReminderCreatedEvent(Guid key,
                                                     MessageType messageType,
                                                     Guid organizationKey,
                                                     Guid patientKey,
                                                     Guid createdByStaffKey,
                                                     Guid assessmentDefinitionKey,
                                                     string title,
                                                     DateTime start,
                                                     string description,
                                                     AssessmentReminderStatus status
            )
            : base(key, messageType)
        {
            OrganizationKey = organizationKey;
            PatientKey = patientKey;
            CreatedByStaffKey = createdByStaffKey;
            AssessmentDefinitionKey = assessmentDefinitionKey;
            Title = title;
            Start = start;
            Description = description;
            Status = status;
        }

        public Guid OrganizationKey { get; set; }
        public Guid PatientKey { get; set; }
        public Guid CreatedByStaffKey { get; set; }
        public Guid AssessmentDefinitionKey { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public string Description { get; set; }
        public AssessmentReminderStatus Status { get; set; }
    }
}