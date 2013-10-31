namespace ProCenter.Infrastructure.EventStore.PipelineHook
{
    #region Using Statements

    using System;
    using global::EventStore;

    #endregion

    /// <summary>
    ///     Event store <see cref="IPipelineHook" /> to add audit headers to events.
    /// </summary>
    public class AuditPipelineHook : IPipelineHook
    {
        #region Static Fields

        public static readonly string TimestampHeader = "Audit-Timestamp";
        public static readonly string UserIdHeader = "Audit-UserId";
        public static readonly string UserNameHeader = "Audit-UserName";

        #endregion

        #region Fields

        private readonly IUserContextService _userContextService;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AuditPipelineHook" /> class.
        /// </summary>
        /// <param name="userContextService">The user context service.</param>
        public AuditPipelineHook ( IUserContextService userContextService )
        {
            _userContextService = userContextService;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose ()
        {
        }

        /// <summary>
        ///     Hooks into the commit pipeline just after the commit has been *successfully* committed to durable storage.
        /// </summary>
        /// <param name="committed">The commit which has been persisted.</param>
        public void PostCommit ( Commit committed )
        {
        }

        /// <summary>
        ///     Hooks into the commit pipeline prior to persisting the commit to durable storage.
        /// </summary>
        /// <param name="attempt">The attempt to be committed.</param>
        /// <returns>
        ///     If processing should continue, returns true; otherwise returns false.
        /// </returns>
        public bool PreCommit ( Commit attempt )
        {
            var time = DateTime.Now;
            foreach ( var eventMessage in attempt.Events )
            {
            //    eventMessage.Headers.Add ( UserNameHeader, _userContextService.UserName );
            //    eventMessage.Headers.Add ( UserIdHeader, _userContextService.UserId );
                eventMessage.Headers.Add(TimestampHeader, time);
            }
            attempt.Headers.Add(TimestampHeader, time);
            return true;
        }

        /// <summary>
        ///     Hooks into the selection pipeline just prior to the commit being returned to the caller.
        /// </summary>
        /// <param name="committed">The commit to be filtered.</param>
        /// <returns>
        ///     If successful, returns a populated commit; otherwise returns null.
        /// </returns>
        public Commit Select ( Commit committed )
        {
            return committed;
        }

        #endregion
    }
}