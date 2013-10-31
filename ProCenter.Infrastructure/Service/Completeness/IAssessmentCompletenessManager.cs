namespace ProCenter.Infrastructure.Service.Completeness
{
    #region

    using ProCenter.Domain.AssessmentModule;

    #endregion

    public interface IAssessmentCompletenessManager
    {
        #region Public Methods and Operators

        CompletenessResults CalculateCompleteness<TAssessment>(string completenessCategory, TAssessment assessment, IContainItemDefinitions itemDefinitionContainer)
            where TAssessment : AssessmentInstance;

        #endregion
    }
}