namespace ProCenter.Infrastructure.EventStore
{
    #region Using Statements

    using System.Collections.Generic;

    #endregion

    public class EmptyConflictDetector : IDetectConflicts
    {
        #region Public Methods and Operators

        public bool ConflictsWith ( IEnumerable<object> uncommittedEvents, IEnumerable<object> committedEvents )
        {
            return false;
        }

        public void Register<TUncommitted, TCommitted> ( ConflictDelegate handler ) where TUncommitted : class where TCommitted : class
        {
        }

        #endregion
    }
}