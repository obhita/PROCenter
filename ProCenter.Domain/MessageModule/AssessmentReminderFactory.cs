namespace ProCenter.Domain.MessageModule
{
    using System;

    public class AssessmentReminderFactory : IAssessmentReminderFactory
    {
        public AssessmentReminder Create(Guid organizationKey, Guid patientKey, Guid createdByStaffKey, Guid assessmentDefinitionKey, string title, DateTime start,
                                                string description)
        {
            return new AssessmentReminder(organizationKey, patientKey, createdByStaffKey, assessmentDefinitionKey, title, start, description);
        }
    }
}