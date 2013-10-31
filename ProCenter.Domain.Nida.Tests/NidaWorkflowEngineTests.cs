namespace ProCenter.Domain.Nida.Tests
{
    #region Using Statements

    using System;
    using System.Linq;
    using AssessmentModule;
    using Common;
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
    public class NidaWorkflowEngineTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void GivenDast_CorrectRuleSetExecuted ()
        {
            using ( var serviceLocatorFixture = new ServiceLocatorFixture () )
            {
                // Setup
                SetupServiceLocatorFixture ( serviceLocatorFixture );
                IRuleEngineContext ruleEngineContext = null;

                var ruleEngineMock = new Mock<IRuleEngine<AssessmentInstance>> ();
                ruleEngineMock.Setup ( re => re.ExecuteRules ( It.IsAny<IRuleEngineContext> () ) ).Callback<IRuleEngineContext> ( ctx => ruleEngineContext = ctx );

                var ruleEngineFactoryMock = new Mock<IRuleEngineFactory> ();
                ruleEngineFactoryMock.Setup ( f => f.CreateRuleEngine ( It.IsAny<AssessmentInstance> (), It.IsAny<IRuleCollection<AssessmentInstance>> () ) )
                                     .Returns ( ruleEngineMock.Object );

                var nidaWorkflowRuleCollection = serviceLocatorFixture.StructureMapContainer.GetInstance<NidaWorkflowRuleCollection> ();
                var nidaWorkflowEngine = new NidaWorkflowEngine ( null, ruleEngineFactoryMock.Object, new MessageCollector () );

                var assessmentInstance = new AssessmentInstance ( Guid.NewGuid (), Guid.NewGuid (), DrugAbuseScreeningTest.AssessmentCodedConcept.Name );

                nidaWorkflowEngine.Run ( assessmentInstance );

                Assert.IsNotNull ( ruleEngineContext );
                foreach ( var rule in ruleEngineContext.RuleSelector.SelectRules ( nidaWorkflowRuleCollection, ruleEngineContext ) )
                {
                    Assert.IsTrue ( nidaWorkflowRuleCollection.DrugAbuseScreeningTestRuleSet.Any ( r => r.Name == rule.Name ) );
                }
            }
        }

        [TestMethod]
        public void GivenNidaAssessFurther_CorrectRuleSetExecuted ()
        {
            using ( var serviceLocatorFixture = new ServiceLocatorFixture () )
            {
                // Setup
                SetupServiceLocatorFixture ( serviceLocatorFixture );
                IRuleEngineContext ruleEngineContext = null;

                var ruleEngineMock = new Mock<IRuleEngine<AssessmentInstance>> ();
                ruleEngineMock.Setup ( re => re.ExecuteRules ( It.IsAny<IRuleEngineContext> () ) ).Callback<IRuleEngineContext> ( ctx => ruleEngineContext = ctx );

                var ruleEngineFactoryMock = new Mock<IRuleEngineFactory> ();
                ruleEngineFactoryMock.Setup ( f => f.CreateRuleEngine ( It.IsAny<AssessmentInstance> (), It.IsAny<IRuleCollection<AssessmentInstance>> () ) )
                                     .Returns ( ruleEngineMock.Object );

                var nidaWorkflowRuleCollection = serviceLocatorFixture.StructureMapContainer.GetInstance<NidaWorkflowRuleCollection> ();
                var nidaWorkflowEngine = new NidaWorkflowEngine ( null, ruleEngineFactoryMock.Object, new MessageCollector () );

                var assessmentInstance = new AssessmentInstance ( Guid.NewGuid (), Guid.NewGuid (), NidaAssessFurther.AssessmentCodedConcept.Name );

                nidaWorkflowEngine.Run ( assessmentInstance );

                Assert.IsNotNull ( ruleEngineContext );
                foreach ( var rule in ruleEngineContext.RuleSelector.SelectRules ( nidaWorkflowRuleCollection, ruleEngineContext ) )
                {
                    Assert.IsTrue ( nidaWorkflowRuleCollection.NidaAssessFurtherRuleSet.Any ( r => r.Name == rule.Name ) );
                }
            }
        }

        [TestMethod]
        public void GivenNidaSingleQuestionScreener_CorrectRuleSetExecuted ()
        {
            using ( var serviceLocatorFixture = new ServiceLocatorFixture () )
            {
                // Setup
                SetupServiceLocatorFixture ( serviceLocatorFixture );
                IRuleEngineContext ruleEngineContext = null;

                var ruleEngineMock = new Mock<IRuleEngine<AssessmentInstance>> ();
                ruleEngineMock.Setup ( re => re.ExecuteRules ( It.IsAny<IRuleEngineContext> () ) ).Callback<IRuleEngineContext> ( ctx => ruleEngineContext = ctx );

                var ruleEngineFactoryMock = new Mock<IRuleEngineFactory> ();
                ruleEngineFactoryMock.Setup ( f => f.CreateRuleEngine ( It.IsAny<AssessmentInstance> (), It.IsAny<IRuleCollection<AssessmentInstance>> () ) )
                                     .Returns ( ruleEngineMock.Object );

                var nidaWorkflowRuleCollection = serviceLocatorFixture.StructureMapContainer.GetInstance<NidaWorkflowRuleCollection> ();
                var nidaWorkflowEngine = new NidaWorkflowEngine ( null, ruleEngineFactoryMock.Object, new MessageCollector () );

                var assessmentInstance = new AssessmentInstance ( Guid.NewGuid (), Guid.NewGuid (), NidaSingleQuestionScreener.AssessmentCodedConcept.Name );

                nidaWorkflowEngine.Run ( assessmentInstance );

                Assert.IsNotNull ( ruleEngineContext );
                foreach ( var rule in ruleEngineContext.RuleSelector.SelectRules ( nidaWorkflowRuleCollection, ruleEngineContext ) )
                {
                    Assert.IsTrue ( nidaWorkflowRuleCollection.NidaSingleQuestionScreenerRuleSet.Any ( r => r.Name == rule.Name ) );
                }
            }
        }

        #endregion

        #region Methods

        private static void SetupServiceLocatorFixture ( ServiceLocatorFixture serviceLocatorFixture )
        {
            serviceLocatorFixture.StructureMapContainer.Configure (
                                                                   c => c.For<ICommitDomainEventService> ().Singleton ().Use<CommitDomainEventService> () );
            serviceLocatorFixture.StructureMapContainer.Configure (
                                                                   c => c.For<IUnitOfWorkProvider> ().Use<UnitOfWorkProvider> () );
            serviceLocatorFixture.StructureMapContainer.Configure (
                                                                   c => c.For<IUnitOfWork> ().Use ( new Mock<IUnitOfWork> ().Object ) );
            serviceLocatorFixture.StructureMapContainer.Configure (
                                                                   c => c.For<IAssessmentDefinitionRepository>().Use(new Mock<IAssessmentDefinitionRepository>().Object));
            serviceLocatorFixture.StructureMapContainer.Configure(
                                                                   c => c.For<IAssessmentInstanceRepository>().Use(new Mock<IAssessmentInstanceRepository>().Object));
            serviceLocatorFixture.StructureMapContainer.Configure (
                                                                   c => c.For<IWorkflowMessageRepository>().Use(new Mock<IWorkflowMessageRepository>().Object));
            serviceLocatorFixture.StructureMapContainer.Configure(
                                                                   c => c.For<IResourcesManager>().Use(new Mock<IResourcesManager>().Object));
        }

        #endregion
    }
}