namespace ProCenter.Domain.MessageModule
{
    using System;

    public interface IAssessmentReminderFactory
    {
        AssessmentReminder Create(Guid organizationKey,
                                         Guid patientKey,
                                         Guid createdByStaffKey,
                                         Guid assessmentDefinitionKey,
                                         string title,
                                         DateTime start,
                                         string description);
    }
}