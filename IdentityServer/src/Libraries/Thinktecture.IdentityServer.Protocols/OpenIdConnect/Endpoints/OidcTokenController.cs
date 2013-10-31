#region Licence Header
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
using System.ComponentModel.Composition;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Protocols.OAuth2;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Protocols.OpenIdConnect
{
    public class OidcTokenController : ApiController
    {
        [Import]
        public IStoredGrantRepository Grants { get; set; }

        [Import]
        public IOpenIdConnectClientsRepository Clients { get; set; }

        [Import]
        public IConfigurationRepository ServerConfiguration { get; set; }

        public OidcTokenController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public HttpResponseMessage Post(TokenRequest request)
        {
            Tracing.Start("OIDC Token Endpoint");

            ValidatedRequest validatedRequest;

            try
            {
                var validator = new TokenRequestValidator(Clients, Grants);
                validatedRequest = validator.Validate(request, ClaimsPrincipal.Current);
            }
            catch (TokenRequestValidationException ex)
            {
                Tracing.Error("Aborting OIDC token request");
                return Request.CreateOAuthErrorResponse(ex.OAuthError);
            }

            // switch over the grant type
            if (validatedRequest.GrantType.Equals(OAuth2Constants.GrantTypes.AuthorizationCode))
            {
                return ProcessAuthorizationCodeRequest(validatedRequest);
            }
            else if (string.Equals(validatedRequest.GrantType, OAuth2Constants.GrantTypes.RefreshToken))
            {
                return ProcessRefreshTokenRequest(validatedRequest);
            }

            Tracing.Error("unsupported grant type: " + request.Grant_Type);
            return Request.CreateOAuthErrorResponse(OAuth2Constants.Errors.UnsupportedGrantType);
        }

        private HttpResponseMessage ProcessRefreshTokenRequest(ValidatedRequest validatedRequest)
        {
            Tracing.Information("Processing refresh token request");

            var tokenService = new OidcTokenService(
                ServerConfiguration.Global.IssuerUri, 
                ServerConfiguration.Keys.SigningCertificate);
            
            var response = tokenService.CreateTokenResponse(validatedRequest.Grant, validatedRequest.Client.AccessTokenLifetime);

            response.RefreshToken = validatedRequest.Grant.GrantId;
            return Request.CreateTokenResponse(response);
        }

        private HttpResponseMessage ProcessAuthorizationCodeRequest(ValidatedRequest validatedRequest)
        {
            Tracing.Information("Processing authorization code request");

            var tokenService = new OidcTokenService(
                ServerConfiguration.Global.IssuerUri, 
                ServerConfiguration.Keys.SigningCertificate);

            var response = tokenService.CreateTokenResponse(validatedRequest.Grant, validatedRequest.Client.AccessTokenLifetime);
            Grants.Delete(validatedRequest.Grant.GrantId);

            if (validatedRequest.Grant.Scopes.Contains(OidcConstants.Scopes.OfflineAccess) &&
                validatedRequest.Client.AllowRefreshToken)
            {
                var refreshToken = StoredGrant.CreateRefreshToken(
                    validatedRequest.Grant.ClientId,
                    validatedRequest.Grant.Subject,
                    validatedRequest.Grant.Scopes,
                    validatedRequest.Client.RefreshTokenLifetime);

                Grants.Add(refreshToken);
                response.RefreshToken = refreshToken.GrantId;
            }

            return Request.CreateTokenResponse(response);
        }
    }
}
