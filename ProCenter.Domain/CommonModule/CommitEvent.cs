namespace ProCenter.Domain.CommonModule
{
    #region Using Statements

    using System;
    using Pillar.Common.InversionOfControl;
    using Pillar.Domain.Event;

    #endregion

    /// <summary>
    ///     Class for raising commit events.
    /// </summary>
    public static class CommitEvent
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Raises the commit event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="aggregateRoot">The aggregate root.</param>
        /// <param name="event">The event.</param>
        public static void RaiseCommitEvent<TEvent> ( IAggregateRoot aggregateRoot, TEvent @event ) where TEvent : ICommitEvent
        {
            if ( IoC.CurrentContainer == null )
                return;
            IoC.CurrentContainer.Resolve<ICommitDomainEventService> ().RaiseCommit<TEvent> ( aggregateRoot, @event );
        }

        /// <summary>
        /// Registers for all events.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public static void RegisterAll(Action<IDomainEvent> callback)
        {
            if (IoC.CurrentContainer == null)
                return;
            IoC.CurrentContainer.Resolve<ICommitDomainEventService>().RegisterAll(callback);
        }

        #endregion
    }
}