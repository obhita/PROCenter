namespace ProCenter.Domain.Tests.AssessmentModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using CommonModule;
    using Domain.AssessmentModule;
    using Domain.AssessmentModule.Event;
    using Infrastructure;
    using Infrastructure.EventStore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Pillar.Common.Tests;
    using Pillar.Common.Utility;
    using Pillar.Domain.Event;

    #endregion

    [TestClass]
    public class AssessmentInstanceTests
    {
        #region Public Methods and Operators

        private const string assessmentName = "TestName";

        [TestMethod]
        public void ShouldApplyCreatedEvent()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid defGuid = CombGuid.NewCombGuid();
                Guid patientGuid = CombGuid.NewCombGuid();
                var source = new AssessmentInstance(defGuid, patientGuid, assessmentName);

                // Verify
                Assert.AreEqual(1, events.Count);
                var createdEvent = events[0];
                Assert.IsNotNull(createdEvent);
                Assert.AreEqual(typeof (AssessmentCreatedEvent), createdEvent.GetType());
                Assert.AreEqual((createdEvent as AssessmentCreatedEvent).PatientKey, patientGuid);
                Assert.AreEqual((createdEvent as AssessmentCreatedEvent).AssessmentName, assessmentName);
                Assert.AreEqual(1, source.Version);
            }
        }

        [TestMethod]
        public void ShouldApplyItemAddedEvent()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid defGuid = CombGuid.NewCombGuid();
                Guid patientGuid = CombGuid.NewCombGuid();
                var source = new AssessmentInstance(defGuid, patientGuid, assessmentName);
                source.UpdateItem("", 0);

                // Verify
                Assert.AreEqual(2, events.Count);
                var itemUpdatedEvent = events[1];
                Assert.IsNotNull(itemUpdatedEvent);
                Assert.AreEqual(typeof (ItemUpdatedEvent), itemUpdatedEvent.GetType());
                Assert.AreEqual((itemUpdatedEvent as ItemUpdatedEvent).Value, 0);
                Assert.AreEqual(2, source.Version);
            }
        }

        [TestMethod]
        public void ShouldApplySubmittedEvent()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid defGuid = CombGuid.NewCombGuid();
                Guid patientGuid = CombGuid.NewCombGuid();
                var source = new AssessmentInstance(defGuid, patientGuid, assessmentName);
                source.Submit();

                // Verify
                Assert.AreEqual(2, events.Count);
                var submittedEvent = events[1];
                Assert.IsNotNull(submittedEvent);
                Assert.AreEqual(typeof (AssessmentSubmittedEvent), submittedEvent.GetType());
                Assert.AreEqual((submittedEvent as AssessmentSubmittedEvent).Submit, true);
                Assert.AreEqual(2, source.Version);
            }
        }

        [TestMethod]
        public void ShouldApplyScoredEvent()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid defGuid = CombGuid.NewCombGuid();
                Guid patientGuid = CombGuid.NewCombGuid();
                var source = new AssessmentInstance(defGuid, patientGuid, assessmentName);
                source.ScoreComplete(new CodedConcept(CodeSystems.Obhita, "dummayCode", ""), "result");

                // Verify
                Assert.AreEqual(2, events.Count);
                var scoredEvent = events[1];
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(typeof (AssessmentScoredEvent), scoredEvent.GetType());
                Assert.AreEqual((scoredEvent as AssessmentScoredEvent).Value, "result");
                Assert.AreEqual((scoredEvent as AssessmentScoredEvent).ScoreCode.Code, "dummayCode");
                Assert.IsNull((scoredEvent as AssessmentScoredEvent).Guidance);
                Assert.AreEqual(2, source.Version);
            }
        }


        [TestMethod]
        public void ShouldApplyAddedToWorkflowEvent()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid defGuid = CombGuid.NewCombGuid();
                Guid patientGuid = CombGuid.NewCombGuid();
                var source = new AssessmentInstance(defGuid, patientGuid, assessmentName);
                Guid workflowKey = CombGuid.NewCombGuid();
                source.AddToWorkflow(workflowKey);

                // Verify
                Assert.AreEqual(2, events.Count);
                var addedToWorkflowEvent = events[1];
                Assert.IsNotNull(addedToWorkflowEvent);
                Assert.AreEqual(typeof (AssessmentAddedToWorkflowEvent), addedToWorkflowEvent.GetType());
                Assert.AreEqual((addedToWorkflowEvent as AssessmentAddedToWorkflowEvent).WorkflowKey, workflowKey);
                Assert.AreEqual(2, source.Version);
            }
        }


        [TestInitialize]
        public void TestInitialize()
        {
        }

        #endregion

        #region Methods

        private static void SetupServiceLocatorFixture(ServiceLocatorFixture serviceLocatorFixture)
        {
            serviceLocatorFixture.StructureMapContainer.Configure(
                c => c.For<ICommitDomainEventService>().Singleton().Use<CommitDomainEventService>());
            serviceLocatorFixture.StructureMapContainer.Configure(
                c => c.For<IUnitOfWorkProvider>().Use<UnitOfWorkProvider>());
            serviceLocatorFixture.StructureMapContainer.Configure(
                c => c.For<IUnitOfWork>().Use(new Mock<IUnitOfWork>().Object));
        }

        #endregion
    }
}