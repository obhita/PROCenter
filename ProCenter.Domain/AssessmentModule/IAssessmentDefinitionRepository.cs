namespace ProCenter.Domain.AssessmentModule
{
    using System;
    using CommonModule;

    public interface IAssessmentDefinitionRepository : IRepository<AssessmentDefinition>
    {
        Guid GetKeyByCode(string assessmentDefinitionCode);
    }
}