namespace ProCenter.Infrastructure.Tests.EventStore
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using Domain;
    using Infrastructure.Domain;
    using Infrastructure.EventStore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Pillar.Domain.Event;
    using Primitive;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.PatientModule;
    using ProCenter.Domain.PatientModule.Event;

    #endregion

    [TestClass]
    public class EventStoreRepositoryTests
    {
        private EventStoreRepository _eventStoreRepository;

        [TestMethod]
        public void GetByKeyReturnNullForUnexistingRecord()
        {
            var patientKey = Guid.NewGuid();
            var patient = _eventStoreRepository.GetByKey<Patient>(patientKey);

            Assert.AreEqual(patient.Key, patientKey);
            Assert.IsNull(patient.Name);
            Assert.IsNull(patient.DateOfBirth);
            Assert.IsNull(patient.Gender);
        }

        [TestMethod]
        public void CanSavePatient()
        {
            var newPatient = new Patient();
            var patientKey = Guid.NewGuid();
            typeof (Patient).GetProperty("Key").SetValue(newPatient, patientKey, null);

            const string firstname = "John";
            const string lastname = "Doe";
            var dateOfBirth = new DateTime(1980, 1, 1);
            var gender = Gender.Male;

            var uncommitedEvents = new List<IDomainEvent>
                {
                    new PatientCreatedEvent(patientKey, 0, Guid.NewGuid(), new PersonName(firstname, string.Empty, lastname), dateOfBirth, gender, string.Empty)
                };

            _eventStoreRepository.Save(newPatient, uncommitedEvents, Guid.NewGuid(), null);

            var patient = _eventStoreRepository.GetByKey<Patient>(patientKey);
            Assert.AreEqual(patient.Key, patientKey);
            Assert.AreEqual(patient.Name.FirstName, firstname);
            Assert.AreEqual(patient.Name.MiddleName, string.Empty);
            Assert.AreEqual(patient.Name.LastName, lastname);
            Assert.AreEqual(patient.DateOfBirth, dateOfBirth);
            Assert.AreEqual(patient.Gender, gender);
        }

        [TestMethod]
        public void CanUpdatePatient()
        {
            var newPatient = new Patient();
            var patientKey = Guid.NewGuid();
            typeof (Patient).GetProperty("Key").SetValue(newPatient, patientKey, null);

            const string firstname = "John";
            const string lastname = "Doe";
            var dateOfBirth = new DateTime(1980, 1, 1);
            var gender = Gender.Male;

            var uncommitedEvents = new List<IDomainEvent>
                {
                    new PatientCreatedEvent(patientKey, 0, Guid.NewGuid(), new PersonName("dummy", string.Empty, "dummy"), dateOfBirth, gender, string.Empty),
                    new PatientChangedEvent(patientKey, 0, p => p.Name, new PersonName(firstname, string.Empty, lastname)),
                };

            _eventStoreRepository.Save(newPatient, uncommitedEvents, Guid.NewGuid(), null);

            var patient = _eventStoreRepository.GetByKey<Patient>(patientKey);
            Assert.AreEqual(patient.Key, patientKey);
            Assert.AreEqual(patient.Name.FirstName, firstname);
            Assert.AreEqual(patient.Name.MiddleName, string.Empty);
            Assert.AreEqual(patient.Name.LastName, lastname);
            Assert.AreEqual(patient.DateOfBirth, dateOfBirth);
            Assert.AreEqual(patient.Gender, gender);
        }

        [TestInitialize]
        public void Setup()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var unitOfWorkProviderMock = new Mock<IUnitOfWorkProvider>();
            unitOfWorkProviderMock.Setup(u => u.GetCurrentUnitOfWork()).Returns(unitOfWorkMock.Object);

            _eventStoreRepository = new EventStoreRepository(new InMemoryEventStoreFactory(),
                                                   new AggregateFactory(),
                                                   new Mock<IDetectConflicts>().Object,
                                                   unitOfWorkProviderMock.Object);
        }
    }
}