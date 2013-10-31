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
    public class Client : IValidatableObject
    {
        [UIHint("HiddenInput")]
        public int ID { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Client), Name = "Name", Description = "NameDescription")]
        [Required]
        public string Name { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Client), Name = "Description", Description = "DescriptionDescription")]
        [Required]
        public string Description { get; set; }
        
        [Display(ResourceType = typeof (Resources.Models.Client), Name = "ClientId", Description = "ClientIdDescription")]
        [Required]
        public string ClientId { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Client), Name = "ClientSecret", Description = "ClientSecretDescription")]
        [UIHint("SymmetricKey")]
        public string ClientSecret { get; set; }

        [UIHint("HiddenInput")]
        public bool HasClientSecret { get; set; }

        [AbsoluteUri]
        [Display(ResourceType = typeof (Resources.Models.Client), Name = "RedirectUri", Description = "RedirectUriDescription")]
        public Uri RedirectUri { get; set; }

        //[Display(Name = "Native Client", Description = "Native Client.")]
        //[UIHint("HiddenInput")]
        //public bool NativeClient { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Client), Name = "AllowImplicitFlow", Description = "AllowImplicitFlowDescription")]
        public bool AllowImplicitFlow { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Client), Name = "AllowResourceOwnerFlow", Description = "AllowResourceOwnerFlowDescription")]
        public bool AllowResourceOwnerFlow { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Client), Name = "AllowCodeFlow", Description = "AllowCodeFlowDescription")]
        public bool AllowCodeFlow { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Client), Name = "AllowRefreshToken", Description = "AllowRefreshTokenDescription")]
        public bool AllowRefreshToken { get; set; }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (!HasClientSecret &&
                String.IsNullOrWhiteSpace(this.ClientSecret) &&
                (this.AllowCodeFlow || this.AllowResourceOwnerFlow))
            {
                errors.Add(new ValidationResult(Resources.Models.Client.ClientSecretRequiredError, new string[] { "ClientSecret" }));
            }

            if (this.RedirectUri == null &&
                (this.AllowCodeFlow || this.AllowImplicitFlow))
            {
                errors.Add(new ValidationResult(Resources.Models.Client.RedirectUriRequiredError, new string[] { "RedirectUri" }));
            }

            if (this.RedirectUri != null && this.RedirectUri.Scheme == Uri.UriSchemeHttp)
            {
                errors.Add(new ValidationResult(Resources.Models.Client.RedirectUriMustBeHTTPS, new string[] { "RedirectUri" }));
            }

            if (!this.AllowCodeFlow && !this.AllowResourceOwnerFlow && this.AllowRefreshToken)
            {
                errors.Add(new ValidationResult("Refresh tokens only allowed with Code or Resource Owner flows.", new string[] { "AllowRefreshToken" }));
            }

            return errors;
        }
    }
}
