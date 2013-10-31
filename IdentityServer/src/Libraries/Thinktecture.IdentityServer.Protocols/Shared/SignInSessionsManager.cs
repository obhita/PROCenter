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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thinktecture.IdentityServer.Protocols
{
    public class SignInSessionsManager
    {
        string _cookieName;
        HttpContextBase _context;
        int _maximumCookieLifetime;

        public SignInSessionsManager(HttpContextBase context, string cookieName) : this(context, cookieName, 24)
        { }

        public SignInSessionsManager(HttpContextBase context, string cookieName, int maximumCookieLifetime)
        {
            _context = context;
            _cookieName = cookieName;
            _maximumCookieLifetime = maximumCookieLifetime;
        }

        public void AddEndpoint(string address)
        {
            var endpoints = ReadCookie();
            if (!endpoints.Contains(address))
            {
                endpoints.Add(address);
                WriteCookie(endpoints);
            }
        }

        public void SetEndpoint(string address)
        {
            ClearEndpoints();
            WriteCookie(new List<string> { address });
        }

        public List<string> GetEndpoints()
        {
            return ReadCookie();
        }

        public void ClearEndpoints()
        {
            var cookie = _context.Request.Cookies[_cookieName];
            if (cookie != null)
            {
                cookie.Value = "";
                cookie.Expires = new DateTime(2000, 1, 1);
                cookie.Path = HttpRuntime.AppDomainAppVirtualPath;

                _context.Response.SetCookie(cookie);
            }
        }

        private List<string> ReadCookie()
        {
            var cookie = _context.Request.Cookies[_cookieName];
            if (cookie == null)
            {
                return new List<string>();
            }

            return cookie.Value.Split('|').ToList();
        }

        private void WriteCookie(List<string> realms)
        {
            if (realms.Count == 0)
            {
                ClearEndpoints();
                return;
            }

            var realmString = string.Join("|", realms);

            var cookie = new HttpCookie(_cookieName, realmString)
            {
                Secure = true,
                HttpOnly = true,
                Path = HttpRuntime.AppDomainAppVirtualPath
            };

            _context.Response.Cookies.Add(cookie);
        }
    }
}
