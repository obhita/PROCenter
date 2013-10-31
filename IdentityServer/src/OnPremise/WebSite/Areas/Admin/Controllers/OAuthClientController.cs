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
    public class OAuthClientController : System.Web.Mvc.Controller
    {
        [Import]
        public IClientsRepository clientRepository { get; set; }

        public OAuthClientController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }
        public OAuthClientController(IClientsRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public ActionResult Index()
        {
            var vm = new OAuthClientIndexViewModel(this.clientRepository);
            return View("Index", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string action, OAuthClientIndexInputModel[] list)
        {
            if (action == "new") return RedirectToAction("Edit");
            if (action == "delete") return Delete(list);

            ModelState.AddModelError("", Resources.OAuthClientController.InvalidAction);
            return Index();
        }

        private ActionResult Delete(OAuthClientIndexInputModel[] list)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var client in list.Where(x => x.Delete))
                    {
                        this.clientRepository.Delete(client.ID);
                    }
                    TempData["Message"] = Resources.OAuthClientController.ClientsDeleted;
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", Resources.OAuthClientController.ErrorDeletingClients);
                }
            }
            
            return Index();
        }

        public ActionResult Edit(int? id)
        {
            Client client = null;
            if (id != null && id > 0)
            {
                client = this.clientRepository.Get(id.Value);
                if (client == null) return HttpNotFound();
            }
            else
            {
                client = new Client();
            }

            var vm = new OAuthClientViewModel(client); 
            return View("Edit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.clientRepository.Create(client);
                    TempData["Message"] = Resources.OAuthClientController.ClientCreated;
                    return RedirectToAction("Edit", new { id = client.ID });
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", Resources.OAuthClientController.ErrorCreatingClient);
                }
            }

            return Edit(client.ID);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Client client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.clientRepository.Update(client);
                    TempData["Message"] = Resources.OAuthClientController.ClientUpdated;
                    return RedirectToAction("Edit", new { id = client.ID });
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", Resources.OAuthClientController.ErrorUpdatingClient);
                }
            }

            return Edit(client.ID);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.clientRepository.Delete(id);
                    TempData["Message"] = Resources.OAuthClientController.ClientDeleted;
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", Resources.OAuthClientController.ErrorDeletingClient);
                }
            }

            return Edit(id);
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            var list = new OAuthClientIndexViewModel(this.clientRepository);
            if (list.Clients.Any())
            {
                var vm = new ChildMenuViewModel
                {
                    Items = list.Clients.Select(x =>
                        new ChildMenuItem
                        {
                            Controller = "OAuthClient",
                            Action = "Edit",
                            Title = x.Name,
                            RouteValues = new { id = x.ID }
                        }).ToArray()
                };
                return PartialView("ChildMenu", vm);
            }
            return new EmptyResult();
        }


    }
}
