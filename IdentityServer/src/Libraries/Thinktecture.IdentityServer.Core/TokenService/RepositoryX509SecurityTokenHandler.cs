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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.TokenService
{
    public class RepositoryX509SecurityTokenHandler : X509SecurityTokenHandler
    {
        [Import]
        public IUserRepository UserRepository { get; set; }

        public override ReadOnlyCollection<ClaimsIdentity> ValidateToken(SecurityToken token)
        {
            this.Configuration.IssuerNameRegistry = new ClientCertificateIssuerNameRegistry();

            Tracing.Information("Beginning client certificate token validation and authentication for SOAP");
            Container.Current.SatisfyImportsOnce(this);
            
            // call base class implementation for validation and claims generation 
            var identity = base.ValidateToken(token).First();

            // retrieve thumbprint
            var clientCert = ((X509SecurityToken)token).Certificate;
            Tracing.Information(String.Format("Client certificate thumbprint: {0}", clientCert.Thumbprint));

            // check if mapped user exists
            string userName;
            if (!UserRepository.ValidateUser(clientCert, out userName))
            {
                var message = String.Format("No mapped user exists for thumbprint {0}", clientCert.Thumbprint);
                Tracing.Error(message);
                throw new SecurityTokenValidationException(message);
            }

            Tracing.Information(String.Format("Mapped user found: {0}", userName));

            // retrieve issuer name
            var issuer = identity.Claims.First().Issuer;
            Tracing.Information(String.Format("Certificate issuer: {0}", issuer));

            // create new ClaimsIdentity for the STS issuance logic
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.AuthenticationMethod, AuthenticationMethods.X509),
                new Claim(ClaimTypes.AuthenticationInstant, identity.FindFirst(ClaimTypes.AuthenticationInstant).Value)
            };

            var id = new ClaimsIdentity(claims, "Client Certificate");
            return new List<ClaimsIdentity> { id }.AsReadOnly();
        }
    }
}