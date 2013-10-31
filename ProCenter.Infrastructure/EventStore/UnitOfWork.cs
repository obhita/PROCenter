namespace ProCenter.Infrastructure.EventStore
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Pillar.Common.Utility;
    using Pillar.Domain.Event;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>
    ///     Implements the unit of work for a request.
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        #region Fields

        private readonly Guid _commitId = CombGuid.NewCombGuid ();
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly Dictionary<IAggregateRoot, List<IDomainEvent>> _uncommitedEvents = new Dictionary<IAggregateRoot, List<IDomainEvent>> ();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork" /> class.
        /// </summary>
        /// <param name="eventStoreRepository">The repository.</param>
        public UnitOfWork ( IEventStoreRepository eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Commits the unit of work.
        /// </summary>
        public void Commit ()
        {
            foreach ( var uncommitedEvents in _uncommitedEvents )
            {
                _eventStoreRepository.Save ( uncommitedEvents.Key, uncommitedEvents.Value, _commitId, a => { } );
            }
        }

        /// <summary>
        ///     Gets an aggregate for the specified key.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of <see cref="IAggregateRoot" />.
        /// </typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        ///     A <see cref="IAggregateRoot" />.
        /// </returns>
        public T Get<T> ( Guid key ) where T : class, IAggregateRoot
        {
            return _uncommitedEvents.Keys.FirstOrDefault ( a => a.Key == key ) as T;
        }

        /// <summary>
        ///     Registers the specified aggregate root.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root.</param>
        /// <param name="events">The events.</param>
        public void Register(IAggregateRoot aggregateRoot, params IDomainEvent[] events)
        {
            if ( _uncommitedEvents.ContainsKey ( aggregateRoot ) )
            {
                _uncommitedEvents[aggregateRoot].AddRange ( events );
            }
            else
            {
                _uncommitedEvents.Add ( aggregateRoot, new List<IDomainEvent> ( events ) );
            }
        }

        #endregion

        public void Dispose ()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if ( _eventStoreRepository is IDisposable )
            {
                (_eventStoreRepository as IDisposable).Dispose ();
            }
        }
    }
}