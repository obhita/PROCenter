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

namespace ProCenter.Mvc.Controllers.Api
{
    #region Using Statements

    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Agatha.Common;
    using Common;

    using DevExpress.XtraReports.Web.Native;

    using Infrastructure.Extension;
    using Models;
    using Resources;
    using Service.Message.Security;

    #endregion

    /// <summary>Sytstem account controller.</summary>
    public class SystemAccountController : BaseApiController
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SystemAccountController" /> class.
        /// </summary>
        /// <param name="requestDispatcherFactory">The request dispatcher factory.</param>
        public SystemAccountController ( IRequestDispatcherFactory requestDispatcherFactory )
            : base ( requestDispatcherFactory )
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Changs the password.</summary>
        /// <param name="changePasswordViewModel">The change password view model.</param>
        /// <returns>A <see cref="String"/> array.</returns>
        [HttpPost]
        public async Task<string[]> ChangePassword ( ChangePasswordViewModel changePasswordViewModel )
        {
            var requestDisplacther = CreateAsyncRequestDispatcher ();
            requestDisplacther.Add ( new ChangePasswordRequest
            {
                SystemAccountKey = UserContext.Current.SystemAccountKey.Value,
                OldPassword = changePasswordViewModel.OldPassword,
                NewPassword = changePasswordViewModel.NewPassword,
            } );
            var response = await requestDisplacther.GetAsync<ChangePasswordResponse> ();

            var results = new[] {response.ResultCode.ToString (), UserControlResources.ResourceManager.GetTypedString ( response.ResultCode )};
            return results;
        }

        /// <summary>Locks the specified key.</summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="HttpResponseMessage"/>.</returns>
        [HttpPost]
        public async Task<HttpResponseMessage> Lock ( Guid key )
        {
            var requestDisplacther = CreateAsyncRequestDispatcher ();
            requestDisplacther.Add ( new LockUnLockRequest ()
            {
                SystemAccountKey = key,
                Lock = true
            } );
            var response = await requestDisplacther.GetAsync<LockUnLockResponse> ();
            if ( response.ResponseCode == LockUnLockResponseCode.Success )
            {
                var httpResonse = Request.CreateResponse ( HttpStatusCode.OK,
                    new
                    {
                        text = "UnLock",
                        location = Url.Route ( "DefaultApiPost", new {action = "UnLock", key} )
                    } );
                return httpResonse;
            }

            return Request.CreateErrorResponse ( HttpStatusCode.InternalServerError, UserControlResources.ResourceManager.GetTypedString ( response.ResponseCode ) ?? "Error" );
        }

        /// <summary>
        ///     Resets the password.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> ResetPassword(Guid key)
        {
            var requestDisplacther = CreateAsyncRequestDispatcher ();
            requestDisplacther.Add ( new ResetPasswordRequest ()
            {
                SystemAccountKey = key
            } );
            var response = await requestDisplacther.GetAsync<ResetPasswordResponse> ();
            if ( response.ResponseCode == ResetPasswordResponseCode.Success )
            {
                return new JsonResult
                {
                    Data = new
                    {
                        text = Api.PasswordReset,
                        error = string.Empty
                    }
                };
            }
            return new JsonResult
            {
                Data = new
                {
                    text = Api.PasswordNotReset,
                    error = string.Empty
                }
            };
        }

        /// <summary>
        ///     Unlocks the account.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="HttpResponseMessage"/>.</returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UnLock ( Guid key )
        {
            var requestDisplacther = CreateAsyncRequestDispatcher ();
            requestDisplacther.Add ( new LockUnLockRequest ()
            {
                SystemAccountKey = key,
                Lock = false
            } );
            var response = await requestDisplacther.GetAsync<LockUnLockResponse> ();
            if ( response.ResponseCode == LockUnLockResponseCode.Success )
            {
                var httpResonse = Request.CreateResponse ( HttpStatusCode.OK,
                    new
                    {
                        text = "Lock",
                        location = Url.Route ( "DefaultApiPost", new {action = "Lock", key} )
                    } );
                return httpResonse;
            }

            return Request.CreateErrorResponse ( HttpStatusCode.InternalServerError, UserControlResources.ResourceManager.GetTypedString ( response.ResponseCode ) ?? "Error" );
        }

        #endregion
    }
}