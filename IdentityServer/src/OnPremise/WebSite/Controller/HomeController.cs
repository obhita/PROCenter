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
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using System.Web.WebPages;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web.Controllers
{
    public class HomeController : System.Web.Mvc.Controller
    {
        [Import]
        public IConfigurationRepository Configuration { get; set; }

        public HomeController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public HomeController(IConfigurationRepository configuration)
        {
            Configuration = configuration;
        }

        public ActionResult Index()
        {
            if (Request.Browser.IsMobileDevice)
            {
                HttpContext.SetOverriddenBrowser(BrowserOverride.Desktop);
            }

            return View();
        }

        public ActionResult AppIntegration()
        {
            var endpoints = Endpoints.Create(
                               HttpContext.Request.Headers["Host"],
                               HttpContext.Request.ApplicationPath,
                               Configuration.Global.HttpPort,
                               Configuration.Global.HttpsPort);

            var list = new Dictionary<string, string>();

            // federation metadata
            if (Configuration.FederationMetadata.Enabled)
            {
                list.Add("WS-Federation metadata", endpoints.WSFederationMetadata.AbsoluteUri);
            }

            // ws-federation
            if (Configuration.WSFederation.Enabled)
            {
                if (Configuration.WSFederation.EnableAuthentication)
                {
                    list.Add("WS-Federation", endpoints.WSFederation.AbsoluteUri);
                }
                if (Configuration.WSFederation.EnableFederation)
                {
                    list.Add("WS-Federation HRD", endpoints.WSFederationHRD.AbsoluteUri);
                    list.Add("OAuth2 Callback", endpoints.OAuth2Callback.AbsoluteUri);
                }
            }

            // ws-trust
            if (Configuration.WSTrust.Enabled)
            {
                list.Add("WS-Trust metadata", endpoints.WSTrustMex.AbsoluteUri);

                if (Configuration.WSTrust.EnableMessageSecurity)
                {
                    list.Add("WS-Trust message security (user name)", endpoints.WSTrustMessageUserName.AbsoluteUri);

                    if (Configuration.WSTrust.EnableClientCertificateAuthentication)
                    {
                        list.Add("WS-Trust message security (client certificate)", endpoints.WSTrustMessageCertificate.AbsoluteUri);
                    }
                }

                if (Configuration.WSTrust.EnableMixedModeSecurity)
                {
                    list.Add("WS-Trust mixed mode security (user name)", endpoints.WSTrustMixedUserName.AbsoluteUri);

                    if (Configuration.WSTrust.EnableClientCertificateAuthentication)
                    {
                        list.Add("WS-Trust mixed mode security (client certificate)", endpoints.WSTrustMixedCertificate.AbsoluteUri);
                    }
                }
            }

            // openid connect
            if (Configuration.OpenIdConnect.Enabled)
            {
                list.Add("OpenID Connect Authorize", endpoints.OidcAuthorize.AbsoluteUri);
                list.Add("OpenID Connect Token", endpoints.OidcToken.AbsoluteUri);
                list.Add("OpenID Connect UserInfo", endpoints.OidcUserInfo.AbsoluteUri);
            }

            // oauth2
            if (Configuration.OAuth2.Enabled)
            {
                if (Configuration.OAuth2.EnableImplicitFlow)
                {
                    list.Add("OAuth2 Authorize", endpoints.OAuth2Authorize.AbsoluteUri);
                }
                if (Configuration.OAuth2.EnableResourceOwnerFlow)
                {
                    list.Add("OAuth2 Token", endpoints.OAuth2Token.AbsoluteUri);
                }
            }

            // adfs integration
            if (Configuration.AdfsIntegration.Enabled)
            {
                if (Configuration.AdfsIntegration.UsernameAuthenticationEnabled || 
                    Configuration.AdfsIntegration.SamlAuthenticationEnabled || 
                    Configuration.AdfsIntegration.JwtAuthenticationEnabled)
                {
                    list.Add("ADFS Integration", endpoints.AdfsIntegration.AbsoluteUri);
                }
            }

            // simple http
            if (Configuration.SimpleHttp.Enabled)
            {
                list.Add("Simple HTTP", endpoints.SimpleHttp.AbsoluteUri);
            }

            return View(list);
        }
    }
}
