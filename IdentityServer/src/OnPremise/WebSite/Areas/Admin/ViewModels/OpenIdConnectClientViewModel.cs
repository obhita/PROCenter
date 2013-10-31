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
using System.ComponentModel.DataAnnotations;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Repositories;
using System.Linq;
using System;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class OpenIdConnectClientInputModel : IValidatableObject
    {
        public OpenIdConnectClient Client { get; set; }

        public void MapRedirectUris()
        {
            this.RedirectUris = null;
            if (this.Client.RedirectUris != null && this.Client.RedirectUris.Any())
            {
                this.RedirectUris = this.Client.RedirectUris.Aggregate((x, y) => x + System.Environment.NewLine + y);
            }
        }
        
        [DataType(DataType.MultilineText)]
        [Display(Name = "Redirect Uris", Description = "List of URIs allowed to redirect to in OAuth2 protocol.")]
        public string RedirectUris { get; set; }
        
        public string[] ParsedRedirectUris
        {
            get
            {
                if (this.RedirectUris == null)
                {
                    return new string[0];
                }
                else
                {
                    return this.RedirectUris.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);
                }
            }
        }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var item in ParsedRedirectUris)
            {
                Uri val;
                if (!Uri.TryCreate(item, UriKind.Absolute, out val))
                {
                    yield return new ValidationResult(item + " is not a valid URI", new string[] { "RedirectUris" });
                }
            }
        }
    }

    public class OpenIdConnectClientViewModel : OpenIdConnectClientInputModel
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public OpenIdConnectClientViewModel(Models.OpenIdConnectClient client)
        {
            Container.Current.SatisfyImportsOnce(this);
            this.Client = client;
            this.MapRedirectUris();
        }

        public bool IsNew
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.Client.ClientId);
            }
        }

        public bool IsOAuthRefreshTokenEnabled
        {
            get
            {
                return !IsNew && Client.Flow == OpenIdConnectFlows.AuthorizationCode;
            }
        }
    }
}