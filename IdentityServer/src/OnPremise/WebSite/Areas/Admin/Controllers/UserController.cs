using System;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Thinktecture.IdentityModel.Authorization.Mvc;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.Controllers
{
    [ClaimsAuthorize(Constants.Actions.Administration, Constants.Resources.Configuration)]
    public class UserController : System.Web.Mvc.Controller
    {
        [Import]
        public IUserManagementRepository UserManagementRepository { get; set; }

        private Uri _baseAddress;

        public UserController()
        {
            Container.Current.SatisfyImportsOnce(this);
        }

        public UserController(IUserManagementRepository userManagementRepository)
        {
            UserManagementRepository = userManagementRepository;
        }

        [HttpGet]
        public IdentityServiceResponse Lock(string username)
        {
            _baseAddress = new Uri(Request.Url.Scheme + "://" + Request.Url.Authority + "/");
            var identityServiecResponse = new IdentityServiceResponse();
            var req = WebRequest.Create(_baseAddress + "api/membership/Lock/" + username);
            req.Method = "POST";
            req.ContentLength = 0;
            var response = (HttpWebResponse)req.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //_baseAddress = new Uri(Request.Url.Scheme + "://" + Request.Url.Authority + "/"); 
            //var identityServiecResponse = new IdentityServiceResponse();
            //var response = MakeRequestAsync(HttpMethod.Post, "api/membership/Lock/" + username).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    identityServiecResponse.Sucess = true;
            //}
            //else
            //{
            //    identityServiecResponse.ErrorMessage = response.Content.ReadAsStringAsync().Result;
            //}
            return identityServiecResponse;
        }

        [HttpGet]
        public IdentityServiceResponse Unlock(string username)
        {
            _baseAddress = new Uri(Request.Url.Scheme + "://" + Request.Url.Authority + "/");
            var identityServiecResponse = new IdentityServiceResponse();
            var req = WebRequest.Create(_baseAddress + "api/membership/UnLock/" + username);
            req.Method = "POST";
            var response = (HttpWebResponse) req.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //var response = MakeRequestAsync(HttpMethod.Post, "api/membership/UnLock/" + username).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    identityServiecResponse.Sucess = true;
            //}
            //else
            //{
            //    identityServiecResponse.ErrorMessage = response.Content.ReadAsStringAsync().Result;
            //}
            return identityServiecResponse;
        }

        private async Task<HttpResponseMessage> MakeRequestAsync(HttpMethod method, string url, object data = null)
        {
            using (var httpClient = new HttpClient { BaseAddress = _baseAddress })
            {
                if (method == HttpMethod.Get)
                {
                    return await httpClient.GetAsync(url);
                }
                if (method == HttpMethod.Post)
                {
                    var content = data == null ? string.Empty : JsonConvert.SerializeObject(data);
                    return await httpClient.PostAsync(url, new StringContent(content));
                }
                if (method == HttpMethod.Put)
                {
                    var content = data == null ? string.Empty : JsonConvert.SerializeObject(data);
                    var response = await httpClient.PutAsync(url, new StringContent(content));
                    return response;
                }
                throw new ArgumentException("Invalid method: " + method, "method");
            }
        }

        public ActionResult Index(int page = 1, string filter = null)
        {
            var vm = new UsersViewModel(UserManagementRepository, page, filter);
            return View("Index", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int page, string filter, string action, UserDeleteModel[] list)
        {
            if (action == "new") return Create();
            if (action == "delete") return Delete(page, filter, list);

            ModelState.AddModelError("", Resources.UserController.InvalidAction);
            var vm = new UsersViewModel(UserManagementRepository, page, filter);
            return View("Index", vm);
        }

        public ActionResult Create()
        {
            var rolesvm = new UserRolesViewModel(UserManagementRepository, String.Empty);
            var vm = new UserInputModel();
            vm.Roles = rolesvm.RoleAssignments;
            return View("Create", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.UserManagementRepository.CreateUser(model.Username, model.Password, model.Email);
                    if (model.Roles != null)
                    {
                        var roles = model.Roles.Where(x => x.InRole).Select(x => x.Role);
                        if (roles.Any())
                        {
                            this.UserManagementRepository.SetRolesForUser(model.Username, roles);
                        }
                    }
                    TempData["Message"] = Resources.UserController.UserCreated;
                    return RedirectToAction("Index", new { filter = model.Username });
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", Resources.UserController.ErrorCreatingUser);
                }
            }

            return View("Create", model);
        }

        private ActionResult Delete(int page, string filter, UserDeleteModel[] list)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var name in list.Where(x => x.Delete).Select(x => x.Username))
                    {
                        this.UserManagementRepository.DeleteUser(name);
                    }
                    TempData["Message"] = Resources.UserController.UsersDeleted;
                    return RedirectToAction("Index", new { page, filter });
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", Resources.UserController.ErrorDeletingUser);
                }
            }
            return Index(page, filter);
        }

        public ActionResult Roles(string username)
        {
            var vm = new UserRolesViewModel(this.UserManagementRepository, username);
            return View("Roles", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Roles(string username, UserRoleAssignment[] roleAssignments)
        {
            var vm = new UserRolesViewModel(this.UserManagementRepository, username);
            if (ModelState.IsValid)
            {
                try
                {
                    var currentRoles =
                        roleAssignments.Where(x => x.InRole).Select(x => x.Role);
                    this.UserManagementRepository.SetRolesForUser(username, currentRoles);
                    TempData["Message"] = Resources.UserController.RolesAssignedSuccessfully;
                    return RedirectToAction("Roles", new { username });
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", Resources.UserController.ErrorAssigningRoles);
                }
            }

            return View("Roles", vm);
        }

        public new ActionResult Profile(string username)
        {
            var vm = new UserProfileViewModel(username);
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public new ActionResult Profile(string username, ProfilePropertyInputModel[] profileValues)
        {
            var vm = new UserProfileViewModel(username, profileValues);

            if (vm.UpdateProfileFromValues(ModelState))
            {
                TempData["Message"] = Resources.UserController.ProfileUpdated;
                return RedirectToAction("Profile", new { username });
            }

            return View(vm);
        }

        public ActionResult ChangePassword(string username)
        {
            UserPasswordModel model = new UserPasswordModel();
            model.Username = username;
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(UserPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.UserManagementRepository.SetPassword(model.Username, model.Password);
                    TempData["Message"] = Resources.UserController.ProfileUpdated;
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", "Error updating password");
                }
            }
            
            return View("ChangePassword", model);
        }
    }

    /// <summary>Identity Service Response.</summary>
    public class IdentityServiceResponse
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the error message.
        /// </summary>
        /// <value>
        ///     The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="IdentityServiceResponse" /> is sucess.
        /// </summary>
        /// <value>
        ///     <c>true</c> if sucess; otherwise, <c>false</c>.
        /// </value>
        public bool Sucess { get; set; }

        #endregion
    }
}
