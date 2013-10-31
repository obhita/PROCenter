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
using System;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityServer;
using Thinktecture.IdentityServer.Protocols;
using Thinktecture.IdentityServer.Protocols.OAuth2;
using Thinktecture.IdentityServer.Protocols.OpenIdConnect;

namespace Thinktecture.IdentityServer.Protocols.OAuth2
{
    public static class Extensions
    {
        public static HttpResponseMessage CreateOAuthErrorResponse(this HttpRequestMessage request, string OAuthError)
        {
            Tracing.Information("Sending error response: " + OAuthError);

            return request.CreateErrorResponse(HttpStatusCode.BadRequest,
                string.Format("{{ \"{0}\": \"{1}\" }}", OAuth2Constants.Errors.Error, OAuthError));
        }

        public static HttpResponseMessage CreateTokenResponse(this HttpRequestMessage request, TokenResponse response)
        {
            Tracing.Information("Returning token response.");
            return request.CreateResponse<TokenResponse>(HttpStatusCode.OK, response);
        }

        public static HttpResponseMessage CreateTokenResponse(this HttpRequestMessage request, OidcTokenResponse response)
        {
            Tracing.Information("Returning token response.");
            return request.CreateResponse<OidcTokenResponse>(HttpStatusCode.OK, response);
        }
        
        public static ActionResult AuthorizeValidationError(this Controller controller, AuthorizeRequestValidationException exception)
        {
            var roException = exception as AuthorizeRequestResourceOwnerException;
            if (roException != null)
            {
                Tracing.Error(roException.Message);

                var result = new ViewResult
                {
                    ViewName = "ValidationError",
                };

                result.ViewBag.Message = roException.Message;

                return result;
            }

            var clientException = exception as AuthorizeRequestClientException;
            if (clientException != null)
            {
                Tracing.Error(clientException.Message);
                return new ClientErrorResult(clientException.RedirectUri, clientException.Error, clientException.ResponseType, clientException.State);
            }

            throw new ArgumentException("Invalid exception type");
        }
    }
}
