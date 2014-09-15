
namespace Thinktecture.IdentityServer.Web.Security
{
    #region

    using System.IdentityModel.Tokens;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using System.Web;
    using IdentityModel;
    using IdentityModel.Tokens.Http;
    using Repositories;

    #endregion

    public class IdentityServerHttpAuthentication : HttpAuthentication
    {
        private readonly IConfigurationRepository _configurationRepository;

        public IdentityServerHttpAuthentication(AuthenticationConfiguration configuration, IConfigurationRepository configurationRepository)
            : base(configuration)
        {
            _configurationRepository = configurationRepository;
        }

        public override ClaimsPrincipal AuthenticateSessionToken(HttpRequestMessage request)
        {
            // grab header
            var headerValues = request.Headers.SingleOrDefault(h => h.Key == Configuration.SessionToken.HeaderName).Value;
            if (headerValues != null)
            {
                var header = headerValues.SingleOrDefault();
                if (header != null)
                {
                    var parts = header.Split(' ');
                    if (parts.Length == 2)
                    {
                        // if configured scheme was sent, try to authenticate the session token
                        if (parts[0] == Configuration.SessionToken.Scheme)
                        {
                            var token = new JwtSecurityToken(parts[1]);

                            if (_configurationRepository.Keys.SigningCertificate.Thumbprint != null)
                            {
                                var store = new X509Store(StoreName.TrustedPeople, StoreLocation.LocalMachine);
                                store.Open(OpenFlags.ReadOnly);
                                var cert = store.Certificates.Find(X509FindType.FindByThumbprint, _configurationRepository.Keys.SigningCertificate.Thumbprint, false)[0];
                                store.Close();

                                var validationParameters = new TokenValidationParameters
                                    {
                                        ValidIssuer = Configuration.SessionToken.IssuerName,
                                        AllowedAudience =
                                            HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                                            HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/" + Configuration.SessionToken.Audience,
                                        SigningToken = new X509SecurityToken(cert),
                                    };

                                var handler = new JwtSecurityTokenHandler();
                                return handler.ValidateToken(token, validationParameters);
                            }
                        }
                    }
                }
            }

            return Principal.Anonymous;
        }
    }
}