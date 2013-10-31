namespace ProCenter.Domain.AssessmentModule
{
    #region

    using Pillar.FluentRuleEngine;

    #endregion

    public interface ICompletenessRuleCollection<TEntity> : IRuleCollection<TEntity>
    {
        string CompletenessCategory { get; }

        IRuleSet CompletenessRuleSet { get; }
    }
}