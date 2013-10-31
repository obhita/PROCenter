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
using System.ComponentModel.Composition;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Protocols.SimpleHTTP
{
    [ClaimsAuthorize(Constants.Actions.Issue, Constants.Resources.SimpleHttp)]
    public class SimpleHttpController : ApiController
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public SimpleHttpController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public SimpleHttpController(IConfigurationRepository configurationRepository)
        {
            ConfigurationRepository = configurationRepository;
        }

        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            Tracing.Information("Simple HTTP endpoint called.");

            var query = request.GetQueryNameValuePairs();
            var auth = new AuthenticationHelper();

            var realm = query.FirstOrDefault(p => p.Key.Equals("realm", System.StringComparison.OrdinalIgnoreCase)).Value;
            var tokenType = query.FirstOrDefault(p => p.Key.Equals("tokenType", System.StringComparison.OrdinalIgnoreCase)).Value;

            if (string.IsNullOrWhiteSpace(realm))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, "realm parameter is missing.");
            }

            EndpointReference appliesTo;
            try
            {
                appliesTo = new EndpointReference(realm);
                Tracing.Information("Simple HTTP endpoint called for realm: " + realm);
            }
            catch
            {
                Tracing.Error("Malformed realm: " + realm);
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, "malformed realm name.");
            }

            if (string.IsNullOrWhiteSpace(tokenType))
            {
                tokenType = ConfigurationRepository.Global.DefaultHttpTokenType;
            }
            else
            {
                if (tokenType.Equals("jwt"))
                {
                    tokenType = TokenTypes.JsonWebToken;
                }
                else if (tokenType.Equals("swt"))
                {
                    tokenType = TokenTypes.SimpleWebToken;
                }
                else if (tokenType.Equals("saml11"))
                {
                    tokenType = TokenTypes.Saml11TokenProfile11;
                }
                else if (tokenType.Equals("saml2"))
                {
                    tokenType = TokenTypes.Saml2TokenProfile11;
                }
            }

            Tracing.Verbose("Token type: " + tokenType);

            TokenResponse tokenResponse;
            var sts = new STS();
            if (sts.TryIssueToken(appliesTo, ClaimsPrincipal.Current, tokenType, out tokenResponse))
            {
                var resp = request.CreateResponse<TokenResponse>(HttpStatusCode.OK, tokenResponse);
                return resp;
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid request.");
            }
        }
    }
}
