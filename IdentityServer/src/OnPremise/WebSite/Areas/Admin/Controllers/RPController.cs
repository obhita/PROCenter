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
using System.Collections.Generic;
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
    public class RPController : System.Web.Mvc.Controller
    {
        [Import]
        public IRelyingPartyRepository RelyingPartyRepository { get; set; }

        public RPController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public RPController(IRelyingPartyRepository relyingPartyRepository)
        {
            this.RelyingPartyRepository = relyingPartyRepository;
        }

        public ActionResult Index()
        {
            var vm = new RelyingPartiesViewModel(RelyingPartyRepository);
            return View("Index", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string action, IEnumerable<RelyingPartyViewModel> list)
        {
            if (action == "save")
            {
                var vm = new RelyingPartiesViewModel(RelyingPartyRepository);
                if (ModelState.IsValid)
                {
                    vm.Update(list);
                    TempData["Message"] = Resources.RPController.UpdateSuccessful;
                    return RedirectToAction("Index");
                }

                return View("Index", vm);
            }

            if (action == "new")
            {
                return RedirectToAction("RP");
            }

            ModelState.AddModelError("", Resources.RPController.InvalidAction);
            return View("Index", new RelyingPartiesViewModel(RelyingPartyRepository));
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            var rpvm = new RelyingPartiesViewModel(RelyingPartyRepository);
            var list = rpvm.RPs.Where(x => x.Enabled);
            if (list.Any())
            {
                var vm = new ChildMenuViewModel
                {
                    Items = list.Select(x=>
                        new ChildMenuItem
                        {
                            Controller = "RP",
                            Action = "RP",
                            Title = x.DisplayName,
                            RouteValues = new{id=x.ID}
                        }).ToArray()
                };
                return PartialView("ChildMenu", vm);
            }
            return new EmptyResult();
        }

        public ActionResult RP(string id)
        {
            RelyingParty rp = null;
            if (id == null) rp = new RelyingParty();
            else rp = this.RelyingPartyRepository.Get(id);
            if (rp == null) return HttpNotFound();
            return View("RP", rp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RP(string id,
            string action,
            [Bind(Exclude = "EncryptingCertificate")] RelyingParty rp,
            RPCertInputModel cert)
        {
            if (action == "create")
            {
                return CreateRP(rp, cert);
            }
            if (action == "save")
            {
                return SaveRP(id, rp, cert);
            }
            if (action == "delete")
            {
                return DeleteRP(id);
            }

            var origRP = this.RelyingPartyRepository.Get(id);
            rp.EncryptingCertificate = origRP.EncryptingCertificate;

            ModelState.AddModelError("", Resources.RPController.InvalidAction);
            return View("RP", rp);
        }

        private ActionResult DeleteRP(string id)
        {
            try
            {
                this.RelyingPartyRepository.Delete(id);
                TempData["Message"] = Resources.RPController.DeleteSuccessful;
                return RedirectToAction("Index");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch
            {
                ModelState.AddModelError("", Resources.RPController.ErrorDeletingRelyingParty);

            }

            var rp = this.RelyingPartyRepository.Get(id);
            return View("RP", rp);
        }

        private ActionResult CreateRP(RelyingParty rp, RPCertInputModel cert)
        {
            // ID is not required for create
            ModelState["ID"].Errors.Clear();

            rp.Id = null;
            rp.EncryptingCertificate = cert.Cert;

            if (ModelState.IsValid)
            {
                try
                {
                    this.RelyingPartyRepository.Add(rp);
                    TempData["Message"] = Resources.RPController.CreateSuccessful;
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", Resources.RPController.ErrorCreatingRelyingParty);
                }
            }

            return View("RP", rp);
        }

        private ActionResult SaveRP(string id, RelyingParty rp, RPCertInputModel cert)
        {
            if (cert.RemoveCert == true)
            {
                rp.EncryptingCertificate = null;
            }
            else if (cert.Cert != null)
            {
                rp.EncryptingCertificate = cert.Cert;
            }
            else
            {
                var origRP = this.RelyingPartyRepository.Get(id);
                rp.EncryptingCertificate = origRP.EncryptingCertificate;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this.RelyingPartyRepository.Update(rp);
                    TempData["Message"] = Resources.RPController.UpdateSuccessful;
                    return RedirectToAction("RP", new { id });
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", Resources.RPController.ErrorUpdatingRelyingParty);
                }
            }

            return View("RP", rp);
        }
    }
}


