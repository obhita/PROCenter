namespace ProCenter.Domain.Tests.AssessmentModule
{
    #region Using Statements

    using System.Collections.Generic;
    using CommonModule;
    using CommonModule.Lookups;
    using Domain.AssessmentModule;
    using Domain.AssessmentModule.Event;
    using Domain.AssessmentModule.Lookups;
    using Infrastructure;
    using Infrastructure.EventStore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Pillar.Common.Tests;
    using Pillar.Domain.Event;

    #endregion

    [TestClass]
    public class AssessmentDefinitionTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void ShouldApplyCreatedEvent ()
        {
            using ( var serviceLocatorFixture = new ServiceLocatorFixture () )
            {
                // Setup
                SetupServiceLocatorFixture ( serviceLocatorFixture );

                var events = new List<IDomainEvent> ();
                CommitEvent.RegisterAll ( events.Add );

                // Exercise
                const string testCode = "testCode";
                var source = new AssessmentDefinition ( new CodedConcept ( CodeSystems.Nci, testCode, "Test" ) );

                // Verify
                Assert.AreEqual ( 1, events.Count );
                var createdEvent = events[0];
                Assert.IsNotNull ( createdEvent );
                Assert.AreEqual ( typeof(AssessmentDefinitionCreatedEvent), createdEvent.GetType () );
                Assert.AreEqual ( ( createdEvent as AssessmentDefinitionCreatedEvent ).CodedConcept.Code, testCode );
                Assert.AreEqual ( 1, source.Version );
            }
        }

        [TestMethod]
        public void ShouldApplyItemAddedEvent ()
        {
            using ( var serviceLocatorFixture = new ServiceLocatorFixture () )
            {
                // Setup
                SetupServiceLocatorFixture ( serviceLocatorFixture );

                var events = new List<IDomainEvent> ();
                CommitEvent.RegisterAll ( events.Add );

                // Exercise
                const string testCode = "testCode";
                var source = new AssessmentDefinition ( new CodedConcept ( CodeSystems.Nci, testCode, "Test" ) );
                source.AddItemDefinition ( new ItemDefinition ( new CodedConcept ( CodeSystems.Nci, testCode, "Test" ), ItemType.Question, new Lookup (new CodedConcept ( CodeSystems.Nci, testCode, "Test")) ) );

                // Verify
                Assert.AreEqual ( 2, events.Count );
                var itemDefinitionAddedEvent = events[1];
                Assert.IsNotNull ( itemDefinitionAddedEvent );
                Assert.AreEqual ( typeof(ItemDefinitionAddedEvent), itemDefinitionAddedEvent.GetType () );
                Assert.AreEqual ( ( itemDefinitionAddedEvent as ItemDefinitionAddedEvent ).ItemDefinition.CodedConcept.Code, testCode );
                Assert.AreEqual ( 2, source.Version );
            }
        }

        [TestInitialize]
        public void TestInitialize ()
        {
        }

        #endregion

        #region Methods

        private static void SetupServiceLocatorFixture ( ServiceLocatorFixture serviceLocatorFixture )
        {
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<ICommitDomainEventService>().Singleton ().Use<CommitDomainEventService>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IUnitOfWorkProvider>().Use<UnitOfWorkProvider>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IUnitOfWork>().Use( new Mock<IUnitOfWork>().Object));
        }

        #endregion
    }
}