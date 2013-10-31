namespace ProCenter.Domain.CommonModule
{
    #region Using Statements

    using System;
    using Pillar.Domain;

    #endregion

    /// <summary>
    ///     Aggregate root marker interface.
    /// </summary>
    public interface IAggregateRoot : IAggregateRoot<Guid>
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Applies the event.
        /// </summary>
        /// <param name="event">The event.</param>
        void ApplyEvent ( object @event );

        /// <summary>
        ///     Gets the snapshot.
        /// </summary>
        /// <returns></returns>
        IMemento GetSnapshot ();

        /// <summary>
        /// Restores the snapshot.
        /// </summary>
        /// <param name="memento">The memento.</param>
        void RestoreSnapshot ( IMemento memento );

        #endregion
    }
}