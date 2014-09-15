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

            Assert.AreEqual ( patient, null );
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