namespace ProCenter.Domain.CommonModule
{
    #region Using Statements

    using System;

    #endregion

    /// <summary>
    ///     Interface for event routing.
    /// </summary>
    public interface IRouteEvents
    {
        #region Public Methods and Operators

        void Dispatch ( object eventMessage );
        void Register<T> ( Action<T> handler );
        void Register ( IAggregateRoot aggregate );

        #endregion
    }
}