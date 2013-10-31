namespace ProCenter.Mvc.Infrastructure
{
    #region

    using Domain.AssessmentModule;
    using Pillar.FluentRuleEngine;

    #endregion

    public class EmptyCompletenessRuleCollection : AbstractRuleCollection<AssessmentInstance>, ICompletenessRuleCollection<AssessmentInstance>
    {
        public string CompletenessCategory {
            get { return Domain.AssessmentModule.CompletenessCategory.Report; }
        }

        public IRuleSet CompletenessRuleSet { get; private set; }
    }
}