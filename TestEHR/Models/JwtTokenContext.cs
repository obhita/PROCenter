#region License Header
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

                var response = httpClient.GetAsync(@"issue/simple?realm=" + WebConfigurationManager.AppSettings["WsFederationRealm"] + "&tokenType=jwt").Result;
                //var response = httpClient.GetAsync("https://localhost:44305/issue/simple?realm=https://localhost:44302/api/&tokenType=jwt").Result;//@"issue/simple?realm=" + @"https://localhost:44302/api/" + "&tokenType=jwt").Result;
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