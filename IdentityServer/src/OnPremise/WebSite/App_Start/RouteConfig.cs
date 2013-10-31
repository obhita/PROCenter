#region Licence Header
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
using System.Web.Mvc;
using System.Web.Routing;
using Thinktecture.IdentityServer.Repositories;

namespace Thinktecture.IdentityServer.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes, IConfigurationRepository configuration, IUserRepository userRepository)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Administration & Configuration
            routes.MapRoute(
                "InitialConfiguration",
                "initialconfiguration",
                new { controller = "InitialConfiguration", action = "Index" }
            );

            //routes.MapRoute(
            //    "Admin",
            //    "admin/{action}/{id}",
            //    new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            //);

            //routes.MapRoute(
            //    "RelyingPartiesAdmin",
            //    "admin/relyingparties/{action}/{id}",
            //    new { controller = "RelyingPartiesAdmin", action = "Index", id = UrlParameter.Optional }
            //);

            //routes.MapRoute(
            //    "ClientCertificatesAdmin",
            //    "admin/clientcertificates/{action}/{userName}",
            //    new { controller = "ClientCertificatesAdmin", action = "Index", userName = UrlParameter.Optional }
            //);

            //routes.MapRoute(
            //    "DelegationAdmin",
            //    "admin/delegation/{action}/{userName}",
            //    new { controller = "DelegationAdmin", action = "Index", userName = UrlParameter.Optional }
            //);
            #endregion

            #region Main UI
            routes.MapRoute(
                "Account",
                "account/{action}",
                new { controller = "Account", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Home",
                "{action}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[]{"Thinktecture.IdentityServer.Web.Controllers"}
            );
            #endregion
        }

    }
}