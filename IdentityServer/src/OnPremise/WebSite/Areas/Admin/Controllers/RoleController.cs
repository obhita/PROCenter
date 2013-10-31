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
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Authorization.Mvc;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.Controllers
{
    [ClaimsAuthorize(Constants.Actions.Administration, Constants.Resources.Configuration)]
    public class RoleController : System.Web.Mvc.Controller
    {
        [Import]
        public IUserManagementRepository UserManagementRepository { get; set; }

        public RoleController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public RoleController(IUserManagementRepository userManagementRepository)
        {
            UserManagementRepository = userManagementRepository;
        }

        public ActionResult Index()
        {
            var vm = new RolesViewModel(UserManagementRepository);
            return View("Index", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string action, RoleInputModel[] list)
        {
            if (action == "new") return Create();
            if (action == "delete") return Delete(list);

            ModelState.AddModelError("", Resources.RoleController.InvalidAction);
            var vm = new RolesViewModel(UserManagementRepository);
            return View("Index", vm);
        }

        public ActionResult Create()
        {
            return View("Create", new RoleInputModel());
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserManagementRepository.CreateRole(model.Name);
                    TempData["Message"] = Resources.RoleController.RoleCreated;
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", Resources.RoleController.ErrorCreatingRole);
                }
            }

            return View("Create", model);
        }

        private ActionResult Delete(RoleInputModel[] list)
        {
            var query = from item in list
                        where item.Delete && !(item.CanDelete)
                        select item.Name;
            foreach(var name in query)
            {
                ModelState.AddModelError("", string.Format(Resources.RoleController.CannotDeleteRole, name));
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in list.Where(x=>x.Delete && x.CanDelete).Select(x=>x.Name))
                    {
                        UserManagementRepository.DeleteRole(item);
                    }
                    TempData["Message"] = Resources.RoleController.RolesDeleted;
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", Resources.RoleController.ErrorDeletingRole);
                }
            }
            
            var vm = new RolesViewModel(UserManagementRepository);
            return View("Index", vm);
        }
    }
}
