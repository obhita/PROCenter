namespace ProCenter.Infrastructure.Domain.Repositories
{
    #region Using Statements

    using System;
    using EventStore;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>
    /// Repository Base
    /// </summary>
    /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
    public abstract class RepositoryBase<TAggregate> : IRepository<TAggregate>
        where TAggregate : class, IAggregateRoot
    {
        #region Fields

        /// <summary>
        /// The _event store repository
        /// </summary>
        protected readonly IEventStoreRepository _eventStoreRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{TAggregate}" /> class.
        /// </summary>
        /// <param name="eventStoreRepository">The event store repository.</param>
        protected RepositoryBase ( IEventStoreRepository eventStoreRepository )
        {
            _eventStoreRepository = eventStoreRepository;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the aggregate by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public TAggregate GetByKey ( Guid key )
        {
            return _eventStoreRepository.GetByKey<TAggregate> ( key );
        }

        /// <summary>
        /// Gets the last modified date.
        /// </summary>
        /// <param name="key">The aggregate key.</param>
        /// <returns>Last modified date.</returns>
        public DateTime? GetLastModifiedDate ( Guid key )
        {
            return _eventStoreRepository.GetLastModifiedDate<TAggregate> ( key );
        }

        #endregion
    }
}