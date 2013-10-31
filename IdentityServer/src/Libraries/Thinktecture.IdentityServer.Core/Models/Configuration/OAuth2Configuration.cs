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
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Models.Configuration
{
    public class OAuth2Configuration : ProtocolConfiguration
    {
        [Display(ResourceType = typeof (Resources.Models.Configuration.OAuth2Configuration), Name = "EnableImplicitFlow", Description = "EnableImplicitFlowDescription")]
        public bool EnableImplicitFlow { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Configuration.OAuth2Configuration), Name = "EnableResourceOwnerFlow", Description = "EnableResourceOwnerFlowDescription")]
        public bool EnableResourceOwnerFlow { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Configuration.OAuth2Configuration), Name = "EnableCodeFlow", Description = "EnableCodeFlowDescription")]
        public bool EnableCodeFlow { get; set; }

        [Display(ResourceType = typeof (Resources.Models.Configuration.OAuth2Configuration), Name = "EnableConsent", Description = "EnableConsentDescription")]
        public bool EnableConsent { get; set; }
    }
}
