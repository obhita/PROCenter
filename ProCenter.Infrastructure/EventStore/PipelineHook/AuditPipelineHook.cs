#region License Header
// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/
#endregion
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