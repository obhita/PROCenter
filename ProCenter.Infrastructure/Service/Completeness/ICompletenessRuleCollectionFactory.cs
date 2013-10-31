namespace ProCenter.Infrastructure.Service.Completeness
{
    #region

    using System.Collections.Generic;
    using ProCenter.Domain.AssessmentModule;

    #endregion

    public interface ICompletenessRuleCollectionFactory
    {
        ICompletenessRuleCollection<TEntity> GetCompletenessRuleCollection<TEntity>(string completenessCategory);

        IEnumerable<ICompletenessRuleCollection<TEntity>> GetCompletenessRuleCollections<TEntity>();
    }
}