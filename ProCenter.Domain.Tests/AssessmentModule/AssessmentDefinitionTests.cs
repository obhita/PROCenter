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