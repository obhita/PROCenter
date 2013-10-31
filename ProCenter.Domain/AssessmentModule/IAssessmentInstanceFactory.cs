namespace ProCenter.Domain.AssessmentModule
{
    using System;

    public interface IAssessmentInstanceFactory
    {
        AssessmentInstance Create(Guid assessmentDefinitionKey, Guid patientKey, string assessmentName);
    }
}