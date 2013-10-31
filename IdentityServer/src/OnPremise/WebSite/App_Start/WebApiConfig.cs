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