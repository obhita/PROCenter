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
namespace ProCenter.Infrastructure.Tests.Service.ReadSideService
{
    #region

    using System;
    using System.Collections.Generic;
    using Infrastructure.Service.ReadSideService;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Pillar.Common.InversionOfControl;
    using Primitive;
    using ProCenter.Domain.PatientModule;
    using ProCenter.Domain.PatientModule.Event;
    using global::EventStore;

    #endregion

    [TestClass]
    public class MessageUpdaterTests
    {
        [TestMethod]
        public void CommitEventIsDispacthed()
        {
            //arrange
            var readSidePersistenceDispatcher = new ReadSidePersistenceDispatcher(It.IsAny<IContainer>());
            var handleMessagesOfPatientCreatedEventMock = new Mock<IHandleMessages<PatientCreatedEvent>>();
            readSidePersistenceDispatcher.Register(handleMessagesOfPatientCreatedEventMock.Object);

            var patientCreatedEvent = new PatientCreatedEvent(Guid.NewGuid(), 1, Guid.NewGuid(), new PersonName("John", "Doe"), DateTime.Parse("1/1/2000"), Gender.Male, string.Empty);
            var eventMessage = new EventMessage {Body = patientCreatedEvent};

            var commitMock = new Mock<Commit>();
            commitMock.Setup(c => c.Events).Returns(new List<EventMessage> {eventMessage});

            //act
            readSidePersistenceDispatcher.Dispatch(commitMock.Object);
            
            //assert
            handleMessagesOfPatientCreatedEventMock.Verify(h => h.Handle(It.IsAny<PatientCreatedEvent>()), Times.Once());
        }
    }
}