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

namespace ProCenter.Mvc.Controllers
{
    #region Using Statements

    using System;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Agatha.Common;
    using Common;
    using Infrastructure.Security;
    using Service.Message.Patient;
    using Service.Message.Security;

    #endregion

    /// <summary>The portal controller class.</summary>
    public class PortalController : BaseController
    {
        #region Fields

        private readonly ILogoutService _logoutService;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PortalController"/> class.
        /// </summary>
        /// <param name="requestDispatcherFactory">The request dispatcher factory.</param>
        /// <param name="logoutService">The logout service.</param>
        public PortalController ( IRequestDispatcherFactory requestDispatcherFactory, ILogoutService logoutService )
            : base ( requestDispatcherFactory )
        {
            _logoutService = logoutService;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        /// <exception cref="System.Web.HttpException">404;Patient record not found.</exception>
        public async Task<ActionResult> Index ()
        {
            if ( UserContext.Current.PatientKey.HasValue )
            {
                if ( UserContext.Current.Validated )
                {
                    var requestDispatcher = CreateAsyncRequestDispatcher ();
                    requestDispatcher.Add ( new GetPatientDtoByKeyRequest {PatientKey = UserContext.Current.PatientKey.Value} );
                    var response = await requestDispatcher.GetAsync<GetPatientDtoResponse> ();

                    if ( response.DataTransferObject == null )
                    {
                        throw new HttpException ( 404, "Patient record not found." );
                    }

                    return View ( response.DataTransferObject );
                }
                return ValidateLogin ();
            }
            return RedirectToAction ( "Index", "Home" );
        }

        /// <summary>Validates the login.</summary>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        public ActionResult ValidateLogin ()
        {
            return View ( "ValidateLogin" );
        }

        /// <summary>
        /// Validates the login.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> ValidateLogin ( string patientId, DateTime dateOfBirth )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new ValidatePatientAccountRequest
            {
                SystemAccountKey = UserContext.Current.SystemAccountKey.Value,
                PatientIdentifier = patientId,
                DateOfBirth = dateOfBirth
            } );
            var response = await requestDispatcher.GetAsync<ValidatePatientAccountResponse> ();
            if ( response.IsLocked )
            {
                var signoutMessage = _logoutService.Logout ();
                return Redirect ( signoutMessage.WriteQueryString () );
            }
            if ( response.Validated )
            {
                return RedirectToAction ( "Index" );
            }
            ModelState.AddModelError ( "validation-error", "Invalid information." );
            return View ();
        }

        #endregion
    }
}