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
namespace ProCenter.Domain.Tests.AssessmentModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;

    using CommonModule;
    using Domain.AssessmentModule;
    using Domain.AssessmentModule.Event;
    using Infrastructure;
    using Infrastructure.EventStore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    using NSubstitute;

    using Pillar.Common.Metadata;
    using Pillar.Common.Tests;
    using Pillar.Common.Utility;
    using Pillar.Domain.Event;

    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule.Lookups;
    using ProCenter.Domain.AssessmentModule.Metadata;
    using ProCenter.Domain.AssessmentModule.Rules;

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
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName", false);

                // Verify
                Assert.AreEqual(1, events.Count);
                var createdEvent = events[0];
                Assert.IsNotNull(createdEvent);
                Assert.AreEqual(typeof (AssessmentCreatedEvent), createdEvent.GetType());
                Assert.AreEqual((createdEvent as AssessmentCreatedEvent).PatientKey, patientGuid);
                Assert.AreEqual((createdEvent as AssessmentCreatedEvent).AssessmentName, assessmentName);
                Assert.AreEqual(1, assessment.Version);
            }
        }

        [TestMethod]
        public void ShouldApplyItemUpdatedEvent()
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

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName", false);
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("1", "1", "Test"), "1", "Test"), ItemType.Question, null), 0);

                // Verify
                Assert.AreEqual(2, events.Count);
                var itemUpdatedEvent = events[1];
                Assert.IsNotNull(itemUpdatedEvent);
                Assert.AreEqual(typeof (ItemUpdatedEvent), itemUpdatedEvent.GetType());
                Assert.AreEqual((itemUpdatedEvent as ItemUpdatedEvent).Value, 0);
                Assert.AreEqual(2, assessment.Version);
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
                Guid patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName", false);
                assessment.Submit();

                // Verify
                Assert.AreEqual(2, events.Count);
                var submittedEvent = events[1];
                Assert.IsNotNull(submittedEvent);
                Assert.AreEqual(typeof (AssessmentSubmittedEvent), submittedEvent.GetType());
                Assert.AreEqual((submittedEvent as AssessmentSubmittedEvent).Submit, true);
                Assert.AreEqual(2, assessment.Version);
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
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName", false);
                assessment.ScoreComplete(new CodedConcept(CodeSystems.Obhita, "dummayCode", ""), "result");

                // Verify
                Assert.AreEqual(2, events.Count);
                var scoredEvent = events[1];
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(typeof (AssessmentScoredEvent), scoredEvent.GetType());
                Assert.AreEqual((scoredEvent as AssessmentScoredEvent).Value, "result");
                Assert.AreEqual((scoredEvent as AssessmentScoredEvent).ScoreCode.Code, "dummayCode");
                Assert.IsNull((scoredEvent as AssessmentScoredEvent).Guidance);
                Assert.AreEqual(2, assessment.Version);
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
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition>();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName", false);
                var workflowKey = CombGuid.NewCombGuid();
                assessment.AddToWorkflow(workflowKey);

                // Verify
                Assert.AreEqual(2, events.Count);
                var addedToWorkflowEvent = events[1];
                Assert.IsNotNull(addedToWorkflowEvent);
                Assert.AreEqual(typeof (AssessmentAddedToWorkflowEvent), addedToWorkflowEvent.GetType());
                Assert.AreEqual((addedToWorkflowEvent as AssessmentAddedToWorkflowEvent).WorkflowKey, workflowKey);
                Assert.AreEqual(2, assessment.Version);
            }
        }

        [TestMethod]
        public void CalculateCompleteness_NothingSkipped_CompletenessTotalCorrect()
        {
            using ( var serviceLocatorFixture = new ServiceLocatorFixture () )
            {
                // Setup
                SetupServiceLocatorFixture ( serviceLocatorFixture );

                var assessmentDefinition = Substitute.For<AssessmentDefinition> ();
                var itemDefinitions = GetItemDefinitions ();
                assessmentDefinition.GetAllItemDefinitionsOfType ( Arg.Any<ItemType> () ).Returns ( itemDefinitions );
                var assessmentInstance = new AssessmentInstance ( assessmentDefinition, Guid.NewGuid (), "Test", false );

                var completeness = assessmentInstance.CalculateCompleteness ();

                Assert.AreEqual ( itemDefinitions.Count ( i => i.GetIsRequired () ), completeness.Total );
            }
        }

        [TestMethod]
        public void CalculateCompleteness_SkippedItems_CompletenessTotalCorrect()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);
                var itemDefinitions = GetItemDefinitions();

                var ruleCollection = Substitute.For<IAssessmentRuleCollection> ();
                var rule = Substitute.For<IItemSkippingRule> ();
                rule.SkippedItemDefinitions.Returns ( itemDefinitions.Where ( i => i.CodedConcept.Code == "3" ) );
                ruleCollection.ItemSkippingRules.Returns(new List<IItemSkippingRule> { rule });
                serviceLocatorFixture.StructureMapContainer.Configure(
                    c => c.For<IAssessmentRuleCollection>().Use( ruleCollection ).Named ( "Test" ));


                var assessmentDefinition = Substitute.For<AssessmentDefinition>();
                assessmentDefinition.GetAllItemDefinitionsOfType(Arg.Any<ItemType>()).Returns(itemDefinitions);
                var assessmentInstance = new AssessmentInstance(assessmentDefinition, Guid.NewGuid(), "Test", false);

                var completeness = assessmentInstance.CalculateCompleteness();

                Assert.AreEqual(itemDefinitions.Count(i => i.GetIsRequired()) - 1, completeness.Total);
            }
        }

        private IEnumerable<ItemDefinition> GetItemDefinitions ()
        {
            var codeSystem = new CodeSystem ( "1", "1", "1" );
            return new List<ItemDefinition>
                   {
                       new ItemDefinition ( new CodedConcept ( codeSystem, "1", "Test"  ), ItemType.Question, null  )
                       {
                           ItemMetadata = new ItemMetadata
                                          {
                                              MetadataItems = new List<IMetadataItem> { new RequiredForCompletenessMetadataItem ( "Report" ) }
                                          }
                       },
                       new ItemDefinition ( new CodedConcept ( codeSystem, "2", "Test"  ), ItemType.Question, null  ),
                       new ItemDefinition ( new CodedConcept ( codeSystem, "3", "Test"  ), ItemType.Question, null  )
                       {
                           ItemMetadata = new ItemMetadata
                                          {
                                              MetadataItems = new List<IMetadataItem> { new RequiredForCompletenessMetadataItem ( "Report" ) }
                                          }
                       },
                       new ItemDefinition ( new CodedConcept ( codeSystem, "4", "Test"  ), ItemType.Question, null  )
                       {
                           ItemMetadata = new ItemMetadata
                                          {
                                              MetadataItems = new List<IMetadataItem> { new RequiredForCompletenessMetadataItem ( "Report" ) }
                                          }
                       },
                   };
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
                c => c.For<IAssessmentRuleCollectionFactory>().Use<AssessmentRuleCollectionFactory>());
            serviceLocatorFixture.StructureMapContainer.Configure(
                c => c.For<IUnitOfWork>().Use(new Mock<IUnitOfWork>().Object));
            Thread.CurrentPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(ProCenterClaimType.StaffKeyClaimType, Guid.NewGuid().ToString())
                }));
        }

        #endregion
    }
}