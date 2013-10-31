namespace ProCenter.Domain.Tests.MessageModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommonModule;
    using Domain.AssessmentModule;
    using Domain.MessageModule;
    using Domain.MessageModule.Event;
    using Infrastructure;
    using Infrastructure.EventStore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Pillar.Common.Tests;
    using Pillar.Domain.Event;

    #endregion

    [TestClass]
    public class WorkflowMessageTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void CallAdministerAssessment_WorkflowMessageStatusChangedEventRaised ()
        {
            using ( var serviceLocatorFixture = new ServiceLocatorFixture () )
            {
                // Setup
                SetupServiceLocatorFixture ( serviceLocatorFixture );

                var workflowMessage = new WorkflowMessage ( Guid.NewGuid (), Guid.NewGuid (), string.Empty, Guid.NewGuid (), string.Empty, new Score () );

                var events = new List<IDomainEvent> ();
                CommitEvent.RegisterAll ( events.Add );

                workflowMessage.AdministerAssessment ();

                Assert.AreEqual ( 1, events.Count (), "Incorrect number of events." );
                Assert.AreEqual ( typeof(WorkflowMessageStatusChangedEvent), events.First ().GetType () );
                Assert.AreEqual ( WorkflowMessageStatus.InProgress, ( events.First () as WorkflowMessageStatusChangedEvent ).Status );
            }
        }

        [TestMethod]
        public void CallAdvance_CorrectEventsRaised ()
        {
            using ( var serviceLocatorFixture = new ServiceLocatorFixture () )
            {
                // Setup
                SetupServiceLocatorFixture ( serviceLocatorFixture );

                var workflowMessage = new WorkflowMessage ( Guid.NewGuid (), Guid.NewGuid (), string.Empty, Guid.NewGuid (), string.Empty, new Score () );

                workflowMessage.AdministerAssessment ();

                var events = new List<IDomainEvent> ();
                CommitEvent.RegisterAll ( events.Add );

                var assessmentKey = Guid.NewGuid ();
                var assessmentCode = Guid.NewGuid ().ToString ();
                var recommendedAssessmentKey = Guid.NewGuid ();
                var recommendedAssessmentCode = Guid.NewGuid ().ToString ();
                var score = new Score ();
                workflowMessage.Advance ( assessmentKey, assessmentCode, recommendedAssessmentKey, recommendedAssessmentCode, score );

                Assert.AreEqual ( 2, events.Count (), "Incorrect number of events." );
                Assert.IsTrue ( events.Any ( a => a.GetType () == typeof(WorkflowMessageStatusChangedEvent) ), "Status Changed event not raised." );
                Assert.IsTrue ( events.Any ( a => a.GetType () == typeof(WorkflowMessageAdvancedEvent) ), "Advanced Event not raised." );
                Assert.IsTrue (
                               ( events.First ( a => a.GetType () == typeof(WorkflowMessageStatusChangedEvent) ) as WorkflowMessageStatusChangedEvent ).Status ==
                               WorkflowMessageStatus.WaitingForResponse,
                               "Status event has wrong status." );
                var advanceEvent = events.First ( a => a.GetType () == typeof(WorkflowMessageAdvancedEvent) ) as WorkflowMessageAdvancedEvent;
                Assert.AreEqual ( assessmentKey, advanceEvent.InitiatingAssessmentKey, "Advance event has wrong InitiatingAssessmentKey." );
                Assert.AreEqual ( assessmentCode, advanceEvent.InitiatingAssessmentCode, "Advance event has wrong InitiatingAssessmentCode." );
                Assert.AreEqual ( recommendedAssessmentKey, advanceEvent.RecommendedAssessmentDefinitionKey, "Advance event has wrong RecommendedAssessmentDefinitionKey." );
                Assert.AreEqual ( recommendedAssessmentCode, advanceEvent.RecommendedAssessmentDefinitionCode, "Advance event has wrong RecommendedAssessmentDefinitionCode." );
                Assert.AreSame ( score, advanceEvent.InitiatingAssessmentScore, "Advance event has wrong InitiatingAssessmentScore." );
                Assert.AreEqual ( assessmentKey, workflowMessage.InitiatingAssessmentKey, "Worflow message has wrong InitiatingAssessmentKey." );
                Assert.AreEqual ( assessmentCode, workflowMessage.InitiatingAssessmentCode, "Worflow message has wrong InitiatingAssessmentCode." );
                Assert.AreEqual ( recommendedAssessmentKey, workflowMessage.RecommendedAssessmentDefinitionKey, "Worflow message has wrong RecommendedAssessmentDefinitionKey." );
                Assert.AreEqual ( recommendedAssessmentCode, workflowMessage.RecommendedAssessmentDefinitionCode, "Worflow message has wrong RecommendedAssessmentDefinitionCode." );
                Assert.AreSame ( score, workflowMessage.InitiatingAssessmentScore, "Worflow message has wrong InitiatingAssessmentScore." );
            }
        }

        [TestMethod]
        public void CallConstructor_WorkflowMessageCreatedEventRaised ()
        {
            using ( var serviceLocatorFixture = new ServiceLocatorFixture () )
            {
                // Setup
                SetupServiceLocatorFixture ( serviceLocatorFixture );

                var events = new List<IDomainEvent> ();
                CommitEvent.RegisterAll ( events.Add );

                var workflow = new WorkflowMessage ( Guid.NewGuid (), Guid.NewGuid (), string.Empty, Guid.NewGuid (), string.Empty, new Score () );

                Assert.AreEqual ( 1, events.Count (), "Incorrect number of events." );
                Assert.AreEqual ( typeof(WorkflowMessageCreatedEvent), events.First ().GetType () );
            }
        }

        [TestMethod]
        public void CallReject_WorkflowMessageStatusChangedEventRaised ()
        {
            using ( var serviceLocatorFixture = new ServiceLocatorFixture () )
            {
                // Setup
                SetupServiceLocatorFixture ( serviceLocatorFixture );

                var workflowMessage = new WorkflowMessage ( Guid.NewGuid (), Guid.NewGuid (), string.Empty, Guid.NewGuid (), string.Empty, new Score () );

                var events = new List<IDomainEvent> ();
                CommitEvent.RegisterAll ( events.Add );

                workflowMessage.Reject ();

                Assert.AreEqual ( 1, events.Count (), "Incorrect number of events." );
                Assert.AreEqual ( typeof(WorkflowMessageStatusChangedEvent), events.First ().GetType () );
                Assert.AreEqual ( WorkflowMessageStatus.Rejected, ( events.First () as WorkflowMessageStatusChangedEvent ).Status );
            }
        }

        [TestMethod]
        public void WorkflowStatesFlowCorrectly ()
        {
            using ( var serviceLocatorFixture = new ServiceLocatorFixture () )
            {
                // Setup
                SetupServiceLocatorFixture ( serviceLocatorFixture );

                var events = new List<IDomainEvent> ();
                CommitEvent.RegisterAll ( events.Add );

                var workflow = new WorkflowMessage(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid().ToString(), Guid.NewGuid(), Guid.NewGuid().ToString(), new Score());

                Assert.AreEqual ( WorkflowMessageStatus.WaitingForResponse, workflow.Status );

                events.Clear ();
                workflow.Advance(Guid.NewGuid(), Guid.NewGuid().ToString(), Guid.NewGuid(), Guid.NewGuid().ToString(), new Score());

                Assert.AreEqual ( 0, events.Count () );
                Assert.AreEqual ( WorkflowMessageStatus.WaitingForResponse, workflow.Status );

                workflow.AdministerAssessment ();

                Assert.AreEqual ( WorkflowMessageStatus.InProgress, workflow.Status );

                events.Clear ();
                workflow.Reject ();

                Assert.AreEqual ( 0, events.Count () );
                Assert.AreEqual ( WorkflowMessageStatus.InProgress, workflow.Status );

                events.Clear ();
                workflow.AdministerAssessment ();

                Assert.AreEqual ( 0, events.Count () );
                Assert.AreEqual ( WorkflowMessageStatus.InProgress, workflow.Status );

                workflow.Advance(Guid.NewGuid(), Guid.NewGuid().ToString(), Guid.NewGuid(), Guid.NewGuid().ToString(), new Score());

                Assert.AreEqual ( WorkflowMessageStatus.WaitingForResponse, workflow.Status );

                workflow.Reject ();

                Assert.AreEqual ( WorkflowMessageStatus.Rejected, workflow.Status );

                events.Clear ();
                workflow.Reject ();

                Assert.AreEqual ( 0, events.Count () );
                Assert.AreEqual ( WorkflowMessageStatus.Rejected, workflow.Status );

                events.Clear ();
                workflow.AdministerAssessment ();

                Assert.AreEqual ( 0, events.Count () );
                Assert.AreEqual ( WorkflowMessageStatus.Rejected, workflow.Status );

                events.Clear ();
                workflow.Advance(Guid.NewGuid(), Guid.NewGuid().ToString(), Guid.NewGuid(), Guid.NewGuid().ToString(), new Score());

                Assert.AreEqual ( 0, events.Count () );
                Assert.AreEqual ( WorkflowMessageStatus.Rejected, workflow.Status );
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
        }

        #endregion
    }
}