namespace ProCenter.Domain.GainShortScreener.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;

    using AssessmentModule.Event;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using NSubstitute;

    using Pillar.Common.Tests;
    using Pillar.Common.Utility;
    using Pillar.Domain.Event;

    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Lookups;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.PatientModule;
    using ProCenter.Infrastructure;
    using ProCenter.Infrastructure.EventStore;

    [TestClass]
    public class GainShortScreenerScoringEngingTests
    {
        [TestMethod]
        public void CalculateScoreInternalDisorderScreenerAllPastMonthReturns6()
        {
            using ( var serviceLocatorFixture = new ServiceLocatorFixture () )
            {
                // Setup
                SetupServiceLocatorFixture ( serviceLocatorFixture );

                var events = new List<IDomainEvent> ();
                CommitEvent.RegisterAll ( events.Add );

                // Exercise
                var patientGuid = CombGuid.NewCombGuid ();
                var assessmentDefinition = Substitute.For<AssessmentDefinition> ();

                var assessment = new AssessmentInstanceFactory ().Create ( assessmentDefinition, patientGuid, "TestName" );
                assessment.UpdateItem (
                    new ItemDefinition (
                        new CodedConcept(new CodeSystem("6125007", "1", "Test 1"), "6125007", "Test 1"),
                        ItemType.Question,
                        null ),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125006", "1", "Test 2"), "6125006", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125003", "1", "Test 3"), "6125003", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125009", "1", "Test 4"), "6125009", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125005", "1", "Test 5"), "6125005", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125008", "1", "Test 6"), "6125008", "Test 6"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);

                var resourceManager = new Mock<IResourcesManager> ();
                var patientRepository = new Mock<IPatientRepository> ();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine( resourceManager.Object, patientRepository.Object );
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault ( e => e.GetType () == typeof(AssessmentScoredEvent) ) as AssessmentScoredEvent;
                Assert.IsNotNull ( scoredEvent );
                var pastMonthTotal = ((GainShortScreenerScore)(scoredEvent.Value)).InternalDisorder.PastMonth;
                Assert.AreEqual(pastMonthTotal, 6);
                Assert.AreEqual ( int.Parse ( scoredEvent.Value.ToString ()), 6 );
            }
        }

        [TestMethod]
        public void CalculateScoreInternalDisorderScreenerAllTwoToThreeMonthsReturns6()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125007", "1", "Test 1"), "6125007", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125006", "1", "Test 2"), "6125006", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125003", "1", "Test 3"), "6125003", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125009", "1", "Test 4"), "6125009", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125005", "1", "Test 5"), "6125005", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125008", "1", "Test 6"), "6125008", "Test 6"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var past90DaysTotal = ((GainShortScreenerScore)(scoredEvent.Value)).InternalDisorder.Past90Days;
                Assert.AreEqual(past90DaysTotal, 6);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 6);
            }
        }

        [TestMethod]
        public void CalculateScoreInternalDisorderScreenerAllFourToTwelveMonthsReturns6()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125007", "1", "Test 1"), "6125007", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125006", "1", "Test 2"), "6125006", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125003", "1", "Test 3"), "6125003", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125009", "1", "Test 4"), "6125009", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125005", "1", "Test 5"), "6125005", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125008", "1", "Test 6"), "6125008", "Test 6"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var pastYearTotal = ( (GainShortScreenerScore)( scoredEvent.Value ) ).InternalDisorder.PastYear;
                Assert.AreEqual(pastYearTotal, 6);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 6);
            }
        }

        [TestMethod]
        public void CalculateScoreInternalDisorderScreenerAllOnePlusYearsReturns6()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125007", "1", "Test 1"), "6125007", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125006", "1", "Test 2"), "6125006", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125003", "1", "Test 3"), "6125003", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125009", "1", "Test 4"), "6125009", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125005", "1", "Test 5"), "6125005", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125008", "1", "Test 6"), "6125008", "Test 6"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var onePlusYearsTotal = ((GainShortScreenerScore)(scoredEvent.Value)).InternalDisorder.Lifetime;
                Assert.AreEqual(onePlusYearsTotal, 6);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 6);
            }
        }

        [TestMethod]
        public void CalculateScoreInternalDisorderScreenerAllNeverReturns0()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125007", "1", "Test 1"), "6125007", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125006", "1", "Test 2"), "6125006", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125003", "1", "Test 3"), "6125003", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125009", "1", "Test 4"), "6125009", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125005", "1", "Test 5"), "6125005", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125008", "1", "Test 6"), "6125008", "Test 6"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var scoreObject = ( (GainShortScreenerScore)( scoredEvent.Value ) );
                var totalAll = scoreObject.InternalDisorder.PastMonth +
                               scoreObject.InternalDisorder.Past90Days +
                               scoreObject.InternalDisorder.PastYear +
                               scoreObject.InternalDisorder.Lifetime;
                Assert.AreEqual(totalAll, 0);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 0);
            }
        }

        [TestMethod]
        public void CalculateScoreExternalDisorderScreenerAllPastMonthReturns7()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125013", "1", "Test 1"), "6125013", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125012", "1", "Test 2"), "6125012", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125014", "1", "Test 3"), "6125014", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125011", "1", "Test 4"), "6125011", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125016", "1", "Test 5"), "6125016", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125017", "1", "Test 6"), "6125017", "Test 6"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125015", "1", "Test 7"), "6125015", "Test 7"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var pastMonthTotal = ((GainShortScreenerScore)(scoredEvent.Value)).ExternalDisorder.PastMonth;
                Assert.AreEqual(pastMonthTotal, 7); 
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 7);
            }
        }

        [TestMethod]
        public void CalculateScoreExternalDisorderScreenerAllTwoToThreeMonthsReturns7()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125013", "1", "Test 1"), "6125013", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125012", "1", "Test 2"), "6125012", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125014", "1", "Test 3"), "6125014", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125011", "1", "Test 4"), "6125011", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125016", "1", "Test 5"), "6125016", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125017", "1", "Test 6"), "6125017", "Test 6"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125015", "1", "Test 7"), "6125015", "Test 7"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var totalScore = ((GainShortScreenerScore)(scoredEvent.Value)).ExternalDisorder.Past90Days;
                Assert.AreEqual(totalScore, 7);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 7);
            }
        }

        [TestMethod]
        public void CalculateScoreExternalDisorderScreenerAllFourToTwelveMonthsReturns7()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125013", "1", "Test 1"), "6125013", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125012", "1", "Test 2"), "6125012", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125014", "1", "Test 3"), "6125014", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125011", "1", "Test 4"), "6125011", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125016", "1", "Test 5"), "6125016", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125017", "1", "Test 6"), "6125017", "Test 6"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125015", "1", "Test 7"), "6125015", "Test 7"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var totalScore = ((GainShortScreenerScore)(scoredEvent.Value)).ExternalDisorder.PastYear;
                Assert.AreEqual(totalScore, 7);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 7);
            }
        }

        [TestMethod]
        public void CalculateScoreExternalDisorderScreenerAllOnePlusYearsReturns7()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125013", "1", "Test 1"), "6125013", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125012", "1", "Test 2"), "6125012", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125014", "1", "Test 3"), "6125014", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125011", "1", "Test 4"), "6125011", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125016", "1", "Test 5"), "6125016", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125017", "1", "Test 6"), "6125017", "Test 6"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125015", "1", "Test 7"), "6125015", "Test 7"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var totalScore = ((GainShortScreenerScore)(scoredEvent.Value)).ExternalDisorder.Lifetime;
                Assert.AreEqual(totalScore, 7);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 7);
            }
        }

        [TestMethod]
        public void CalculateScoreExternalDisorderScreenerNeverReturns0()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125013", "1", "Test 1"), "6125013", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125012", "1", "Test 2"), "6125012", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125014", "1", "Test 3"), "6125014", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125011", "1", "Test 4"), "6125011", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125016", "1", "Test 5"), "6125016", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125017", "1", "Test 6"), "6125017", "Test 6"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125015", "1", "Test 7"), "6125015", "Test 7"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var scoreObject = ((GainShortScreenerScore)(scoredEvent.Value));
                var totalAll = scoreObject.InternalDisorder.PastMonth +
                               scoreObject.InternalDisorder.Past90Days +
                               scoreObject.InternalDisorder.PastYear +
                               scoreObject.InternalDisorder.Lifetime;
                Assert.AreEqual(totalAll, 0);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 0);
            }
        }

        [TestMethod]
        public void CalculateScoreCriminalViolenceDisorderScreenerAllPastMonthReturns5()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125028", "1", "Test 1"), "6125028", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125025", "1", "Test 2"), "6125025", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125029", "1", "Test 3"), "6125029", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125027", "1", "Test 4"), "6125027", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125026", "1", "Test 5"), "6125026", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var totalScore = ((GainShortScreenerScore)(scoredEvent.Value)).CriminalViolenceDisorder.PastMonth;
                Assert.AreEqual(totalScore, 5);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 5);
            }
        }

        [TestMethod]
        public void CalculateScoreCriminalViolenceDisorderScreenerAllTwoToThreeMonthsReturns5()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125028", "1", "Test 1"), "6125028", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125025", "1", "Test 2"), "6125025", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125029", "1", "Test 3"), "6125029", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125027", "1", "Test 4"), "6125027", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125026", "1", "Test 5"), "6125026", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var totalScore = ((GainShortScreenerScore)(scoredEvent.Value)).CriminalViolenceDisorder.Past90Days;
                Assert.AreEqual(totalScore, 5);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 5);
            }
        }

        [TestMethod]
        public void CalculateScoreCriminalViolenceDisorderScreenerAllFourToTwelveMonthsReturns5()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125028", "1", "Test 1"), "6125028", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125025", "1", "Test 2"), "6125025", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125029", "1", "Test 3"), "6125029", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125027", "1", "Test 4"), "6125027", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125026", "1", "Test 5"), "6125026", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var totalScore = ((GainShortScreenerScore)(scoredEvent.Value)).CriminalViolenceDisorder.PastYear;
                Assert.AreEqual(totalScore, 5);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 5);
            }
        }

        [TestMethod]
        public void CalculateScoreCriminalViolenceDisorderScreenerAllOnePlusYearsReturns5()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125028", "1", "Test 1"), "6125028", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125025", "1", "Test 2"), "6125025", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125029", "1", "Test 3"), "6125029", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125027", "1", "Test 4"), "6125027", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125026", "1", "Test 5"), "6125026", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var totalScore = ((GainShortScreenerScore)(scoredEvent.Value)).CriminalViolenceDisorder.Lifetime;
                Assert.AreEqual(totalScore, 5);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 5);
            }
        }

        [TestMethod]
        public void CalculateScoreCriminalViolenceDisorderScreenerAllNeverReturns0()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125028", "1", "Test 1"), "6125028", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125025", "1", "Test 2"), "6125025", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125029", "1", "Test 3"), "6125029", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125027", "1", "Test 4"), "6125027", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125026", "1", "Test 5"), "6125026", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var scoreObject = ((GainShortScreenerScore)(scoredEvent.Value));
                var totalAll = scoreObject.InternalDisorder.PastMonth +
                               scoreObject.InternalDisorder.Past90Days +
                               scoreObject.InternalDisorder.PastYear +
                               scoreObject.InternalDisorder.Lifetime;
                Assert.AreEqual(totalAll, 0);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 0);
            }
        }

        [TestMethod]
        public void CalculateScoreSubstanceDisorderScreenerAllPastMonthReturns5()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125021", "1", "Test 1"), "6125021", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125020", "1", "Test 2"), "6125020", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125019", "1", "Test 3"), "6125019", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125023", "1", "Test 4"), "6125023", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125022", "1", "Test 5"), "6125022", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.PastMonth);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var totalScore = ((GainShortScreenerScore)(scoredEvent.Value)).SubstanceDisorder.PastMonth;
                Assert.AreEqual(totalScore, 5);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 5);
            }
        }

        [TestMethod]
        public void CalculateScoreSubstanceDisorderScreenerAllTwoToThreeMonthsReturns5()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125021", "1", "Test 1"), "6125021", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125020", "1", "Test 2"), "6125020", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125019", "1", "Test 3"), "6125019", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125023", "1", "Test 4"), "6125023", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125022", "1", "Test 5"), "6125022", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.TwoToThreeMonths);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var totalScore = ((GainShortScreenerScore)(scoredEvent.Value)).SubstanceDisorder.Past90Days;
                Assert.AreEqual(totalScore, 5);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 5);
            }
        }

        [TestMethod]
        public void CalculateScoreSubstanceDisorderScreenerAllFourToTwelveMonthsReturns5()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125021", "1", "Test 1"), "6125021", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125020", "1", "Test 2"), "6125020", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125019", "1", "Test 3"), "6125019", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125023", "1", "Test 4"), "6125023", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125022", "1", "Test 5"), "6125022", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.FourToTwelveMonths);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var totalScore = ((GainShortScreenerScore)(scoredEvent.Value)).SubstanceDisorder.PastYear;
                Assert.AreEqual(totalScore, 5);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 5);
            }
        }

        [TestMethod]
        public void CalculateScoreSubstanceDisorderScreenerAllOnePlusYearsReturns5()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125021", "1", "Test 1"), "6125021", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125020", "1", "Test 2"), "6125020", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125019", "1", "Test 3"), "6125019", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125023", "1", "Test 4"), "6125023", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125022", "1", "Test 5"), "6125022", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.OnePlusYears);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var totalScore = ((GainShortScreenerScore)(scoredEvent.Value)).SubstanceDisorder.Lifetime;
                Assert.AreEqual(totalScore, 5);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 5);
            }
        }

        [TestMethod]
        public void CalculateScoreSubstanceDisorderScreenerAllNeverReturns0()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125021", "1", "Test 1"), "6125021", "Test 1"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125020", "1", "Test 2"), "6125020", "Test 2"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125019", "1", "Test 3"), "6125019", "Test 3"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125023", "1", "Test 4"), "6125023", "Test 4"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);
                assessment.UpdateItem(
                    new ItemDefinition(
                        new CodedConcept(new CodeSystem("6125022", "1", "Test 5"), "6125022", "Test 5"),
                        ItemType.Question,
                        null),
                    LastTimeFrequency.Never);

                var resourceManager = new Mock<IResourcesManager>();
                var patientRepository = new Mock<IPatientRepository>();
                var gainShortScreenerScoringEngine = new GainShortScreenerScoringEngine(resourceManager.Object, patientRepository.Object);
                gainShortScreenerScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                var scoreObject = ((GainShortScreenerScore)(scoredEvent.Value));
                var totalAll = scoreObject.InternalDisorder.PastMonth +
                               scoreObject.InternalDisorder.Past90Days +
                               scoreObject.InternalDisorder.PastYear +
                               scoreObject.InternalDisorder.Lifetime;
                Assert.AreEqual(totalAll, 0);
                Assert.AreEqual(int.Parse(scoredEvent.Value.ToString()), 0);
            }
        }

        private void SetupServiceLocatorFixture(ServiceLocatorFixture serviceLocatorFixture)
        {
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<ICommitDomainEventService>().Singleton().Use<CommitDomainEventService>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IUnitOfWorkProvider>().Use<UnitOfWorkProvider>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IUnitOfWork>().Use(new Mock<IUnitOfWork>().Object));
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IPatientUniqueIdentifierGenerator>().Use(new Mock<IPatientUniqueIdentifierGenerator>().Object));
            Thread.CurrentPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(ProCenterClaimType.StaffKeyClaimType, Guid.NewGuid().ToString())
                }));
        }
    }
}
