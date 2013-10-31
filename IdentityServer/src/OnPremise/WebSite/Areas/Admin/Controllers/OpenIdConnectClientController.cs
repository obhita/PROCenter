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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Authorization.Mvc;
using Thinktecture.IdentityServer.Models;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.Controllers
{
    [ClaimsAuthorize(Constants.Actions.Administration, Constants.Resources.Configuration)]
    public class OpenIdConnectClientController : System.Web.Mvc.Controller
    {
        [Import]
        public IOpenIdConnectClientsRepository repository { get; set; }

        public OpenIdConnectClientController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }
        public OpenIdConnectClientController(IOpenIdConnectClientsRepository repo)
        {
            this.repository = repo;
        }

        public ActionResult Index()
        {
            var vm = new OpenIdConnectClientIndexViewModel(this.repository);
            return View("Index", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string action, OpenIdConnectClientIndexInputModel[] list)
        {
            if (action == "new") return RedirectToAction("Edit");
            if (action == "delete") return Delete(list);

            ModelState.AddModelError("", Resources.OpenIdConnectClientController.InvalidAction);
            return Index();
        }

        private ActionResult Delete(OpenIdConnectClientIndexInputModel[] list)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var client in list.Where(x => x.Delete))
                    {
                        this.repository.Delete(client.ClientId);
                    }
                    TempData["Message"] = Resources.OpenIdConnectClientController.ClientsDeleted;
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", Resources.OpenIdConnectClientController.ErrorDeletingClients);
                }
            }
            
            return Index();
        }

        public ActionResult Edit(string clientId)
        {
            OpenIdConnectClient client = null;
            if (!String.IsNullOrWhiteSpace(clientId))
            {
                client = this.repository.Get(clientId);
                if (client == null) return HttpNotFound();
            }
            else
            {
                client = new OpenIdConnectClient();
            }

            var vm = new OpenIdConnectClientViewModel(client); 
            return View("Edit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OpenIdConnectClientInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Client.RedirectUris = model.ParsedRedirectUris;
                    this.repository.Create(model.Client);
                    TempData["Message"] = Resources.OpenIdConnectClientController.ClientCreated;
                    return RedirectToAction("Edit", new { clientId = model.Client.ClientId });
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", Resources.OpenIdConnectClientController.ErrorCreatingClient);
                }
            }

            return Edit(model.Client.ClientId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(OpenIdConnectClientInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Client.RedirectUris = model.ParsedRedirectUris;
                    this.repository.Update(model.Client);
                    TempData["Message"] = Resources.OpenIdConnectClientController.ClientUpdated;
                    return RedirectToAction("Edit", new { clientId = model.Client.ClientId });
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", Resources.OpenIdConnectClientController.ErrorUpdatingClient);
                }
            }

            return Edit(model.Client.ClientId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string clientId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.repository.Delete(clientId);
                    TempData["Message"] = Resources.OpenIdConnectClientController.ClientDeleted;
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", Resources.OpenIdConnectClientController.ErrorDeletingClient);
                }
            }

            return Edit(clientId);
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            var list = new OpenIdConnectClientIndexViewModel(this.repository);
            if (list.Clients.Any())
            {
                var vm = new ChildMenuViewModel
                {
                    Items = list.Clients.Select(x =>
                        new ChildMenuItem
                        {
                            Controller = "OpenIdConnectClient",
                            Action = "Edit",
                            Title = x.Name,
                            RouteValues = new { clientId = x.ClientId }
                        }).ToArray()
                };
                return PartialView("ChildMenu", vm);
            }
            return new EmptyResult();
        }


    }
}
