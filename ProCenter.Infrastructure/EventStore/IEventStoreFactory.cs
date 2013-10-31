namespace ProCenter.Infrastructure.EventStore
{
    #region Using Statements

    using System;
    using global::EventStore;

    #endregion

    /// <summary>
    ///     Interface for event store factory.
    /// </summary>
    public interface IEventStoreFactory : IDisposable
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Builds the event store fo aggregates of type
        ///     <typeparam name="TAggregate"></typeparam>
        ///     .
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <returns>
        ///     An <see cref="IStoreEvents" />.
        /// </returns>
        IStoreEvents Build<TAggregate> ();

        /// <summary>
        ///     Builds the event store for the specified aggregate type.
        /// </summary>
        /// <param name="aggregateType">Type of the aggregate.</param>
        /// <returns>
        ///     An <see cref="IStoreEvents" />.
        /// </returns>
        IStoreEvents Build ( Type aggregateType );

        #endregion
    }
}