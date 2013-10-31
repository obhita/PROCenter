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
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels
{
    public class MenuViewModel
    {
        public MenuViewModel(IConfigurationRepository configuration)
        {
            this.ShowIdentityProviders = configuration.WSFederation.Enabled && configuration.WSFederation.EnableFederation;
            this.ShowOpenIdConnectClients = configuration.OpenIdConnect.Enabled;
            this.ShowOAuthClients = configuration.OAuth2.Enabled;
            this.ShowOAuthTokens = 
                configuration.OAuth2.Enabled && 
                (configuration.OAuth2.EnableCodeFlow || configuration.OAuth2.EnableResourceOwnerFlow);
            this.ShowClientCerts = configuration.Global.EnableClientCertificateAuthentication;
            this.ShowIdentityDelegation = configuration.WSTrust.Enabled && configuration.WSTrust.EnableDelegation;
        }

        public bool ShowIdentityProviders { get; private set; }
        public bool ShowOpenIdConnectClients { get; private set; }
        public bool ShowOAuthClients { get; private set; }
        public bool ShowOAuthTokens { get; private set; }
        public bool ShowClientCerts { get; private set; }
        public bool ShowIdentityDelegation { get; private set; }
    }
}