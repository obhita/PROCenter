namespace TestEHR.Models
{
    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Reflection;
    using System.Web;
    using System.Web.Configuration;
    using Controllers;
    using Newtonsoft.Json.Linq;

    public class JwtTokenContext
    {
        private static readonly object Sync = new object();
        private const string JwtTokenContextKey = "JwtTokenContext";

        public JwtTokenContext()
        {
            var baseAddress = new Uri(WebConfigurationManager.AppSettings["WsFederationIssuer"].Replace("issue/wsfed",""));
            DateTime expiration;
            Token = RequestSessionToken(baseAddress, "a", "123456", out expiration);
            _expiration = expiration;
        }


        private static string RequestSessionToken(Uri baseAddress, string admin, string adminPassword, out DateTime expiration)
        {
            using (var httpClient = new HttpClient ())//{ BaseAddress = baseAddress })
            {
                httpClient.SetBasicAuthentication(admin, adminPassword);

                //var response = httpClient.GetAsync(@"issue/simple?realm=" + WebConfigurationManager.AppSettings["WsFederationRealm"] + "&tokenType=jwt").Result;
                var response = httpClient.GetAsync("https://localhost:44305/issue/simple?realm=https://localhost:44302/api/&tokenType=jwt").Result;//@"issue/simple?realm=" + @"https://localhost:44302/api/" + "&tokenType=jwt").Result;
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