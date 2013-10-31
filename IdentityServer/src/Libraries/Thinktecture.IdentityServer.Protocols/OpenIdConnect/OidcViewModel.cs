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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.Protocols.OpenIdConnect
{
    public class OidcViewModel
    {
        static string[] SupportedScopes =
            {
                OidcConstants.Scopes.Profile,
                OidcConstants.Scopes.Phone,
                OidcConstants.Scopes.Address,
                OidcConstants.Scopes.Email,
            };

        public ValidatedRequest ValidatedRequest { get; set; }
        public OidcViewModel(ValidatedRequest validatedRequest)
        {
            if (validatedRequest == null) throw new ArgumentNullException("validatedRequest");
            this.ValidatedRequest = validatedRequest;
            ValidateScopes(GetRawScopes());
        }

        void ValidateScopes(IEnumerable<string> scopes)
        {
            scopes = scopes ?? Enumerable.Empty<string>();

            var reminder = scopes
                .Except(SupportedScopes)
                .Except(new string[] { 
                    OidcConstants.Scopes.OpenId, 
                    OidcConstants.Scopes.OfflineAccess });
            
            if (reminder.Any())
            {
                throw new Exception("Unsupported Scopes Requested");
            }
        }

        public bool OfflineScope
        {
            get
            {
                return this.ValidatedRequest.Scopes.Contains(OidcConstants.Scopes.OfflineAccess);
            }
        }

        IEnumerable<string> GetRawScopes()
        {
            return this.ValidatedRequest.Scopes.Split(
                new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
        }
        void SetRawScopes(IEnumerable<string> scopes)
        {
            this.ValidatedRequest.Scopes = scopes.Aggregate((x, y) => x + " " + y);
        }

        public IEnumerable<string> GetDisplayScopes()
        {
            var vals =
                this.GetRawScopes()
                .Except(new string[] { 
                    OidcConstants.Scopes.OpenId, 
                    OidcConstants.Scopes.OfflineAccess })
                .Intersect(SupportedScopes);
            return vals;
        }
        
        public IEnumerable<string> GetScopes()
        {
            return this.GetRawScopes().Except(new string[] { OidcConstants.Scopes.OpenId });
        }

        public void SetScopes(IEnumerable<string> scopes)
        {
            scopes = scopes ?? Enumerable.Empty<string>();
            ValidateScopes(scopes);
            
            var intersection = GetScopes().Intersect(scopes);
            var newScopes = new List<string>()
            {
                OidcConstants.Scopes.OpenId
            };
            newScopes.AddRange(intersection);

            SetRawScopes(newScopes);
        }
    }
}
