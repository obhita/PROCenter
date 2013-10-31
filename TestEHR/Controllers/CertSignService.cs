namespace TestEHR.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Web;

    public static class CertSignService
    {
        public static byte[] SignCertificate(string text, string signingCertName)
        {
            // Open certificate store of current user
            var my = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            my.Open(OpenFlags.ReadOnly);

            // Look for the certificate with specific subject 
            var csp = my.Certificates.Cast<X509Certificate2>()
                        .Where(
                            cert =>
                            cert.Subject.Contains("CN=" + signingCertName))
                        .Select(cert => (RSACryptoServiceProvider) cert.PrivateKey)
                        .FirstOrDefault();
            if (csp == null)
            {
                throw new Exception("Valid certificate was not found");
            }

            // Hash the data
            var sha1 = new SHA1Managed();
            var data = Encoding.Unicode.GetBytes(text);
            var hash = sha1.ComputeHash(data);

            // Sign the hash
            return csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
        }

        public static string BuildReturnUrl(string issuer, string realm, string controllerAndAction,
                                            string actionParameter, DateTime now, Dictionary<string, string> queryStrings = null)
        {
            const string returnUrlFormat = "{0}?wa=wsignin1.0&wtrealm={1}&wctx={2}&wct={3}";
            const string wctxFormat = "rm=0&id=passive&ru={0}{1}{2}";

            var issuerUri = new Uri(issuer);
            var parameters = "?";
            if (queryStrings != null)
            {
                parameters = queryStrings.Aggregate(parameters, (current, s) => current + (s.Key + "=" + s.Value + "&"));
            }
            parameters = parameters.TrimEnd(new[] {'&'});

            var wctx = string.Format(wctxFormat,
                                     controllerAndAction,
                                     actionParameter,
                                     parameters == "?" ? string.Empty : parameters);

            var returnUrl = string.Format(returnUrlFormat,
                                          issuerUri.LocalPath,
                                          HttpUtility.UrlEncode(realm),
                                          HttpUtility.UrlEncode(wctx),
                                          HttpUtility.UrlEncode(now.ToString("s") + "Z"));// date time in ISO 8601 format
            return returnUrl;
        }
    }
}