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
using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Web.GlobalFilter
{
    public class SslRedirectFilter : ActionFilterAttribute
    {
        int _port = 443;

        public SslRedirectFilter(int sslPort)
        {
            _port = sslPort;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsSecureConnection)
            {
                filterContext.Result = new RedirectResult(
                    GetAbsoluteUri(filterContext.HttpContext.Request.Url).AbsoluteUri,
                    true);
            }
        }

        private Uri GetAbsoluteUri(Uri uriFromCaller)
        {
            UriBuilder builder = new UriBuilder(Uri.UriSchemeHttps, uriFromCaller.Host);
            builder.Path = uriFromCaller.GetComponents(UriComponents.Path, UriFormat.Unescaped);
            builder.Port = _port;

            string query = uriFromCaller.GetComponents(UriComponents.Query, UriFormat.UriEscaped);
            if (query.Length > 0)
            {
                string uriWithoutQuery = builder.Uri.AbsoluteUri;
                string absoluteUri = string.Format("{0}?{1}", uriWithoutQuery, query);
                return new Uri(absoluteUri, UriKind.Absolute);
            }
            else
            {
                return builder.Uri;
            }
        }
    }
}