namespace ProCenter.Infrastructure.EventStore
{
    #region Using Statements

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     Delegate for detecting event store conflicts.
    /// </summary>
    /// <param name="uncommitted">The uncommitted.</param>
    /// <param name="committed">The committed.</param>
    /// <returns>Whether there is a conflict.</returns>
    public delegate bool ConflictDelegate ( object uncommitted, object committed );

    /// <summary>
    ///     Interface for detecting event store conflicts.
    /// </summary>
    public interface IDetectConflicts
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Conflictses the with.
        /// </summary>
        /// <param name="uncommittedEvents">The uncommitted events.</param>
        /// <param name="committedEvents">The committed events.</param>
        /// <returns></returns>
        bool ConflictsWith ( IEnumerable<object> uncommittedEvents, IEnumerable<object> committedEvents );

        /// <summary>
        ///     Registers the specified handler.
        /// </summary>
        /// <typeparam name="TUncommitted">The type of the uncommitted.</typeparam>
        /// <typeparam name="TCommitted">The type of the committed.</typeparam>
        /// <param name="handler">The handler.</param>
        void Register<TUncommitted, TCommitted> ( ConflictDelegate handler )
            where TUncommitted : class
            where TCommitted : class;

        #endregion
    }
}