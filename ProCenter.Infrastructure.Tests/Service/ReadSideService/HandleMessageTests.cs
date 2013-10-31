namespace ProCenter.Infrastructure.Tests.Service.ReadSideService
{
    #region

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Infrastructure.Domain.Repositories;
    using Infrastructure.EventStore;
    using Infrastructure.Service.ReadSideService;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Pillar.Common.InversionOfControl;
    using Pillar.Common.Tests;
    using ProCenter.Domain.AssessmentModule.Event;
    using ProCenter.Domain.OrganizationModule;
    using ProCenter.Domain.OrganizationModule.Event;
    using ProCenter.Domain.PatientModule;
    using ProCenter.Domain.PatientModule.Event;
    using ProCenter.Domain.SecurityModule.Event;
    using global::EventStore.Dispatcher;

    #endregion

    [TestClass]
    public class HandleMessageTests
    {
        [TestMethod]
        public void IHandlerMessagesInheritsIHandlerMessage()
        {
            var commonInterface = typeof (IHandleMessages<>).GetInterfaces().FirstOrDefault(i => i == typeof (IHandleMessage));
            Assert.IsNotNull(commonInterface);
        }

        [TestMethod]
        public void CanRegisterAllEventsAndHandlers()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                //Arrange
                SetServiceLocatorFixture(serviceLocatorFixture);

                var readSidePersistenceDispatcher = new ReadSidePersistenceDispatcher(IoC.CurrentContainer);
                
                //Act
                readSidePersistenceDispatcher.Execute();
                //var eventUpdaters = readSidePersistenceDispatcher.GetEventUpdaters;

                ////Assert
                //Assert.IsNotNull(eventUpdaters.FirstOrDefault(e => e.Key == typeof(AssessmentDefinitionCreatedEvent)));
                //Assert.IsNotNull(eventUpdaters.FirstOrDefault(e => e.Key == typeof(AssessmentCreatedEvent)));
                //Assert.IsNotNull(eventUpdaters.FirstOrDefault(e => e.Key == typeof(ItemUpdatedEvent)));
                //Assert.IsNotNull(eventUpdaters.FirstOrDefault(e => e.Key == typeof(AssessmentSubmittedEvent)));
                //Assert.IsNotNull(eventUpdaters.FirstOrDefault(e => e.Key == typeof(PatientCreatedEvent)));
                //Assert.IsNotNull(eventUpdaters.FirstOrDefault(e => e.Key == typeof(PatientChangedEvent)));
                //Assert.IsNotNull(eventUpdaters.FirstOrDefault(e => e.Key == typeof(RoleCreatedEvent)));
                //Assert.IsNotNull(eventUpdaters.FirstOrDefault(e => e.Key == typeof(RoleNameRevisedEvent)));
                //Assert.IsNotNull(eventUpdaters.FirstOrDefault(e => e.Key == typeof(StaffCreatedEvent)));
                //Assert.IsNotNull(eventUpdaters.FirstOrDefault(e => e.Key == typeof(StaffChangedEvent)));
                //Assert.IsNotNull(eventUpdaters.FirstOrDefault(e => e.Key == typeof(TeamCreatedEvent)));
                //Assert.IsNotNull(eventUpdaters.FirstOrDefault(e => e.Key == typeof(StaffAddedToTeamEvent)));
                //Assert.IsNotNull(eventUpdaters.FirstOrDefault(e => e.Key == typeof(PatientAddedToTeamEvent)));
                //Assert.IsNotNull(eventUpdaters.FirstOrDefault(e => e.Key == typeof(StaffRemovedFromTeamEvent)));
                //Assert.IsNotNull(eventUpdaters.FirstOrDefault(e => e.Key == typeof(PatientRemovedFromTeamEvent)));
            }
        }


        private static void SetServiceLocatorFixture(ServiceLocatorFixture serviceLocatorFixture)
        {
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IUnitOfWork>().Use<UnitOfWork>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IUnitOfWorkProvider>().Use<UnitOfWorkProvider>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IEventStoreFactory>().Use<EventStoreFactory>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IEventStoreRepository>().Use<EventStoreRepository>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IDetectConflicts>().Use<EmptyConflictDetector>());

            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IDbConnectionFactory>().Use<FakeSqlConnectionFactory>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IStaffRepository>().Use<StaffRepository>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.For<IPatientRepository>().Use<PatientRepository>());
            serviceLocatorFixture.StructureMapContainer.Configure(c => c.Scan(scanner =>
                {
                    scanner.AssembliesFromApplicationBaseDirectory(p => (p.FullName == null) ? false : p.FullName.Contains("ProCenter."));
                    scanner.AddAllTypesOf<IDispatchCommits>();
                    scanner.WithDefaultConventions();
                    scanner.AddAllTypesOf<IHandleMessage>();
                }));
        }
    }

    public class FakeSqlConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection CreateConnection()
        {
            var sqlConnection = new SqlConnection();
            return sqlConnection;
        }
    }
}