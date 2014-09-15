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

namespace ProCenter.Service.Handler.Security
{
    #region Using Statements

    using Common;
    using Domain.SecurityModule;
    using Infrastructure.Service;
    using NLog;
    using Service.Message.Security;

    #endregion

    /// <summary>Lock/UnLock System account request handler.</summary>
    public class LockUnlockRequestHandler : ServiceRequestHandler<LockUnLockRequest, LockUnLockResponse>
    {
        #region Static Fields

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger ();

        #endregion

        #region Fields

        private readonly ISystemAccountIdentityServiceManager _systemAccountIdentityServiceManager;
        private readonly ISystemAccountRepository _systemAccountRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LockUnlockRequestHandler" /> class.
        /// </summary>
        /// <param name="systemAccountRepository">The system account repository.</param>
        /// <param name="systemAccountIdentityServiceManager">The system account identity service manager.</param>
        public LockUnlockRequestHandler ( ISystemAccountRepository systemAccountRepository,
            ISystemAccountIdentityServiceManager systemAccountIdentityServiceManager )
        {
            _systemAccountRepository = systemAccountRepository;
            _systemAccountIdentityServiceManager = systemAccountIdentityServiceManager;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( LockUnLockRequest request, LockUnLockResponse response )
        {
            var systemAccount = _systemAccountRepository.GetByKey ( request.SystemAccountKey );
            if ( systemAccount != null )
            {
                if ( request.Lock && !systemAccount.IsLocked )
                {
                    var result = _systemAccountIdentityServiceManager.Lock ( systemAccount.Identifier );
                    if ( result.Sucess )
                    {
                        systemAccount.Lock ();
                        response.ResponseCode = LockUnLockResponseCode.Success;
                        _logger.Debug ( "Locked account: {0}-{1}", systemAccount.Key, systemAccount.Identifier );
                    }
                    else
                    {
                        response.ResponseCode = LockUnLockResponseCode.Error;
                        _logger.Debug ( "Failed lock for {0}-{1}: {2}", systemAccount.Key, systemAccount.Identifier, result.ErrorMessage );
                    }
                }
                else if ( !request.Lock && systemAccount.IsLocked )
                {
                    var result = _systemAccountIdentityServiceManager.UnLock ( systemAccount.Identifier );
                    if ( result.Sucess )
                    {
                        systemAccount.UnLock ();
                        response.ResponseCode = LockUnLockResponseCode.Success;
                        _logger.Debug ( "UnLocked account: {0}-{1}", systemAccount.Key, systemAccount.Identifier );
                    }
                    else
                    {
                        response.ResponseCode = LockUnLockResponseCode.Error;
                        _logger.Debug ( "Failed unlock for {0}-{1}: {2}", systemAccount.Key, systemAccount.Identifier, result.ErrorMessage );
                    }
                }
            }
            else
            {
                response.ResponseCode = LockUnLockResponseCode.UnknownAccount;
                _logger.Debug ( "Failed {0} for system account key {1}, system account does not exist.", request.Lock ? "lock" : "unlock", request.SystemAccountKey );
            }
        }

        #endregion
    }
}