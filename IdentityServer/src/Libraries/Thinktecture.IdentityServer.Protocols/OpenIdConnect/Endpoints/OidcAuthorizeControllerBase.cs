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
using System;
using System.ComponentModel.Composition;
using System.Security.Claims;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Protocols.OAuth2;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Protocols.OpenIdConnect
{
    [Authorize]
    public abstract class OidcAuthorizeControllerBase : Controller
    {
        [Import]
        public IOpenIdConnectClientsRepository Clients { get; set; }

        [Import]
        public IStoredGrantRepository Grants { get; set; }

        protected abstract ActionResult ShowConsent(ValidatedRequest validatedRequest);
        
        public OidcAuthorizeControllerBase()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public OidcAuthorizeControllerBase(IOpenIdConnectClientsRepository clients, IStoredGrantRepository grants)
        {
            Clients = clients;
            Grants = grants;
        }

        protected bool TryValidateRequest(AuthorizeRequest request, 
            out ValidatedRequest validatedRequest, 
            out ActionResult failedResult)
        {
            validatedRequest = null;
            failedResult = null;

            try
            {
                var validator = new AuthorizeRequestValidator(Clients);
                validatedRequest = validator.Validate(request);
                return true;
            }
            catch (AuthorizeRequestValidationException ex)
            {
                failedResult = this.AuthorizeValidationError(ex);
                return false;
            }
        }

        public ActionResult Index(AuthorizeRequest request)
        {
            Tracing.Start("OIDC Authorize Endpoint");

            ValidatedRequest validatedRequest;
            ActionResult failedResult;
            if (!TryValidateRequest(request, out validatedRequest, out failedResult))
            {
                Tracing.Error("Aborting OAuth2 authorization request");
                return failedResult;
            }

            if (validatedRequest.Client.RequireConsent)
            {
                return ShowConsent(validatedRequest);
            }

            return PerformGrant(validatedRequest);
        }

        protected virtual ActionResult PerformGrant(ValidatedRequest validatedRequest)
        {
            // implicit grant
            if (validatedRequest.ResponseType.Equals(OAuth2Constants.ResponseTypes.Token, StringComparison.Ordinal))
            {
                return PerformImplicitGrant(validatedRequest);
            }

            // authorization code grant
            if (validatedRequest.ResponseType.Equals(OAuth2Constants.ResponseTypes.Code, StringComparison.Ordinal))
            {
                return PerformAuthorizationCodeGrant(validatedRequest);
            }

            return null;
        }

        protected virtual ActionResult DenyGrant(ValidatedRequest validatedRequest)
        {
            return new ClientErrorResult(new Uri(validatedRequest.RedirectUri), OAuth2Constants.Errors.AccessDenied, validatedRequest.ResponseType, validatedRequest.State);
        }

        protected virtual ActionResult PerformAuthorizationCodeGrant(ValidatedRequest validatedRequest)
        {
            Tracing.Information("Processing authorization code request");

            var grant = StoredGrant.CreateAuthorizationCode(
                validatedRequest.Client.ClientId,
                ClaimsPrincipal.Current.Identity.Name,
                validatedRequest.Scopes,
                validatedRequest.RedirectUri,
                60);

            Grants.Add(grant);

            var tokenString = string.Format("code={0}", grant.GrantId);

            if (!string.IsNullOrWhiteSpace(validatedRequest.State))
            {
                tokenString = string.Format("{0}&state={1}", tokenString, Server.UrlEncode(validatedRequest.State));
            }

            var redirectString = string.Format("{0}?{1}",
                        validatedRequest.RedirectUri,
                        tokenString);

            return Redirect(redirectString);
        }

        protected virtual ActionResult PerformImplicitGrant(ValidatedRequest validatedRequest)
        {
            throw new NotImplementedException();
        }
    }
}
