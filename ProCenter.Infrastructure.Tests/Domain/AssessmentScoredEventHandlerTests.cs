namespace ProCenter.Infrastructure.Tests.Domain
{
    #region Using Statements

    using System;
    using Infrastructure.EventStore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Pillar.Common.Tests;
    using Pillar.Common.Utility;
    using Pillar.Domain.Event;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Event;
    using ProCenter.Domain.CommonModule;

    #endregion

    [TestClass]
    public class AssessmentSubmittedEventHandlerTests
    {
        [TestMethod]
        public void AssessmentSubmittedEventIsRaisedWhenSubmit()
        {
            using (var serviceLocatorFixture = new ServiceLocatorFixture())
            {
                //Setup
                SetupServiceLocatorFixture(serviceLocatorFixture);

                bool eventRaised = false;
                DomainEvent.Register<AssessmentSubmittedEvent>(s => eventRaised = true);

                // Exercise
                Guid defGuid = CombGuid.NewCombGuid();
                Guid patientGuid = CombGuid.NewCombGuid();
                var source = new AssessmentInstance(defGuid, patientGuid, "testName");
                source.Submit();

                // Verify
                Assert.IsTrue(eventRaised);
            }
        }

        private static void SetupServiceLocatorFixture(ServiceLocatorFixture serviceLocatorFixture)
        {
            serviceLocatorFixture.StructureMapContainer.Configure(
                c => c.For<ICommitDomainEventService>().Singleton().Use<CommitDomainEventService>());
            serviceLocatorFixture.StructureMapContainer.Configure(
                c => c.For<IUnitOfWorkProvider>().Use<UnitOfWorkProvider>());
            serviceLocatorFixture.StructureMapContainer.Configure(
                c => c.For<IUnitOfWork>().Use(new Mock<IUnitOfWork>().Object));
            serviceLocatorFixture.StructureMapContainer.Configure(
                c => c.For<IDomainEventService>().Use(context => context.GetInstance<ICommitDomainEventService>()));
        }
    }
}