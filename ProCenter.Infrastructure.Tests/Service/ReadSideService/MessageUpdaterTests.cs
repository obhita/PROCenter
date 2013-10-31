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