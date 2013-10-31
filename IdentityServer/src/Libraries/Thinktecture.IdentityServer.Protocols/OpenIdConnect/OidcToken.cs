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
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Security.Claims;

namespace Thinktecture.IdentityServer.Protocols.OpenIdConnect
{
    public abstract class OidcToken
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Ttl { get; set; }
        public List<Claim> ExtraClaims { get; set; }
        public X509SigningCredentials SigningCredential { get; set; }

        protected virtual List<Claim> CreateClaims()
        {
            var claims = new List<Claim>();

            if (ExtraClaims != null)
            {
                claims.AddRange(ExtraClaims);
            }

            return claims;
        }

        public virtual JwtSecurityToken ToJwt()
        {
            if (string.IsNullOrWhiteSpace(Issuer))
            {
                throw new InvalidOperationException("Issuer is empty");
            }
            if (string.IsNullOrWhiteSpace(Audience))
            {
                throw new InvalidOperationException("Audience is empty");
            }
            if (SigningCredential == null)
            {
                throw new InvalidOperationException("Signing credential is empty");
            }
            if (Ttl == 0)
            {
                throw new InvalidOperationException("Ttl is 0");
            }

            var claims = CreateClaims();

            return new JwtSecurityToken(
                Issuer,
                Audience,
                claims,
                new Lifetime(DateTime.UtcNow, DateTime.UtcNow.AddMinutes(Ttl)),
                SigningCredential);

        }

        public virtual string ToJwtString()
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(ToJwt());
        }
    }
}