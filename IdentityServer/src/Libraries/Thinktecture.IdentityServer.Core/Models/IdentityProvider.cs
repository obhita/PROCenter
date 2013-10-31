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
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Models
{
    public class IdentityProvider : IValidatableObject
    {
        [UIHint("HiddenInput")]
        public int ID { get; set; }

        [Required]
        [Display(Order = 1, ResourceType = typeof (Resources.Models.IdentityProvider), Name = "Name", Description = "NameDescription")]
        public string Name { get; set; }

        [Required]
        [Display(Order = 2, ResourceType = typeof (Resources.Models.IdentityProvider), Name = "DisplayName", Description = "DisplayNameDescription")]
        public string DisplayName { get; set; }

        [Display(Order = 3, ResourceType = typeof (Resources.Models.IdentityProvider), Name = "Enabled", Description = "EnabledDescription")]
        public bool Enabled { get; set; }

        [Display(Order = 4, ResourceType = typeof (Resources.Models.IdentityProvider), Name = "ShowInHrdSelection", Description = "ShowInHrdSelectionDescription")]
        public bool ShowInHrdSelection { get; set; }

        [Required]
        [UIHint("Enum")]
        [Display(Order = 5, ResourceType = typeof (Resources.Models.IdentityProvider), Name = "Type", Description = "TypeDescription")]
        public Models.IdentityProviderTypes Type { get; set; }

        [Display(Order = 6, ResourceType = typeof (Resources.Models.IdentityProvider), Name = "WSFederationEndpoint", Description = "WSFederationEndpointDescription")]
        [AbsoluteUri]
        public string WSFederationEndpoint { get; set; }

        string _IssuerThumbprint;
        [UIHint("Thumbprint")]
        [Display(Order = 7, ResourceType = typeof (Resources.Models.IdentityProvider), Name = "IssuerThumbprint", Description = "IssuerThumbprintDescription")]
        public string IssuerThumbprint
        {
            get
            {
                return _IssuerThumbprint;
            }
            set
            {
                _IssuerThumbprint = value;
                if (_IssuerThumbprint != null) _IssuerThumbprint = _IssuerThumbprint.Replace(" ", "");
            }
        }

        [Display(Order = 8, ResourceType = typeof (Resources.Models.IdentityProvider), Name = "ProviderType", Description = "ProviderTypeDescription")]
        [UIHint("Enum")]
        public OAuth2ProviderTypes? ProviderType { get; set; }

        [Display(Order = 9, ResourceType = typeof (Resources.Models.IdentityProvider), Name = "ClientID", Description = "ClientIDDescription")]
        public string ClientID { get; set; }

        [Display(Order = 10, ResourceType = typeof (Resources.Models.IdentityProvider), Name = "ClientSecret", Description = "ClientSecretDescription")]
        public string ClientSecret { get; set; }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (this.Type == IdentityProviderTypes.WSStar)
            {
                if (String.IsNullOrEmpty(this.WSFederationEndpoint))
                {
                    errors.Add(new ValidationResult(Resources.Models.IdentityProvider.WSFederationEndpointRequiredError, new string[] { "WSFederationEndpoint" }));
                }
                if (String.IsNullOrEmpty(this.IssuerThumbprint))
                {
                    errors.Add(new ValidationResult(Resources.Models.IdentityProvider.IssuerThumbprintRequiredError, new string[] { "IssuerThumbprint" }));
                }
            }
            if (this.Type == IdentityProviderTypes.OAuth2)
            {
                if (String.IsNullOrEmpty(this.ClientID))
                {
                    errors.Add(new ValidationResult(Resources.Models.IdentityProvider.ClientIDRequiredError, new string[] { "ClientID" }));
                }
                if (String.IsNullOrEmpty(this.ClientSecret))
                {
                    errors.Add(new ValidationResult(Resources.Models.IdentityProvider.ClientSecretRequiredError, new string[] { "ClientSecret" }));
                }
                if (this.ProviderType == null)
                {
                    errors.Add(new ValidationResult(Resources.Models.IdentityProvider.ProviderTypeRequiredError, new string[] { "ProfileType" }));
                }
            }

            return errors;
        }
    }
}