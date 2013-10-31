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
namespace Thinktecture.IdentityServer.Web.App_Start
{
    #region

    using System;
    using System.Text;
    using System.Web.Http;
    using System.Web.Security;
    using IdentityModel.Http.Cors;
    using IdentityModel.Http.Cors.WebApi;
    using IdentityModel.Tokens.Http;
    using Repositories;
    using Security;

    #endregion

    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config, IConfigurationRepository configurationRepository)
        {
            var corsConfiguration = new CorsConfiguration();
            corsConfiguration.AllowAll();
            var corsMessageHandler = new CorsMessageHandler(corsConfiguration, config);
            config.MessageHandlers.Add(corsMessageHandler);

            var authentication = CreateAuthenticationConfiguration(configurationRepository);
            //config.MessageHandlers.Add(new AuthenticationHandler(authentication));

            var authentificationhandler = new AuthenticationHandler(new IdentityServerHttpAuthentication(authentication, configurationRepository), config);
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}/{username}", new {username = RouteParameter.Optional}, null, authentificationhandler);
        }

        private static AuthenticationConfiguration CreateAuthenticationConfiguration(IConfigurationRepository configurationRepository)
        {
            const string audience = "api/";
            var issuerUri = configurationRepository.Global.IssuerUri;
            if (configurationRepository.Keys.SigningCertificate == null)
            {
                //Note: when set up Identity server 1st time, it goes here. After the initial configuration, please re-start IIS to make sure the following code executed.
                return null;
            }

            var signingKey = configurationRepository.Keys.SigningCertificate.Thumbprint;
            var authenticationConfiguration = new AuthenticationConfiguration
                {
                    ClaimsAuthenticationManager = new ClaimsTransformer(),
                    RequireSsl = false,
                    EnableSessionToken = true,
                    SessionToken = new SessionTokenConfiguration
                        {
                            DefaultTokenLifetime = TimeSpan.FromHours(10.0),
                            EndpointAddress = "/issue/simple",
                            HeaderName = "Authorization",
                            Scheme = "Session",
                            Audience = audience,
                            IssuerName = issuerUri,
                            SigningKey = Encoding.UTF8.GetBytes(signingKey),
                        }
                };
            // IdentityServer JWT
            authenticationConfiguration.AddJsonWebToken(issuerUri, audience, signingKey);

            authenticationConfiguration.AddBasicAuthentication(Membership.ValidateUser);

            //Client Certificates
            authenticationConfiguration.AddClientCertificate(ClientCertificateMode.ChainValidation);

            return authenticationConfiguration;
        }
    }
}