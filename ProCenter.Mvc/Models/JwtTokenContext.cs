namespace ProCenter.Mvc.Models
{
    using System;
    using System.Net.Http;
    using System.Reflection;
    using System.Web;
    using Infrastructure.Security;
    using Newtonsoft.Json.Linq;
    using Pillar.Common.Configuration;
    using Pillar.Common.InversionOfControl;

    public class JwtTokenContext
    {
        private static readonly object Sync = new object();
        private const string JwtTokenContextKey = "JwtTokenContext";

        public JwtTokenContext()
        {
            var appSettingsConfiguration = IoC.CurrentContainer.Resolve<IConfigurationPropertiesProvider>();
            var baseAddress = new Uri(IdentityServerUtil.BaseAddress);
            DateTime expiration;
            Token = RequestSessionToken(baseAddress, appSettingsConfiguration.GetProperty<string>("IdentityServerWebApiUsername"),
                                        appSettingsConfiguration.GetProperty<string>("IdentityServerWebApiPassword"), out expiration);
            _expiration = expiration;
        }


        private static string RequestSessionToken(Uri baseAddress, string admin, string adminPassword, out DateTime expiration)
        {
            using (var httpClient = new HttpClient {BaseAddress = baseAddress})
            {
                httpClient.SetBasicAuthentication(admin, adminPassword);

                var response = httpClient.GetAsync("issue/simple?realm=" + baseAddress + "api/&tokenType=jwt").Result;
                response.EnsureSuccessStatusCode();

                var tokenResponse = response.Content.ReadAsStringAsync().Result;
                var json = JObject.Parse(tokenResponse);
                var token = json["access_token"].ToString();
                var expiresIn = int.Parse(json["expires_in"].ToString());
                expiration = DateTime.UtcNow.AddSeconds(expiresIn);

                return token;
            }
        }

        public string Token { get; set; }
        private readonly DateTime _expiration;

        public static JwtTokenContext Current
        {
            get
            {
                if (HttpContext.Current.Application[JwtTokenContextKey] == null)
                {
                    lock (Sync)
                    {
                        if (HttpContext.Current.Application[JwtTokenContextKey] == null)
                        {
                            HttpContext.Current.Application[JwtTokenContextKey] = new JwtTokenContext();
                        }
                    }
                }
                else if (HttpContext.Current.Application[JwtTokenContextKey] != null)
                {
                    lock (Sync)
                    {
                        if (HttpContext.Current.Application[JwtTokenContextKey] != null)
                        {
                            var webApiJwtTokenContext = HttpContext.Current.Application[JwtTokenContextKey] as JwtTokenContext;
                            if (webApiJwtTokenContext != null && webApiJwtTokenContext._expiration <= DateTime.Now.AddMinutes(1))
                            {
                                HttpContext.Current.Application[JwtTokenContextKey] = new JwtTokenContext();
                            }
                        }
                    }
                }
                return HttpContext.Current.Application[JwtTokenContextKey] as JwtTokenContext;
            }
        }
    }
}