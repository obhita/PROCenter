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
namespace Thinktecture.IdentityServer.Web.Security
{
    #region

    using System.IdentityModel.Tokens;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using System.Web;
    using IdentityModel;
    using IdentityModel.Tokens.Http;
    using Repositories;

    #endregion

    public class IdentityServerHttpAuthentication : HttpAuthentication
    {
        private readonly IConfigurationRepository _configurationRepository;

        public IdentityServerHttpAuthentication(AuthenticationConfiguration configuration, IConfigurationRepository configurationRepository)
            : base(configuration)
        {
            _configurationRepository = configurationRepository;
        }

        public override ClaimsPrincipal AuthenticateSessionToken(HttpRequestMessage request)
        {
            // grab header
            var headerValues = request.Headers.SingleOrDefault(h => h.Key == Configuration.SessionToken.HeaderName).Value;
            if (headerValues != null)
            {
                var header = headerValues.SingleOrDefault();
                if (header != null)
                {
                    var parts = header.Split(' ');
                    if (parts.Length == 2)
                    {
                        // if configured scheme was sent, try to authenticate the session token
                        if (parts[0] == Configuration.SessionToken.Scheme)
                        {
                            var token = new JwtSecurityToken(parts[1]);

                            if (_configurationRepository.Keys.SigningCertificate.Thumbprint != null)
                            {
                                var store = new X509Store(StoreName.TrustedPeople, StoreLocation.LocalMachine);
                                store.Open(OpenFlags.ReadOnly);
                                var cert = store.Certificates.Find(X509FindType.FindByThumbprint, _configurationRepository.Keys.SigningCertificate.Thumbprint, false)[0];
                                store.Close();

                                var validationParameters = new TokenValidationParameters
                                    {
                                        ValidIssuer = Configuration.SessionToken.IssuerName,
                                        AllowedAudience =
                                            HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                                            HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/" + Configuration.SessionToken.Audience,
                                        SigningToken = new X509SecurityToken(cert),
                                    };

                                var handler = new JwtSecurityTokenHandler();
                                return handler.ValidateToken(token, validationParameters);
                            }
                        }
                    }
                }
            }

            return Principal.Anonymous;
        }
    }
}