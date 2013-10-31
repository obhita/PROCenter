namespace ProCenter.Infrastructure.Tests.Service.Completeness
{
    #region

    using Infrastructure.Service.Completeness;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Pillar.FluentRuleEngine;
    using ProCenter.Domain.AssessmentModule;

    #endregion

    [TestClass]
    public class AssessmentCompletenessManagerTests
    {
        [TestMethod]
        public void ExecuteCompleteness_CompleteAssessment_ResultCorrect()
        {
            var ruleCollection = new Mock<ICompletenessRuleCollection<AssessmentInstance>>().Object;
            var ruleEngine = new RuleEngine<AssessmentInstance>(ruleCollection);
            var mockRuleEngineFactory = new Mock<IRuleEngineFactory>();
            mockRuleEngineFactory.Setup(f => f.CreateRuleEngine(It.IsAny<AssessmentInstance>(), It.IsAny<IRuleCollection<AssessmentInstance>>())).Returns(ruleEngine);

            var assessmentCompletenessManager = new AssessmentCompletenessManager(new Mock<ICompletenessRuleCollectionFactory>().Object, mockRuleEngineFactory.Object);

            var result = assessmentCompletenessManager.CalculateCompleteness(CompletenessCategory.Report, new Mock<AssessmentInstance>().Object, new Mock<IContainItemDefinitions>().Object);
            Assert.AreEqual(0, result.NumberComplete);
        }
    }
}