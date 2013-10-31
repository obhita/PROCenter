namespace ProCenter.Mvc.Infrastructure.Security
{
    #region

    using System;
    using System.IdentityModel.Services;
    using System.Net.Http;

    #endregion

    public static class IdentityServerUtil
    {
        public static string BaseAddress
        {
            get { return ((FederatedAuthentication.WSFederationAuthenticationModule ?? new WSFederationAuthenticationModule()).Issuer.Replace("issue/wsfed", "")); }
        }
    }
}