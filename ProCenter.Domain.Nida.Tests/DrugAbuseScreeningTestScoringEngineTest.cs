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
namespace ProCenter.Domain.Nida.Tests
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
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

    #endregion

    [TestClass]
    public class DrugAbuseScreeningTestScoringEngineTest
    {
        [TestMethod]
        public void CalculateScoreShouldRaiseScoredEvent()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                // Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                var events = new List<IDomainEvent>();
                CommitEvent.RegisterAll(events.Add);

                // Exercise
                var patientGuid = CombGuid.NewCombGuid();
                var assessmentDefinition = Substitute.For<AssessmentDefinition> ();

                var assessment = new AssessmentInstanceFactory().Create(assessmentDefinition, patientGuid, "TestName");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("1", "1", "Test"), "1", "Test"), ItemType.Question, null), "true");
                assessment.UpdateItem(new ItemDefinition(new CodedConcept(new CodeSystem("1", "1", "Test"), "2", "Test"), ItemType.Question, null), "false");

                var drugAbuseScreeningTestScoringEngine = new DrugAbuseScreeningTestScoringEngine();
                drugAbuseScreeningTestScoringEngine.CalculateScore(assessment);

                // Verify
                Assert.AreEqual(4, events.Count);
                var scoredEvent = events[3];
                Assert.IsNotNull(scoredEvent);
                Assert.AreEqual(typeof(AssessmentScoredEvent), scoredEvent.GetType());
                Assert.AreEqual((scoredEvent as AssessmentScoredEvent).Value, 1);
                Assert.AreEqual((scoredEvent as AssessmentScoredEvent).Guidance.Code, "guidance_0_to_2");
                Assert.AreEqual(4, assessment.Version);
            }
        }

        #region Methods

        private static void SetupServiceLocatorFixture(ServiceLocatorFixture serviceLocatorFixture)
        {
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<ICommitDomainEventService>().Singleton().Use<CommitDomainEventService>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IUnitOfWorkProvider>().Use<UnitOfWorkProvider>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IUnitOfWork>().Use(new Mock<IUnitOfWork>().Object));
            Thread.CurrentPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(ProCenterClaimType.StaffKeyClaimType, Guid.NewGuid().ToString())
                }));
        }

        #endregion
    }
}