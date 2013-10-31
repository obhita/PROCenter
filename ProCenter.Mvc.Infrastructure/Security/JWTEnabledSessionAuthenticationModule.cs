namespace ProCenter.Mvc.Infrastructure.Security
{
    #region Using Statements

    using System;
    using System.IdentityModel.Configuration;
    using System.IdentityModel.Services;
    using System.IdentityModel.Tokens;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using System.Web;
    using Thinktecture.IdentityModel.Tokens.Http;

    #endregion

    /// <summary>
    ///     Session Authentication Module that can verify session by JWT Token in header.
    /// </summary>
    public class JWTEnabledSessionAuthenticationModule : SessionAuthenticationModule
    {
        #region Methods

        protected override void OnAuthenticateRequest ( object sender, EventArgs eventArgs )
        {
            var request = HttpContext.Current.Request;
            var authenticationConfiguration = new AuthenticationConfiguration
                {
                    RequireSsl = false,
                    EnableSessionToken = true,
                    SessionToken = new SessionTokenConfiguration
                        {
                            HeaderName = "Authorization",
                            Scheme = "Session",
                        }
                };
            if ( request.Headers.AllKeys.Any ( k => k == authenticationConfiguration.SessionToken.HeaderName ) )
            {
                var header = request.Headers.Get ( authenticationConfiguration.SessionToken.HeaderName );
                var parts = header.Split ( ' ' );
                if ( parts.Length == 2 )
                {
                    // if configured scheme was sent, try to authenticate the session token
                    if ( parts[0] == authenticationConfiguration.SessionToken.Scheme )
                    {
                        var token = new JwtSecurityToken ( parts[1] );

                        var identityConfiguratin = new IdentityConfiguration ();
                        if ( identityConfiguratin.IssuerNameRegistry is ConfigurationBasedIssuerNameRegistry )
                        {
                            var issuers = ( identityConfiguratin.IssuerNameRegistry as ConfigurationBasedIssuerNameRegistry ).ConfiguredTrustedIssuers;
                            if ( issuers.Any ( i => i.Value == token.Issuer ) )
                            {
                                var issuer = issuers.FirstOrDefault ( i => i.Value == token.Issuer );
                                var store = new X509Store ( StoreName.TrustedPeople, StoreLocation.LocalMachine );
                                store.Open ( OpenFlags.ReadOnly );
                                var cert = store.Certificates.Find ( X509FindType.FindByThumbprint, issuer.Key, false )[0];
                                store.Close ();

                                var validationParameters = new TokenValidationParameters
                                    {
                                        ValidIssuer = issuer.Value,
                                        AllowedAudiences = identityConfiguratin.AudienceRestriction.AllowedAudienceUris.Select ( uri => uri.OriginalString ),
                                        SigningToken = new X509SecurityToken ( cert ),
                                    };

                                var handler = new JwtSecurityTokenHandler ();
                                var claimsPrinciple = handler.ValidateToken ( token, validationParameters );
                                claimsPrinciple = identityConfiguratin.ClaimsAuthenticationManager.Authenticate ( request.RawUrl, claimsPrinciple );
                                if ( claimsPrinciple != null && claimsPrinciple.Identity.IsAuthenticated )
                                {
                                    HttpContext.Current.User = claimsPrinciple;
                                    Thread.CurrentPrincipal = claimsPrinciple;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            base.OnAuthenticateRequest ( sender, eventArgs );
        }

        #endregion
    }
}