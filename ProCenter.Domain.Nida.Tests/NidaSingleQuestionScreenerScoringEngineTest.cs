namespace ProCenter.Domain.Nida.Tests
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using AssessmentModule.Event;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Pillar.Common.Tests;
    using AssessmentModule;
    using CommonModule;
    using Infrastructure;
    using Infrastructure.EventStore;
    using Moq;
    using Pillar.Common.Utility;
    using Pillar.Domain.Event;

    #endregion

    [TestClass]
    public class NidaSingleQuestionScreenerScoringEngineTest
    {
        [TestMethod]
        public void CalculateScoreShouldRaiseScoredEvent()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                const int itemValue = 2;
               
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid defGuid = CombGuid.NewCombGuid();
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessment = new AssessmentInstance(defGuid, patientGuid, "TestName");
                assessment.UpdateItem("", itemValue);

                var nidaSingleQuestionScreenerScoringEngine = new NidaSingleQuestionScreenerScoringEngine();
                nidaSingleQuestionScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                Assert.AreEqual(3, events.Count);
                var scoredEvent = events[2];
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(typeof(AssessmentScoredEvent), scoredEvent.GetType());
                Assert.AreEqual((scoredEvent as AssessmentScoredEvent).Value, itemValue);
                Assert.AreEqual((scoredEvent as AssessmentScoredEvent).Guidance.Code, "guidance_1_and_up");
                Assert.AreEqual(3, assessment.Version);
            }
        }


        #region Methods

        private static void SetupServiceLocatorFixture(ServiceLocatorFixture serviceLocatorFixture)
        {
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<ICommitDomainEventService>().Singleton().Use<CommitDomainEventService>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IUnitOfWorkProvider>().Use<UnitOfWorkProvider>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IUnitOfWork>().Use(new Mock<IUnitOfWork>().Object));
        }

        #endregion
    }
}