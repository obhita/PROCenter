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

namespace ProCenter.Mvc.Controllers
{
    #region Using Statements

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Agatha.Common;
    using Common;
    using Infrastructure.Security;
    using Service.Message.Common;
    using Service.Message.Security;

    #endregion

    /// <summary>The role controller class.</summary>
    public class RoleController : BaseController
    {
        #region Fields

        private readonly IProvidePermissions _permissionProvider;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// </summary>
        /// <param name="requestDispatcherFactory">The request dispatcher factory.</param>
        /// <param name="permissionProvider">The permission provider.</param>
        public RoleController ( IRequestDispatcherFactory requestDispatcherFactory, IProvidePermissions permissionProvider )
            : base ( requestDispatcherFactory )
        {
            _permissionProvider = permissionProvider;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the permissions.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="permissions">The permissions.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> AddPermissions ( Guid key, string[] permissions )
        {
            if ( permissions != null )
            {
                var requestDispacther = CreateAsyncRequestDispatcher ();
                requestDispacther.Add ( new AssignPermissionRequest {Key = key, Add = true, Permissions = permissions} );
                var response = await requestDispacther.GetAsync<AssignPermissionResponse> ();
            }
            return new JsonResult
            {
                Data = new {}
            };
        }

        /// <summary>
        /// Creates the specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        public async Task<PartialViewResult> Create ( RoleDto role )
        {
            var requestDistacher = CreateAsyncRequestDispatcher ();
            requestDistacher.Add ( new CreateRoleRequest
            {
                OrganizationKey = UserContext.Current.OrganizationKey.Value,
                Name = role.Name,
            } );
            var response = await requestDistacher.GetAsync<CreateRoleResponse> ();
            SetupAvailablePermssions ( response.Role );
            return PartialView ( "Edit", response.Role );
        }

        /// <summary>
        /// Edits the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="PartialViewResult"/>.</returns>
        /// <exception cref="System.Web.HttpException">404;Role record not found.</exception>
        public async Task<PartialViewResult> Edit ( Guid key )
        {
            var requestDispacther = CreateAsyncRequestDispatcher ();
            requestDispacther.Add ( new GetRoleDtoByKeyRequest {Key = key} );
            var response = await requestDispacther.GetAsync<DtoResponse<RoleDto>> ();

            if ( response.DataTransferObject == null )
            {
                throw new HttpException ( 404, "Role record not found." );
            }
            SetupAvailablePermssions ( response.DataTransferObject );
            return PartialView ( response.DataTransferObject );
        }

        /// <summary>
        /// Edits the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> Edit ( Guid key, string name )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new UpdateRoleRequest {Key = key, Name = name} );
            var response = await requestDispatcher.GetAsync<DtoResponse<RoleDto>> ();
            return new JsonResult {Data = new {sucess = true}};
        }

        /// <summary>
        /// Removes the permissions.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="permissions">The permissions.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> RemovePermissions ( Guid key, string[] permissions )
        {
            if ( permissions != null )
            {
                var requestDispacther = CreateAsyncRequestDispatcher ();
                requestDispacther.Add ( new AssignPermissionRequest {Key = key, Add = false, Permissions = permissions} );
                var response = await requestDispacther.GetAsync<AssignPermissionResponse> ();
            }
            return new JsonResult
            {
                Data = new {}
            };
        }

        #endregion

        #region Methods

        private void SetupAvailablePermssions ( RoleDto role )
        {
            if ( role == null || role.Permissions == null )
            {
                ViewData["AvailablePermissions"] =
                    _permissionProvider.Permissions
                        .Select ( ( r => new SelectListItem {Selected = false, Text = r.Name, Value = r.Name} ) )
                        .OrderBy ( s => s.Text )
                        .ToList ();
            }
            else
            {
                var availablePermissions = _permissionProvider.Permissions.Where ( p => role.Permissions.All ( permission => permission != p.Name ) );
                ViewData["AvailablePermissions"] =
                    availablePermissions
                        .Select ( ( r => new SelectListItem {Selected = false, Text = r.Name, Value = r.Name} ) )
                        .OrderBy ( s => s.Text )
                        .ToList ();
            }
        }

        #endregion
    }
}