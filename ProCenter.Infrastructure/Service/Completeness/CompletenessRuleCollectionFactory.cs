namespace ProCenter.Infrastructure.Service.Completeness
{
    #region

    using System.Collections.Generic;
    using Pillar.Common.InversionOfControl;
    using Pillar.FluentRuleEngine;
    using ProCenter.Domain.AssessmentModule;

    #endregion

    public class CompletenessRuleCollectionFactory : ICompletenessRuleCollectionFactory
    {
        private readonly IRuleCollectionFactory _ruleCollectionFactory;
        private readonly IContainer _container;

        public CompletenessRuleCollectionFactory(IRuleCollectionFactory ruleCollectionFactory, IContainer container)
        {
            _ruleCollectionFactory = ruleCollectionFactory;
            _container = container;
        }

        public ICompletenessRuleCollection<TEntity> GetCompletenessRuleCollection<TEntity>(string completenessCategory)
        {
            var completenessRuleCollection = (ICompletenessRuleCollection<TEntity>) _container.TryResolve(typeof (ICompletenessRuleCollection<TEntity>), completenessCategory);
            _ruleCollectionFactory.CustomizeRuleCollection(completenessRuleCollection);
            return completenessRuleCollection;
        }

        public IEnumerable<ICompletenessRuleCollection<TEntity>> GetCompletenessRuleCollections<TEntity>()
        {
            var completenessRuleCollections = _container.ResolveAll<ICompletenessRuleCollection<TEntity>>();
            foreach (var completenessRuleCollection in completenessRuleCollections)
            {
                _ruleCollectionFactory.CustomizeRuleCollection(completenessRuleCollection);
                yield return completenessRuleCollection;
            }
        }
    }
}