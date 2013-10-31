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
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Authorization.Mvc;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.Controllers
{
    [ClaimsAuthorize(Constants.Actions.Administration, Constants.Resources.Configuration)]
    public class MenuController : System.Web.Mvc.Controller
    {
        [Import]
        public IConfigurationRepository ConfigurationRepository { get; set; }

        public MenuController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public MenuController(IConfigurationRepository configuration)
        {
            ConfigurationRepository = configuration;
        }

        [ChildActionOnly]
        public ActionResult Index()
        {
            var vm = new MenuViewModel(this.ConfigurationRepository);
            return PartialView("Index", vm);
        }

        bool IsController(string controller)
        {
            ControllerContext ctx = this.ControllerContext;
            while (ctx.ParentActionViewContext != null)
            {
                ctx = ctx.ParentActionViewContext;
            }
            var val = (string)ctx.RouteData.Values["controller"];
            return controller.Equals(val, StringComparison.OrdinalIgnoreCase);
        }
        
        bool IsAction(params string[] actions)
        {
            ControllerContext ctx = this.ControllerContext;
            while (ctx.ParentActionViewContext != null)
            {
                ctx = ctx.ParentActionViewContext;
            }
            var action = (string)ctx.RouteData.Values["action"];
            return actions.Contains(action, StringComparer.OrdinalIgnoreCase);
        }

        [ChildActionOnly]
        public ActionResult ChildMenu(string controllerName)
        {
            if (IsController(controllerName))
            {
                return PartialView("LoadChildMenu", controllerName);
            }

            return new EmptyResult();
        }
    }
}
