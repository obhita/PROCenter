namespace ProCenter.Domain.Nida
{
    #region Using Statements

    using AssessmentModule;
    using MessageModule;
    using Pillar.Common.InversionOfControl;
    using Pillar.FluentRuleEngine;
    using Pillar.FluentRuleEngine.RuleSelectors;

    #endregion

    [WorkflowAssessments("NidaSingleQuestionScreener", "DrugAbuseScreeningTest", "NidaAssessFurther")]
    public class NidaWorkflowEngine : IWorkflowEngine
    {
        private readonly IMessageCollector _messageCollector;
        private readonly IRuleCollectionFactory _ruleCollectionFactory;
        private readonly IRuleEngineFactory _ruleEngineFactory;

        public NidaWorkflowEngine(IRuleCollectionFactory ruleCollectionFactory, IRuleEngineFactory ruleEngineFactory,
                                  IMessageCollector messageCollector)
        {
            _ruleCollectionFactory = ruleCollectionFactory;
            _ruleEngineFactory = ruleEngineFactory;
            _messageCollector = messageCollector;
        }

        public void Run(AssessmentInstance assessmentInstance)
        {
            //TODO:If Required
            //Need to update pillar to allow for named rule collections _ruleCollectionFactory.CreateRuleCollection<AssessmentInstance>("NidaWorkflow");
            var ruleCollection = IoC.CurrentContainer.Resolve<NidaWorkflowRuleCollection>();
            var ruleEngine = _ruleEngineFactory.CreateRuleEngine(assessmentInstance, ruleCollection);
            var ruleEngineContext = new RuleEngineContext<AssessmentInstance>(assessmentInstance, new SelectAllRulesInRuleSetSelector(assessmentInstance.AssessmentName + "RuleSet"));
            ruleEngineContext.WorkingMemory.AddContextObject(_messageCollector, "MessageCollector");
            ruleEngine.ExecuteRules(ruleEngineContext);
        }
    }
}