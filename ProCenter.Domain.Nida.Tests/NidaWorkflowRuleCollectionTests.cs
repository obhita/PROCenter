namespace ProCenter.Domain.Nida.Tests
{
    #region

    using System;
    using System.Linq;
    using AssessmentModule;
    using CommonModule;
    using Infrastructure;
    using Infrastructure.Domain;
    using Infrastructure.EventStore;
    using MessageModule;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Pillar.Common.Tests;
    using Pillar.FluentRuleEngine;

    #endregion

    [TestClass]
    public class NidaWorkflowRuleCollectionTests
    {
        [TestMethod]
        public void NidaSingleQuestionRuleSetContainsCorrectRules()
        {
            var ruleCollection = new NidaWorkflowRuleCollection(new Mock<IAssessmentDefinitionRepository>().Object, new Mock<IWorkflowMessageRepository>().Object, new Mock<IAssessmentInstanceRepository>().Object);

            Assert.AreEqual(1, ruleCollection.NidaSingleQuestionScreenerRuleSet.Count());
            Assert.AreEqual(ruleCollection.ShouldRecommendDastRule, ruleCollection.NidaSingleQuestionScreenerRuleSet.First());
        }

        [TestMethod]
        public void NidaDrugAbuseScreeningTestRuleSetContainsCorrectRules()
        {
            var ruleCollection = new NidaWorkflowRuleCollection(new Mock<IAssessmentDefinitionRepository>().Object, new Mock<IWorkflowMessageRepository>().Object, new Mock<IAssessmentInstanceRepository>().Object);

            Assert.AreEqual(1, ruleCollection.DrugAbuseScreeningTestRuleSet.Count());
            Assert.AreEqual(ruleCollection.ShouldRecommendNidaAssessFurtherRule, ruleCollection.DrugAbuseScreeningTestRuleSet.First());
        }

        [TestMethod]
        public void AllRulesAndRuleSetsDefined()
        {
            var ruleCollection = new NidaWorkflowRuleCollection(new Mock<IAssessmentDefinitionRepository>().Object, new Mock<IWorkflowMessageRepository>().Object, new Mock<IAssessmentInstanceRepository>().Object);

            foreach (var propertyInfo in typeof (NidaWorkflowRuleCollection).GetProperties())
            {
                Assert.IsNotNull(propertyInfo.GetMethod.Invoke(ruleCollection, null), string.Format("Property {0} is null.", propertyInfo.Name));
            }
        }

        [TestMethod]
        public void ExecuteShouldRecommendDastRuleThenClause_WorkflowMessageCreated()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);
                var assessmentDefKey = Guid.NewGuid();
                var ruleCollection =
                    new NidaWorkflowRuleCollection(
                        BuildIAssessmentDefinitionRepositoryMock(assessmentDefKey).Object,
                        BuildIRepositoryMock(null).Object,
                        new Mock<IAssessmentInstanceRepository>().Object);

                var assessmentInstance = new AssessmentInstance(Guid.NewGuid(), Guid.NewGuid(), "");
                assessmentInstance.ScoreComplete(new CodedConcept(new CodeSystem("", "", ""), "", ""), "test");
                var messageCollector = new MessageCollector();
                var ruleEngineContext = new RuleEngineContext<AssessmentInstance>(assessmentInstance);
                ruleEngineContext.WorkingMemory.AddContextObject<IMessageCollector>(messageCollector, "MessageCollector");

                foreach (var action in ruleCollection.ShouldRecommendDastRule.ThenClauses)
                {
                    action(ruleEngineContext);
                }

                Assert.AreEqual(1, messageCollector.Messages.Count(), "Incorrect Number of messages.");
                Assert.AreEqual(typeof (WorkflowMessage), messageCollector.Messages.First().GetType());
                var workflowMessage = messageCollector.Messages.First() as WorkflowMessage;
                Assert.AreEqual(assessmentInstance.Key, workflowMessage.InitiatingAssessmentKey);
                Assert.AreEqual(DrugAbuseScreeningTest.AssessmentCodedConcept.Code, workflowMessage.RecommendedAssessmentDefinitionCode);
            }
        }

        [TestMethod]
        public void ExecuteShouldRecommendNidaAssessFurtherRuleThenClause_WorkflowMessageCreated()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);
                var assessmentDefKey = Guid.NewGuid();

                var assessmentInstance = new AssessmentInstance(Guid.NewGuid(), Guid.NewGuid(), "");
                var workflowMessage = new WorkflowMessage(Guid.NewGuid(),
                                                          Guid.NewGuid(),
                                                          string.Empty,
                                                          assessmentInstance.AssessmentDefinitionKey,
                                                          DrugAbuseScreeningTest.AssessmentCodedConcept.Code,
                                                          null);
                workflowMessage.AdministerAssessment();
                assessmentInstance.AddToWorkflow(workflowMessage.Key);
                var ruleCollection =
                    new NidaWorkflowRuleCollection(
                        BuildIAssessmentDefinitionRepositoryMock(assessmentDefKey).Object,
                        BuildIRepositoryMock(workflowMessage).Object,
                        new Mock<IAssessmentInstanceRepository>().Object);
                assessmentInstance.ScoreComplete(new CodedConcept(new CodeSystem("", "", ""), "", ""), "test");
                var messageCollector = new MessageCollector();
                var ruleEngineContext = new RuleEngineContext<AssessmentInstance>(assessmentInstance);
                ruleEngineContext.WorkingMemory.AddContextObject<IMessageCollector>(messageCollector, "MessageCollector");

                foreach (var action in ruleCollection.ShouldRecommendNidaAssessFurtherRule.ThenClauses)
                {
                    action(ruleEngineContext);
                }

                Assert.AreEqual(1, messageCollector.Messages.Count(), "Incorrect Number of messages.");
                Assert.AreEqual(typeof (WorkflowMessage), messageCollector.Messages.First().GetType());
                Assert.AreSame(workflowMessage, messageCollector.Messages.First());
                Assert.AreEqual(NidaAssessFurther.AssessmentCodedConcept.Code, (messageCollector.Messages.First() as WorkflowMessage).RecommendedAssessmentDefinitionCode);
            }
        }

        private Mock<IAssessmentDefinitionRepository> BuildIAssessmentDefinitionRepositoryMock(Guid key)
        {
            var mock = new Mock<IAssessmentDefinitionRepository>();
            mock.Setup(r => r.GetKeyByCode(It.IsAny<string>())).Returns(key);
            return mock;
        }

        private Mock<IWorkflowMessageRepository> BuildIRepositoryMock(WorkflowMessage message)
        {
            var mock = new Mock<IWorkflowMessageRepository>();
            mock.Setup(r => r.GetByKey(It.IsAny<Guid>())).Returns(message);
            return mock;
        }

        private static void SetupServiceLocatorFixture(ServiceLocatorFixture serviceLocatorFixture)
        {
            serviceLocatorFixture.StructureMapContainer.Configure(
                c => c.For<ICommitDomainEventService>().Singleton().Use<CommitDomainEventService>());
            serviceLocatorFixture.StructureMapContainer.Configure(
                c => c.For<IUnitOfWorkProvider>().Use<UnitOfWorkProvider>());
            serviceLocatorFixture.StructureMapContainer.Configure(
                c => c.For<IUnitOfWork>().Use(new Mock<IUnitOfWork>().Object));
        }
    }
}