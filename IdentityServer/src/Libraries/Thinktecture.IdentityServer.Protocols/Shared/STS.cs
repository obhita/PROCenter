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
using System.IdentityModel;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Xml;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityServer.TokenService;

namespace Thinktecture.IdentityServer.Protocols
{
    public class STS
    {
        SecurityTokenService _sts;

        public STS()
            : this(TokenServiceConfiguration.Current.CreateSecurityTokenService())
        { }

        public STS(SecurityTokenService sts)
        {
            if (sts == null)
            {
                throw new ArgumentNullException("sts");
            }

            _sts = sts;
        }

        public bool TryIssueToken(EndpointReference appliesTo, ClaimsPrincipal principal, string tokenType, out SecurityToken token)
        {
            token = null;

            var rst = new RequestSecurityToken
            {
                RequestType = RequestTypes.Issue,
                AppliesTo = appliesTo,
                KeyType = KeyTypes.Bearer,
                TokenType = tokenType
            };

            try
            {
                var rstr = _sts.Issue(principal, rst);
                token = rstr.RequestedSecurityToken.SecurityToken;
                return true;
            }
            catch (Exception e)
            {
                Tracing.Error("Failed to issue token. An exception occurred. " + e);
                return false;
            }
        }

        public bool TryIssueToken(EndpointReference appliesTo, ClaimsPrincipal principal, string tokenType, out TokenResponse response)
        {
            SecurityToken token = null;
            response = new TokenResponse { TokenType = tokenType };

            var result = TryIssueToken(appliesTo, principal, tokenType, out token);
            if (result == false)
            {
                return false;
            }

            var ts = token.ValidTo.Subtract(DateTime.UtcNow);
            response.ExpiresIn = (int)ts.TotalSeconds;

            if (tokenType == TokenTypes.JsonWebToken || tokenType == TokenTypes.SimpleWebToken)
            {
                var handler = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers[tokenType];
                response.AccessToken = handler.WriteToken(token);
            }
            else
            {
                var handler = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers;
                var sb = new StringBuilder(128);
                handler.WriteToken(new XmlTextWriter(new StringWriter(sb)), token);

                response.AccessToken = sb.ToString();
            }

            return result;
        }
    }
}
