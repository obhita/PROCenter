namespace ProCenter.Domain.Nida.Tests
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
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
    using CommonModule.Lookups;

    #endregion

    [TestClass]
    public class NidaAssessFurtherScoringEngineTest
    {
        [TestMethod]
        public void CalculateScoreShouldReturnFalseWhenNoCriteriaMet()
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
                var assessment = new AssessmentInstance(defGuid, patientGuid, "TestName");
                assessment.UpdateItem("", "");

                var nidaAssessFurtherScoringEngine = new NidaAssessFurtherScoringEngine();
                nidaAssessFurtherScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, false);
            }
        }

        [TestMethod]
        public void CalculateScoreShouldReturnTrueWhenHasDilyuseOfAnySubstance()
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
                var assessment = new AssessmentInstance(defGuid, patientGuid, "TestName");
                assessment.UpdateItem("3269979",
                                      new Lookup(new CodedConcept(CodeSystems.Obhita, "", ""), Frequency.DailyOrAlmostDaily.Value));

                var nidaAssessFurtherScoringEngine = new NidaAssessFurtherScoringEngine();
                nidaAssessFurtherScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, true);
            }
        }

        [TestMethod]
        public void CalculateScoreShouldReturnTrueWhenHasWeeklyUseOpioidsCocaineMethamphetamine()
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
                var assessment = new AssessmentInstance(defGuid, patientGuid, "TestName");
                assessment.UpdateItem("3269981",
                                      new Lookup(new CodedConcept(CodeSystems.Obhita, "", ""), Frequency.Weekly.Value));

                var nidaAssessFurtherScoringEngine = new NidaAssessFurtherScoringEngine();
                nidaAssessFurtherScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, true);
            }
        }

        [TestMethod]
        public void CalculateScoreShouldReturnTrueWhenHasInjectionDrugUseInPastThreeMonths()
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
                var assessment = new AssessmentInstance(defGuid, patientGuid, "TestName");
                assessment.UpdateItem("3269978", true);
                assessment.UpdateItem("3269986",
                                      new Lookup(new CodedConcept(CodeSystems.Obhita, "", ""), Frequency.InThePast90Days.Value));

                var nidaAssessFurtherScoringEngine = new NidaAssessFurtherScoringEngine();
                nidaAssessFurtherScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, true);
            }
        }

        [TestMethod]
        public void CalculateScoreShouldReturnFalseWhenHasNoInjectionDrugUse()
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
                var assessment = new AssessmentInstance(defGuid, patientGuid, "TestName");
                assessment.UpdateItem("3269978", false);

                var nidaAssessFurtherScoringEngine = new NidaAssessFurtherScoringEngine();
                nidaAssessFurtherScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, false);
            }
        }

        [TestMethod]
        public void CalculateScoreShouldReturnFalseWhenHasNoInjectionDrugUseInPastThreeMonths()
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
                var assessment = new AssessmentInstance(defGuid, patientGuid, "TestName");
                assessment.UpdateItem("3269978", true);
                assessment.UpdateItem("3269986",
                                      new Lookup(new CodedConcept(CodeSystems.Obhita, "", ""), Frequency.InThePastYear.Value));

                var nidaAssessFurtherScoringEngine = new NidaAssessFurtherScoringEngine();
                nidaAssessFurtherScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, false);
            }
        }

        [TestMethod]
        public void CalculateScoreShouldReturnTrueWhenCurrentlyInDrugAbuseTreatment()
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
                var assessment = new AssessmentInstance(defGuid, patientGuid, "TestName");
                assessment.UpdateItem("3269976", true);

                var nidaAssessFurtherScoringEngine = new NidaAssessFurtherScoringEngine();
                nidaAssessFurtherScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, true);
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