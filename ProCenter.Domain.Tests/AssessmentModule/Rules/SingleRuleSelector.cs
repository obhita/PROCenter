namespace ProCenter.Domain.Tests.AssessmentModule.Rules
{
    using System.Collections.Generic;

    using Pillar.FluentRuleEngine;
    using Pillar.FluentRuleEngine.RuleSelectors;

    using ProCenter.Domain.AssessmentModule.Rules;

    public class SingleRuleSelector : IRuleSelector
    {
        private readonly IItemSkippingRule _skippingRule;

        public SingleRuleSelector (IItemSkippingRule skippingRule)
        {
            _skippingRule = skippingRule;
        }

        /// <summary>
        /// Gets the list of <see cref="T:Pillar.FluentRuleEngine.IRule">Rules</see> to run.
        /// </summary>
        /// <typeparam name="TSubject">Type of subject for <paramref name="ruleCollection">rule collection</paramref></typeparam><param name="ruleCollection">The rule collection.</param><param name="context">The context.</param>
        /// <returns>
        /// List of rules to run.
        /// </returns>
        public IEnumerable<IRule> SelectRules<TSubject> ( IRuleCollection<TSubject> ruleCollection, IRuleEngineContext context )
        {
            return new List<IRule> { _skippingRule };
        }
    }
}
