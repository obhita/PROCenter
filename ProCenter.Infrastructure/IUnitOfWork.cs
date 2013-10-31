namespace ProCenter.Infrastructure
{
    #region Using Statements

    using System;
    using Pillar.Domain.Event;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>
    ///     Interface that defines a unit of work.
    /// </summary>
    public interface IUnitOfWork
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Commits the unit of work.
        /// </summary>
        void Commit ();

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
        T Get<T> ( Guid key ) where T : class, IAggregateRoot;

        /// <summary>
        ///     Registers the specified aggregate root.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root.</param>
        /// <param name="events">The events.</param>
        void Register(IAggregateRoot aggregateRoot, params IDomainEvent[] events);

        #endregion
    }
}