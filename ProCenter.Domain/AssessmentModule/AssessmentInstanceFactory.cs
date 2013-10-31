namespace ProCenter.Domain.AssessmentModule
{
    using System;

    public class AssessmentInstanceFactory : IAssessmentInstanceFactory
    {
        public AssessmentInstance Create(Guid assessmentDefinitionKey, Guid patientKey, string assessmentName)
        {
            return new AssessmentInstance(assessmentDefinitionKey, patientKey, assessmentName);
        }
    }
}