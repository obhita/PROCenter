#region Licence Header
// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/
#endregion
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