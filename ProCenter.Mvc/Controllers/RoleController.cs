namespace ProCenter.Mvc.Controllers
{
    #region

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Agatha.Common;
    using Common;
    using Dapper;
    using Infrastructure.Security;
    using Models;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using Service.Message.Common;
    using Service.Message.Security;

    #endregion

    public class RoleController : BaseController
    {
        private readonly IProvidePermissions _permissionProvider;

        public RoleController(IRequestDispatcherFactory requestDispatcherFactory, IProvidePermissions permissionProvider) 
            : base(requestDispatcherFactory)
        {
            _permissionProvider = permissionProvider;
        }

        public async Task<PartialViewResult> Create(RoleDto role)
        {
            var requestDistacher = CreateAsyncRequestDispatcher();
            requestDistacher.Add(new CreateRoleRequest
                {
                    OrganizationKey = UserContext.Current.OrganizationKey.Value,
                    Name = role.Name,
                });
            var response = await requestDistacher.GetAsync<CreateRoleResponse>();
            SetupAvailablePermssions(response.Role);
            return PartialView("Edit", response.Role);
        }

        public async Task<PartialViewResult> Edit(Guid key)
        {
            var requestDispacther = CreateAsyncRequestDispatcher();
            requestDispacther.Add(new GetRoleDtoByKeyRequest {Key = key});
            var response = await requestDispacther.GetAsync<DtoResponse<RoleDto>>();

            if (response.DataTransferObject == null)
            {
                throw new HttpException(404, "Role record not found.");
            }
            SetupAvailablePermssions(response.DataTransferObject);
            return PartialView(response.DataTransferObject);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Guid key, string name)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new UpdateRoleRequest {Key = key, Name = name});
            var response = await requestDispatcher.GetAsync<DtoResponse<RoleDto>>();
            return new JsonResult { Data = new { sucess = true } };
        }

        [HttpPost]
        public async Task<ActionResult> AddPermissions(Guid key, string[] permissions)
        {
            if (permissions != null)
            {
                var requestDispacther = CreateAsyncRequestDispatcher();
                requestDispacther.Add(new AssignPermissionRequest {Key = key, Add = true, Permissions = permissions});
                var response = await requestDispacther.GetAsync<AssignPermissionResponse>();
            }
            return new JsonResult
                {
                    Data = new {}
                };
        }

        [HttpPost]
        public async Task<ActionResult> RemovePermissions(Guid key, string[] permissions)
        {
            if (permissions != null)
            {
                var requestDispacther = CreateAsyncRequestDispatcher();
                requestDispacther.Add(new AssignPermissionRequest {Key = key, Add = false, Permissions = permissions});
                var response = await requestDispacther.GetAsync<AssignPermissionResponse>();
            }
            return new JsonResult
                {
                    Data = new {}
                };
        }


        private void SetupAvailablePermssions(RoleDto role)
        {
            if (role == null || role.Permissions == null)
            {
                ViewData["AvailablePermissions"] = 
                    _permissionProvider.Permissions
                    .Select((r => new SelectListItem { Selected = false, Text = r.Name, Value = r.Name }))
                    .OrderBy(s => s.Text)
                    .ToList();
            }
            else
            {
                var availablePermissions = _permissionProvider.Permissions.Where(p => role.Permissions.All(permission => permission != p.Name));
                ViewData["AvailablePermissions"] = 
                    availablePermissions
                    .Select((r => new SelectListItem { Selected = false, Text = r.Name, Value = r.Name }))
                    .OrderBy(s => s.Text)
                    .ToList();
            }
        }
    }
}