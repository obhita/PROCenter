namespace ProCenter.Infrastructure.Domain.Repositories
{
    #region Using Statements

    using System;
    using System.Linq;
    using EventStore;
    using ProCenter.Domain.SecurityModule;
    using ProCenter.Domain.SecurityModule.Event;

    #endregion

    public class SystemAccountRepository : RepositoryBase<SystemAccount>, ISystemAccountRepository
    {
        #region Fields

        private readonly IEventStoreFactory _eventStoreFactory;

        #endregion

        #region Constructors and Destructors

        public SystemAccountRepository ( IEventStoreRepository eventStoreRepository, IEventStoreFactory eventStoreFactory )
            : base ( eventStoreRepository )
        {
            _eventStoreFactory = eventStoreFactory;
        }

        #endregion

        #region Public Methods and Operators

        public SystemAccount GetByIdentifier ( string identifier )
        {
            var eventStore = _eventStoreFactory.Build<SystemAccount> ();
            var commits = eventStore.Advanced.GetFrom ( DateTime.MinValue );
            var systemAccountCreatedEvent = commits.SelectMany ( c => c.Events )
                                                   .FirstOrDefault ( e => e.Body is SystemAccountCreatedEvent && ( e.Body as SystemAccountCreatedEvent ).Identifier == identifier );
            return systemAccountCreatedEvent == null ? null : _eventStoreRepository.GetByKey<SystemAccount> ( ( systemAccountCreatedEvent.Body as SystemAccountCreatedEvent ).Key );
            //TODO: See if there is a way to do this without hitting the event store twice.
        }

        #endregion
    }
}