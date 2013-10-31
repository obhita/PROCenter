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
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Threading.Tasks;
    using Agatha.Common;
    using Common;
    using Infrastructure.Security;
    using Models;
    using Service.Message.Patient;
    using Service.Message.Security;

    public class PortalController : BaseController
    {
        private readonly ILogoutService _logoutService;
        //
        // GET: /Portal/

        public PortalController ( IRequestDispatcherFactory requestDispatcherFactory, ILogoutService logoutService )
            : base ( requestDispatcherFactory )
        {
            _logoutService = logoutService;
        }

        public async Task<ActionResult> Index()
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
                return ValidateLogin();
            }
            return RedirectToAction ( "Index", "Home" );
        }

        public ActionResult ValidateLogin ()
        {
            return View ("ValidateLogin");
        }

        [HttpPost]
        public async Task<ActionResult> ValidateLogin (string patientId, DateTime dateOfBirth)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add ( new ValidatePatientAccountRequest { SystemAccountKey = UserContext.Current.SystemAccountKey.Value, PatientIdentifier = patientId, DateOfBirth = dateOfBirth } );
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
            return View();
        }
    }
}
