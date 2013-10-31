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
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.TokenService;

namespace Thinktecture.IdentityServer.Protocols.OpenIdConnect
{
    [Authorize]
    public class OidcUserInfoController : ApiController
    {
        [Import]
        public IClaimsRepository ClaimsRepository { get; set; }

        public OidcUserInfoController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public OidcUserInfoController(IClaimsRepository claimsRepository)
        {
            ClaimsRepository = claimsRepository;
        }

        public HttpResponseMessage Get()
        {
            Tracing.Start("OIDC UserInfo endpoint");

            var details = new RequestDetails { IsOpenIdRequest = true };
            var scopeClaims = ClaimsPrincipal.Current.FindAll(OAuth2Constants.Scope).ToList();
            var requestedClaims = ClaimsPrincipal.Current.FindAll("requestclaim").ToList();

            if (scopeClaims.Count > 0)
            {
                var scopes = new List<string>(scopeClaims.Select(sc => sc.Value));
                details.OpenIdScopes = scopes;
            }

            if (requestedClaims.Count > 0)
            {
                var requestClaims = new RequestClaimCollection();
                requestedClaims.ForEach(rc => requestClaims.Add(new RequestClaim(rc.Value)));
                
                details.ClaimsRequested = true;
                details.RequestClaims = requestClaims;
            }

            var principal = Principal.Create("OpenIdConnect",
                new Claim(ClaimTypes.Name, ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value));

            var claims = ClaimsRepository.GetClaims(principal, details);

            var dictionary = new Dictionary<string, string>();
            foreach (var claim in claims)
            {
                if (!dictionary.ContainsKey(claim.Type))
                {
                    dictionary.Add(claim.Type, claim.Value);
                }
                else
                {
                    var currentValue = dictionary[claim.Type];
                    dictionary[claim.Type] = currentValue += ("," + claim.Value);
                }
            }

            return Request.CreateResponse<Dictionary<string, string>>(HttpStatusCode.OK, dictionary, "application/json");
        }
    }
}