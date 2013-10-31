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
using System.Web.Mvc;
using Thinktecture.IdentityServer.Protocols.OAuth2;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Protocols.OpenIdConnect.Endpoints
{
    public class OidcAuthorizeController : OidcAuthorizeControllerBase
    {
        public OidcAuthorizeController() : base()
        { }

        public OidcAuthorizeController(IOpenIdConnectClientsRepository clients, IStoredGrantRepository grants)
            : base(clients, grants)
        { }

        protected override ActionResult ShowConsent(ValidatedRequest validatedRequest)
        {
            Tracing.Information("OIDC Consent screen");

            return View("Consent", new OidcViewModel(validatedRequest));
        }

        [ActionName("Index")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleConsent(AuthorizeRequest request, string button, string[] selectedScopes)
        {
            Tracing.Start("OIDC consent response");

            ValidatedRequest validatedRequest;
            ActionResult failedResult;
            if (!TryValidateRequest(request, out validatedRequest, out failedResult))
            {
                Tracing.Error("Aborting OIDC consent response");
                return failedResult;
            }

            if (button == "allow")
            {
                var vm = new OidcViewModel(validatedRequest);
                vm.SetScopes(selectedScopes);
                return PerformGrant(vm.ValidatedRequest);
            }
            else
            {
                return DenyGrant(validatedRequest);
            }
        }
    }
}
