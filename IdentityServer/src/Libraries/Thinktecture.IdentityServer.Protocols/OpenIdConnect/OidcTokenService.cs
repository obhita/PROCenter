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
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using Thinktecture.IdentityServer.Models;

namespace Thinktecture.IdentityServer.Protocols.OpenIdConnect
{
    class OidcTokenService
    {
        string _issuer;
        X509Certificate2 _signingCert;

        public OidcTokenService(string issuer, X509Certificate2 signingCertificate)
        {
            _issuer = issuer;
            _signingCert = signingCertificate;
        }

        public OidcTokenResponse CreateTokenResponse(StoredGrant grant, int accessTokenLifetime)
        {
            var accessToken = CreateAccessToken(grant.Subject, _issuer + "/userinfo", grant.ClientId, grant.Scopes, accessTokenLifetime);
            var response = new OidcTokenResponse
            {
                AccessToken = accessToken.ToJwtString(),
                TokenType = "Bearer",
                ExpiresIn = accessTokenLifetime * 60
            };

            if (grant.GrantType == StoredGrantType.AuthorizationCode)
            {
                var idToken = CreateIdentityToken(grant.Subject, grant.ClientId, 60);
                response.IdentityToken = idToken.ToJwtString();
            }

            return response;
        }

        public IdentityToken CreateIdentityToken(string subject, string audience, int ttl)
        {
            return new IdentityToken
            {
                Audience = audience,
                Subject = subject,

                Ttl = ttl,
                Issuer = _issuer,
                SigningCredential = new X509SigningCredentials(_signingCert)
            };
        }

        public AccessToken CreateAccessToken(string subject, string audience, string clientId, string scopes, int ttl)
        {
            var splitScopes = scopes.Split(' ');

            return new AccessToken
            {
                Audience = audience,
                Subject = subject,
                ClientId = clientId,
                Scopes = splitScopes,

                Ttl = ttl,
                Issuer = _issuer,
                SigningCredential = new X509SigningCredentials(_signingCert)
            };
        }
    }
}
