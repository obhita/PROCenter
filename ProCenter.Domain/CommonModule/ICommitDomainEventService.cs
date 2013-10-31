namespace ProCenter.Domain.CommonModule
{
    #region Using Statements

    using System;
    using Pillar.Domain.Event;

    #endregion

    /// <summary>
    ///     Interface for domain service that handles commiting of events.
    /// </summary>
    public interface ICommitDomainEventService : IDomainEventService
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Raises the commit event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="aggregateRoot">The aggregate root.</param>
        /// <param name="commitEvent">The commit event.</param>
        void RaiseCommit<TEvent> ( IAggregateRoot aggregateRoot, TEvent commitEvent ) where TEvent : ICommitEvent;

        /// <summary>
        /// Registers for all events.
        /// </summary>
        /// <param name="callback">The callback.</param>
        void RegisterAll(Action<IDomainEvent> callback);

        #endregion
    }
}