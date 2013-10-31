namespace ProCenter.Infrastructure.EventStore
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using Pillar.Domain.Event;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>
    ///     Interface for repository
    /// </summary>
    public interface IEventStoreRepository
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the by key.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        TAggregate GetByKey<TAggregate> ( Guid key ) where TAggregate : class, IAggregateRoot;

        /// <summary>
        ///     Gets the by key.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        TAggregate GetByKey<TAggregate> ( Guid key, int version ) where TAggregate : class, IAggregateRoot;

        /// <summary>
        /// Gets the last modified date.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="key">The aggregate key.</param>
        /// <returns>
        /// Last modified date.
        /// </returns>
        DateTime? GetLastModifiedDate<TAggregate>(Guid key);

        /// <summary>
        ///     Saves the specified aggregate.
        /// </summary>
        /// <param name="aggregate">The aggregate.</param>
        /// <param name="uncommitedEvents">The uncommited events.</param>
        /// <param name="commitId">The commit id.</param>
        /// <param name="updateHeaders">The update headers.</param>
        void Save ( IAggregateRoot aggregate, IEnumerable<IDomainEvent> uncommitedEvents, Guid commitId, Action<IDictionary<string, object>> updateHeaders );

        #endregion
    }
}