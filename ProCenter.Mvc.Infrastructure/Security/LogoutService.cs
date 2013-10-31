namespace ProCenter.Mvc.Infrastructure.Security
{
    using System;
    using System.IdentityModel.Services;
    using Domain.SecurityModule;
    using NLog;

    public class LogoutService : ILogoutService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public SignOutRequestMessage Logout()
        {
            WSFederationAuthenticationModule federationAuthenticationModule;

            if (FederatedAuthentication.WSFederationAuthenticationModule != null)
            {
                Logger.Debug("Returning current {0}.", typeof (WSFederationAuthenticationModule).Name);
                federationAuthenticationModule = FederatedAuthentication.WSFederationAuthenticationModule;
            }
            else
            {
                Logger.Debug("Returning a new {0}.", typeof (WSFederationAuthenticationModule).Name);
                federationAuthenticationModule = new WSFederationAuthenticationModule();
            }

            Logger.Debug(
                "Initiating: SignOff.  Calling the SignOff method of the WSFederationAuthenticationModule. DateTime Utc: " +
                DateTime.UtcNow);

            federationAuthenticationModule.SignOut(false);

            return new SignOutRequestMessage(new Uri(federationAuthenticationModule.Issuer),
                                             federationAuthenticationModule.Realm);
        }
    }
}