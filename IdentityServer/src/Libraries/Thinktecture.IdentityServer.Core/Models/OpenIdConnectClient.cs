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
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Thinktecture.IdentityServer.Models
{
    public class OpenIdConnectClient : IValidatableObject
    {
        // general
        [Required]
        [ScaffoldColumn(false)]
        [Display(Name="Client ID", Description="Unique identifier for the client.")]
        public string ClientId { get; set; }
        
        [Display(Name = "Client Secret", Description = "Password for the client.")]
        public string ClientSecret { get; set; }
        
        [ScaffoldColumn(false)]
        [UIHint("Enum")]
        public ClientSecretTypes ClientSecretType { get; set; }
        
        [Required]
        [Display(Name = "Name", Description = "Display name for the client.")]
        public string Name { get; set; }
        
        // openid connect
        [Display(Name = "Flow", Description = "OAuth2 flow for the client -- either server-side client (code) or naitve/javascript client (implicit).")]
        [UIHint("Enum")]
        public OpenIdConnectFlows Flow { get; set; }

        [Display(Name = "Access Token Lifetime", Description = "Lifetime (in minutes) of access token issued to client.")]
        public int AccessTokenLifetime { get; set; }

        [Display(Name = "Allow Refresh Token", Description = "Only allowed for code flow clients.")]
        public bool AllowRefreshToken { get; set; }

        [Display(Name = "Refresh Token Lifetime", Description = "Lifetime (in minutes) of refresh token issued to client. Only allowed for code flow clients.")]
        public int RefreshTokenLifetime { get; set; }

        [Display(Name = "Require Consent", Description = "For this client should user be prompted to grant consent to access the user's profile data.")]
        public bool RequireConsent { get; set; }

        [ScaffoldColumn(false)]
        public string[] RedirectUris { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}