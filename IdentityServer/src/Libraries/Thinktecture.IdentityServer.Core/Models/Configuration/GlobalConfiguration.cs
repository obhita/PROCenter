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
using System;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class GlobalConfiguration
    {
        [Display(ResourceType = typeof (Resources.Models.Configuration.GlobalConfiguration), Name = "SiteName", Description = "SiteNameDescription")]
        [Required]
        public String SiteName { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Configuration.GlobalConfiguration), Name = "IssuerUri", Description = "IssuerUriDescription")]
        [Required]
        public String IssuerUri { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Configuration.GlobalConfiguration), Name = "IssuerContactEmail", Description = "IssuerContactEmailDescription")]
        //[RegularExpression(@"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]\b", ErrorMessage = "{0} must be in the form of an email address.")]
        [Required]
        public String IssuerContactEmail { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Configuration.GlobalConfiguration), Name = "DefaultWSTokenType", Description = "DefaultWSTokenTypeDescription")]
        [Required]
        public string DefaultWSTokenType { get; set; }

        [Display(ResourceType = typeof(Resources.Models.Configuration.GlobalConfiguration), Name = "DefaultHttpTokenType", Description = "DefaultHttpTokenTypeDescription")]
        [Required]
        public string DefaultHttpTokenType { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Configuration.GlobalConfiguration), Name = "DefaultTokenLifetime", Description = "DefaultTokenLifetimeDescription")]
        [Range(0, Int32.MaxValue)]
        public int DefaultTokenLifetime { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Configuration.GlobalConfiguration), Name = "MaximumTokenLifetime", Description = "MaximumTokenLifetimeDescription")]
        [Range(0, Int32.MaxValue)]
        public int MaximumTokenLifetime { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Configuration.GlobalConfiguration), Name = "SsoCookieLifetime", Description = "SsoCookieLifetimeDescription")]
        [Range(0, Int32.MaxValue)]
        public int SsoCookieLifetime { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Configuration.GlobalConfiguration), Name = "RequireEncryption", Description = "RequireEncryptionDescription")]
        public Boolean RequireEncryption { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Configuration.GlobalConfiguration), Name = "RequireRelyingPartyRegistration", Description = "RequireRelyingPartyRegistrationDescription")]
        public Boolean RequireRelyingPartyRegistration { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Configuration.GlobalConfiguration), Name = "EnableClientCertificateAuthentication", Description = "EnableClientCertificateAuthenticationDescription")]
        public Boolean EnableClientCertificateAuthentication { get; set; }

        // TODO : Name = "Only Users in the '" + Constants.Roles.IdentityServerUsers + "' role can request tokens"
        [Display(ResourceType = typeof (Resources.Models.Configuration.GlobalConfiguration), Name = "EnforceUsersGroupMembership", Description = "EnforceUsersGroupMembershipDescription")]
        public Boolean EnforceUsersGroupMembership { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Configuration.GlobalConfiguration), Name = "HttpPort", Description = "HttpPortDescription")]
        [Range(0, Int32.MaxValue)]
        public int HttpPort { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Configuration.GlobalConfiguration), Name = "HttpsPort", Description = "HttpsPortDescription")]
        [Range(0, Int32.MaxValue)]
        public int HttpsPort { get; set; }
    }
}
