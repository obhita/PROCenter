using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProCenter.Domain.Tests.AssessmentModule.Rules
{
    using Moq;

    using NSubstitute;

    using Pillar.Common.Tests;
    using Pillar.FluentRuleEngine;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Rules;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Infrastructure;
    using ProCenter.Infrastructure.EventStore;

    [TestClass]
    public class AssessmentRuleEngineExecutorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateRuleEngineExecutor_NullAssessmentInstance_ThrowsException()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                AssessmentRuleEngineExecutor.CreateRuleEngineExecutor(null);
            }
        }

        [TestMethod]
        public void CreateRuleEngineExecutor_ValidAssessmentInstance_CreatesRuleEngineExecutor()
        {
            using ( var serviceLocatorFixture = new ServiceLocatorFixture () )
            {
                // Setup
                SetupServiceLocatorFixture ( serviceLocatorFixture );

                var ruleEngineExecutor = AssessmentRuleEngineExecutor.CreateRuleEngineExecutor ( Substitute.For<AssessmentInstance> () );

                Assert.IsNotNull(ruleEngineExecutor);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ForItemInstance_NullItemInstance_ThrowsException ()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var ruleEngineExecutor = AssessmentRuleEngineExecutor.CreateRuleEngineExecutor(Substitute.For<AssessmentInstance>());

                ruleEngineExecutor.ForItemInstance ( null );
            }
        }

        [TestMethod]
        public void ForItemInstance_ValidItemInstance_CallsForRuleSetCorrectly()
        {
            using ( var serviceLocatorFixture = new ServiceLocatorFixture () )
            {
                // Setup
                SetupServiceLocatorFixture ( serviceLocatorFixture );

                var itemInstance = new ItemInstance ( "1", null, true );
                var ruleEngineExecutor = Substitute.For<RuleEngineExecutor<AssessmentInstance>> ( Substitute.For<AssessmentInstance> () );
                
                ruleEngineExecutor.ForItemInstance ( itemInstance );

                ruleEngineExecutor.Received().ForRuleSet(Arg.Is("ForItemInstance_ValidItemInstance_CallsForRuleSetCorrectlyRuleSet" + itemInstance.ItemDefinitionCode) ?? "Test");
            }
        }

        private static void SetupServiceLocatorFixture(ServiceLocatorFixture serviceLocatorFixture)
        {
            serviceLocatorFixture.StructureMapContainer.Configure(
                c => c.For<ICommitDomainEventService>().Singleton().Use<CommitDomainEventService>());
            serviceLocatorFixture.StructureMapContainer.Configure(
                c => c.For<IUnitOfWorkProvider>().Use<UnitOfWorkProvider>());
            serviceLocatorFixture.StructureMapContainer.Configure(
                c => c.For<IUnitOfWork>().Use(Substitute.For<IUnitOfWork>()));
            serviceLocatorFixture.StructureMapContainer.Configure(
                c => c.For<IAssessmentRuleCollectionFactory>().Singleton().Use(Substitute.For<IAssessmentRuleCollectionFactory>()));
            serviceLocatorFixture.StructureMapContainer.Configure(
                c => c.For<IRuleEngineFactory>().Singleton().Use(Substitute.For<IRuleEngineFactory>()));
        }
    }
}
