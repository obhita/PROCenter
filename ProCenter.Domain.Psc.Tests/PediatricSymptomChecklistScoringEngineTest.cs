#region License Header
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
namespace ProCenter.Domain.Psc.Tests
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;

    using AssessmentModule.Event;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NSubstitute;

    using Pillar.Common.Tests;
    using AssessmentModule;
    using CommonModule;
    using Infrastructure;
    using Infrastructure.EventStore;
    using Moq;
    using Pillar.Common.Utility;
    using Pillar.Domain.Event;

    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule.Lookups;
    using ProCenter.Domain.PatientModule;
    using ProCenter.Domain.Psc;
    using ProCenter.Primitive;

    #endregion

    [TestClass]
    public class PediatricSymptomChecklistScoringEngineTest
    {
        [TestMethod]
        public void CalculateScoreDeductFromTotalWhenOneSelectedQuestionsAreAnsweredReturns1()
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
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250007", "1", "Has Trouble with teacher"), "71250007", "Has Trouble with teacher"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250006", "1", "Fidegty"), "71250006", "Fidegty"), ItemType.Question, null), TimeFrequency.Sometimes);

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(GetPatient(new DateTime(2009, 5, 2)));
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 1);
            }
        }

        [TestMethod]
        public void CalculateScoreDeductFromTotalWhenAllSelectedQuestionsAreAnsweredReturns0()
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
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250007", "1", "Has Trouble with teacher"), "71250007", "Has Trouble with teacher"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250008", "1", "Less interest in school"), "71250008", "Less interest in school"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250019", "1", "Absent from school"), "71250019", "Absent from school"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250020", "1", "School grades dropping"), "71250020", "School grades dropping"), ItemType.Question, null), TimeFrequency.Often);

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(GetPatient(new DateTime(2009, 5, 2)));
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 0);
            }
        }

        [TestMethod]
        public void CalculateScoreShouldWhenNeverSelectedReturnsZero()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("1", "1", "Test"), "3269981", "Test"), ItemType.Question, null), TimeFrequency.Never);

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();
                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(new Patient());
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 0);
            }
        }

        [TestMethod]
        public void CalculateScoreWhenSometimesSelectedReturnsOne()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("1", "1", "Test"), "3269981", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);

                var patietRepository = new Mock<IPatientRepository> ();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup ( a => a.GetByKey ( It.IsAny<Guid> () ) ).Returns ( new Patient () );
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 1);
            }
        }

        [TestMethod]
        public void CalculateScoreWhenOftenSelectedReturnsTwo()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("1", "1", "Test"), "3269981", "Test"), ItemType.Question, null), TimeFrequency.Often);

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(new Patient());
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 2);
            }
        }

        [TestMethod]
        public void CalculateScoreWhenNonTimeFrequencySelectedReturnsZero()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("1", "1", "Test"), "3269981", "Test"), ItemType.Question, null), "Some other value");

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(new Patient());
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 0);
            }
        }

        [TestMethod]
        public void CalculateScoreWhenNoItemsReturnsZero()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(new Patient());
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 0);
            }
        }

        [TestMethod]
        public void GetAttentionProblemSubscaleTotalWhenAllNeverSelectedReturnsZero ()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250006", "1", "Test"), "71250006", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250009", "1", "Test"), "71250009", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250010", "1", "Test"), "71250010", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250011", "1", "Test"), "71250011", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250016", "1", "Test"), "71250016", "Test"), ItemType.Question, null), TimeFrequency.Never);

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(new Patient());
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 0);
            }
        }

        [TestMethod]
        public void GetAttentionProblemSubscaleTotalWhenAllSometimesSelectedReturns5()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250006", "1", "Test"), "71250006", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250009", "1", "Test"), "71250009", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250010", "1", "Test"), "71250010", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250011", "1", "Test"), "71250011", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250016", "1", "Test"), "71250016", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(new Patient());
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 5);
            }
        }

        [TestMethod]
        public void GetAttentionProblemSubscaleTotalWhenAllOftenSelectedReturns10()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250006", "1", "Test"), "71250006", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250009", "1", "Test"), "71250009", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250010", "1", "Test"), "71250010", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250011", "1", "Test"), "71250011", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250016", "1", "Test"), "71250016", "Test"), ItemType.Question, null), TimeFrequency.Often);

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(new Patient());
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 10);
            }
        }

        [TestMethod]
        public void GetAnxietyDepressionSubscaleTotalWhenAllNeverSelectedReturnsZero()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250013", "1", "Test"), "71250013", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250015", "1", "Test"), "71250015", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250021", "1", "Test"), "71250021", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250024", "1", "Test"), "71250024", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250029", "1", "Test"), "71250029", "Test"), ItemType.Question, null), TimeFrequency.Never);

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(new Patient());
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 0);
            }
        }

        [TestMethod]
        public void GetAnxietyDepressionSubscaleTotalWhenAllSometimesSelectedReturns5()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250013", "1", "Test"), "71250013", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250015", "1", "Test"), "71250015", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250021", "1", "Test"), "71250021", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250024", "1", "Test"), "71250024", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250029", "1", "Test"), "71250029", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(new Patient());
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 5);
            }
        }

        [TestMethod]
        public void GetAnxietyDepressionSubscaleTotalWhenAllOftenSelectedReturns10()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250013", "1", "Test"), "71250013", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250015", "1", "Test"), "71250015", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250021", "1", "Test"), "71250021", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250024", "1", "Test"), "71250024", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250029", "1", "Test"), "71250029", "Test"), ItemType.Question, null), TimeFrequency.Often);

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(new Patient());
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 10);
            }
        }

        [TestMethod]
        public void GetConductProblemSubscaleTotalWhenAllNeverSelectedReturnsZero()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250018", "1", "Test"), "71250018", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250031", "1", "Test"), "71250031", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250033", "1", "Test"), "71250033", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250034", "1", "Test"), "71250034", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250035", "1", "Test"), "71250035", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250036", "1", "Test"), "71250036", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250037", "1", "Test"), "71250037", "Test"), ItemType.Question, null), TimeFrequency.Never);

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(new Patient());
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 0);
            }
        }

        [TestMethod]
        public void GetConductProblemSubscaleTotalWhenAllSometimesSelectedReturns7()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250018", "1", "Test"), "71250018", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250031", "1", "Test"), "71250031", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250033", "1", "Test"), "71250033", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250034", "1", "Test"), "71250034", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250035", "1", "Test"), "71250035", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250036", "1", "Test"), "71250036", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250037", "1", "Test"), "71250037", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(new Patient());
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 7);
            }
        }

        [TestMethod]
        public void GetConductProblemSubscaleTotalWhenAllOftenSelectedReturns14()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250018", "1", "Test"), "71250018", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250031", "1", "Test"), "71250031", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250033", "1", "Test"), "71250033", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250034", "1", "Test"), "71250034", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250035", "1", "Test"), "71250035", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250036", "1", "Test"), "71250036", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250037", "1", "Test"), "71250037", "Test"), ItemType.Question, null), TimeFrequency.Often);

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(new Patient());
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 14);
            }
        }

        [TestMethod]
        public void CalculateScoreGetOtherIssuesTotalWhenAllNeverSelectedReturnsZero()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250003", "1", "Test"), "71250003", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250004", "1", "Test"), "71250004", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250005", "1", "Test"), "71250005", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250007", "1", "Test"), "71250007", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250008", "1", "Test"), "71250008", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250012", "1", "Test"), "71250012", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250014", "1", "Test"), "71250014", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250017", "1", "Test"), "71250017", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250019", "1", "Test"), "71250019", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250020", "1", "Test"), "71250020", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250022", "1", "Test"), "71250022", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250023", "1", "Test"), "71250023", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250025", "1", "Test"), "71250025", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250026", "1", "Test"), "71250026", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250027", "1", "Test"), "71250027", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250028", "1", "Test"), "71250028", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250030", "1", "Test"), "71250030", "Test"), ItemType.Question, null), TimeFrequency.Never);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250032", "1", "Test"), "71250032", "Test"), ItemType.Question, null), TimeFrequency.Never);

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(new Patient());
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 0);
            }
        }

        [TestMethod]
        public void CalculateScoreGetOtherIssuesTotalWhenAllSometimesSelectedReturns18()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250003", "1", "Test"), "71250003", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250004", "1", "Test"), "71250004", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250005", "1", "Test"), "71250005", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250007", "1", "Test"), "71250007", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250008", "1", "Test"), "71250008", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250012", "1", "Test"), "71250012", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250014", "1", "Test"), "71250014", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250017", "1", "Test"), "71250017", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250019", "1", "Test"), "71250019", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250020", "1", "Test"), "71250020", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250022", "1", "Test"), "71250022", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250023", "1", "Test"), "71250023", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250025", "1", "Test"), "71250025", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250026", "1", "Test"), "71250026", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250027", "1", "Test"), "71250027", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250028", "1", "Test"), "71250028", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250030", "1", "Test"), "71250030", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250032", "1", "Test"), "71250032", "Test"), ItemType.Question, null), TimeFrequency.Sometimes);

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(new Patient());
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 18);
            }
        }

        [TestMethod]
        public void CalculateScoreGetOtherIssuesTotalWhenAllOftenSelectedReturns36()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250003", "1", "Test"), "71250003", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250004", "1", "Test"), "71250004", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250005", "1", "Test"), "71250005", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250007", "1", "Test"), "71250007", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250008", "1", "Test"), "71250008", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250012", "1", "Test"), "71250012", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250014", "1", "Test"), "71250014", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250017", "1", "Test"), "71250017", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250019", "1", "Test"), "71250019", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250020", "1", "Test"), "71250020", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250022", "1", "Test"), "71250022", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250023", "1", "Test"), "71250023", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250025", "1", "Test"), "71250025", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250026", "1", "Test"), "71250026", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250027", "1", "Test"), "71250027", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250028", "1", "Test"), "71250028", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250030", "1", "Test"), "71250030", "Test"), ItemType.Question, null), TimeFrequency.Often);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("71250032", "1", "Test"), "71250032", "Test"), ItemType.Question, null), TimeFrequency.Often);

                var patietRepository = new Mock<IPatientRepository>();
                var resourceManager = new Mock<IResourcesManager>();

                patietRepository.Setup(a => a.GetByKey(It.IsAny<Guid>())).Returns(new Patient());
                var pediatricSymptomCheclistScoringEngine = new PediatricSymptonChecklistScoringEngine(resourceManager.Object, patietRepository.Object);
                pediatricSymptomCheclistScoringEngine.CalculateScore(assessment);

                // Verify
                var scoredEvent = events.FirstOrDefault(e => e.GetType() == typeof(AssessmentScoredEvent)) as AssessmentScoredEvent;
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(scoredEvent.Value, 36);
            }
        }

        #region Methods

        private Patient GetPatient (DateTime dob)
        {
            var patientFactory = new PatientFactory();
            var pn = new PersonName("First Test", "", "Last Text");
            var patient = patientFactory.Create(CombGuid.NewCombGuid(), pn, dob, Gender.Male);
            return patient;
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

        #endregion
    }
}