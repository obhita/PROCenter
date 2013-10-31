namespace ProCenter.Infrastructure.Domain
{
    #region Using Statements

    using System;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>
    ///     Interface for aggregate factory.
    /// </summary>
    public interface IAggregateFactory
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Builds an aggregate for the specified key.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="memento">The memento.</param>
        /// <returns>An aggregate.</returns>
        TAggregate Build<TAggregate>(Guid key, IMemento memento)
            where TAggregate : class, IAggregateRoot;

        #endregion
    }
}