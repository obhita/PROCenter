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
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    using Agatha.Common;

    using Dapper;

    using NLog;

    using ProCenter.Common;
    using ProCenter.Domain.OrganizationModule;
    using ProCenter.Infrastructure.Domain.Repositories;
    using ProCenter.Primitive;
    using ProCenter.Service.Message.Common;
    using ProCenter.Service.Message.Organization;
    using ProCenter.Service.Message.Security;

    #endregion

    /// <summary>The staff controller class.</summary>
    public class StaffController : BaseController
    {
        #region Static Fields

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger ();

        #endregion

        #region Fields

        private readonly IDbConnectionFactory _connectionFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="StaffController" /> class.
        /// </summary>
        /// <param name="requestDispatcherFactory">The request dispatcher factory.</param>
        /// <param name="connectionFactory">The connection factory.</param>
        public StaffController ( IRequestDispatcherFactory requestDispatcherFactory, IDbConnectionFactory connectionFactory )
            : base ( requestDispatcherFactory )
        {
            _connectionFactory = connectionFactory;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the roles.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="roleKeys">The role keys.</param>
        /// <returns>A <see cref="ActionResult" />.</returns>
        [HttpPost]
        public async Task<ActionResult> AddRoles ( Guid key, Guid[] roleKeys )
        {
            if ( roleKeys != null )
            {
                var requestDisplacther = CreateAsyncRequestDispatcher ();
                requestDisplacther.Add (
                    new AssignRolesRequest
                    {
                        SystemAccoutnKey = key,
                        AddRoles = true,
                        Roles = roleKeys,
                    } );
                var response = await requestDisplacther.GetAsync<AssignRolesResponse> ();
            }
            return new JsonResult
                   {
                       Data = new { }
                   };
        }

        /// <summary>
        ///     Creates the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A <see cref="PartialViewResult" />.</returns>
        public async Task<PartialViewResult> Create ( PersonName name )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add (
                new CreateStaffRequest
                {
                    OrganizationKey = UserContext.Current.OrganizationKey.Value,
                    Name = name,
                } );
            var response = await requestDispatcher.GetAsync<GetStaffDtoResponse> ();
            SetupAvailableRoles ( response.DataTransferObject.SystemAccount );
            return PartialView ( "Edit", response.DataTransferObject );
        }

        /// <summary>
        ///     Creates the account.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="systemAccount">The system account.</param>
        /// <returns>A <see cref="ActionResult" />.</returns>
        [HttpPost]
        public async Task<ActionResult> CreateAccount ( Guid key, SystemAccountDto systemAccount )
        {
            systemAccount.Identifier = systemAccount.Email;
            systemAccount.CreateNew = true;
            var validationMsg = ValidateSystemAccount ( systemAccount );
            if ( validationMsg != string.Empty )
            {
                return new HttpStatusCodeResult ( HttpStatusCode.InternalServerError, validationMsg );
            }

            var requestDispacther = CreateAsyncRequestDispatcher ();

            requestDispacther.Add (
                new AssignAccountRequest
                {
                    OrganizationKey = (Guid)UserContext.Current.OrganizationKey,
                    StaffKey = key,
                    SystemAccountDto = systemAccount
                } );
            var response = await requestDispacther.GetAsync<AssignAccountResponse>();
            SetupAvailableRoles ( response.SystemAccountDto );
            if ( response.SystemAccountDto.DataErrorInfoCollection.Any () )
            {
                var msg = response.SystemAccountDto.DataErrorInfoCollection.FirstOrDefault ().Message;
                _logger.Error ( msg );
                return new HttpStatusCodeResult ( HttpStatusCode.InternalServerError, msg );
            }
            return PartialView ( "EditorTemplates/SystemAccountDto", response.SystemAccountDto );
        }

        /// <summary>
        ///     Edits the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="ActionResult" />.</returns>
        /// <exception cref="System.Web.HttpException">404;Staff record not found.</exception>
        public async Task<PartialViewResult> Edit ( Guid key )
        {
            var requestDispathcer = CreateAsyncRequestDispatcher ();
            requestDispathcer.Add ( new GetStaffDtoByKeyRequest { Key = key } );
            var response = await requestDispathcer.GetAsync<GetStaffDtoResponse> ();

            if ( response.DataTransferObject == null )
            {
                throw new HttpException ( 404, "Staff record not found." );
            }
            SetupAvailableRoles ( response.DataTransferObject.SystemAccount );
            return PartialView ( response.DataTransferObject );
        }

        /// <summary>
        ///     Edits the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <param name="email">The email.</param>
        /// <param name="location">The location.</param>
        /// <param name="npi">The npi.</param>
        /// <returns>A <see cref="ActionResult" />.</returns>
        [HttpPost]
        public async Task<ActionResult> Edit ( Guid key, PersonName name = null, string email = null, string location = null, string npi = null )
        {
            var updateStaffRequest = new UpdateStaffRequest
                                     {
                                         StaffKey = key,
                                     };
            if ( name != null )
            {
                updateStaffRequest.UpdateType = UpdateStaffRequest.StaffUpdateType.Name;
                updateStaffRequest.Value = name;
            }
            else if ( email != null )
            {
                updateStaffRequest.UpdateType = UpdateStaffRequest.StaffUpdateType.Email;
                updateStaffRequest.Value = email;
            }
            else if ( location != null )
            {
                updateStaffRequest.UpdateType = UpdateStaffRequest.StaffUpdateType.Location;
                updateStaffRequest.Value = location;
            }
            else if ( npi != null )
            {
                updateStaffRequest.UpdateType = UpdateStaffRequest.StaffUpdateType.NPI;
                updateStaffRequest.Value = npi;
            }

            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( updateStaffRequest );
            var response = await requestDispatcher.GetAsync<DtoResponse<StaffDto>> ();

            if ( response.DataTransferObject == null )
            {
                throw new HttpException ( 500, "Staff cannot be saved." );
            }

            if ( response.DataTransferObject.DataErrorInfoCollection.Any () )
            {
                return new JsonResult
                       {
                           Data = new
                                  {
                                      error = true,
                                      errors = response.DataTransferObject.DataErrorInfoCollection
                                  }
                       };
            }
            return new JsonResult { Data = new { sucess = true } };
        }

        /// <summary>
        ///     Links the account.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="systemAccount">The system account.</param>
        /// <returns>A <see cref="ActionResult" />.</returns>
        [HttpPost]
        public async Task<ActionResult> LinkAccount ( Guid key, SystemAccountDto systemAccount )
        {
            systemAccount.Identifier = systemAccount.Email;
            systemAccount.CreateNew = false;
            var validationMsg = ValidateSystemAccount ( systemAccount );
            if ( validationMsg != string.Empty )
            {
                return new HttpStatusCodeResult ( HttpStatusCode.InternalServerError, validationMsg );
            }

            var requestDispacther = CreateAsyncRequestDispatcher ();
            requestDispacther.Add (
                new AssignAccountRequest
                {
                    OrganizationKey = (Guid)UserContext.Current.OrganizationKey,
                    StaffKey = key,
                    SystemAccountDto = systemAccount,
                } );
            var response = await requestDispacther.GetAsync<AssignAccountResponse> ();

            if ( response.SystemAccountDto.DataErrorInfoCollection.Any () )
            {
                var msg = response.SystemAccountDto.DataErrorInfoCollection.FirstOrDefault ().Message;
                return new HttpStatusCodeResult ( HttpStatusCode.InternalServerError, msg );
            }
            return PartialView ( "EditorTemplates/SystemAccountDto", response.SystemAccountDto );
        }

        /// <summary>
        ///     Removes the roles.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="roleKeys">The role keys.</param>
        /// <returns>A <see cref="ActionResult" />.</returns>
        [HttpPost]
        public async Task<ActionResult> RemoveRoles ( Guid key, Guid[] roleKeys )
        {
            if ( roleKeys != null )
            {
                var requestDisplacther = CreateAsyncRequestDispatcher ();
                requestDisplacther.Add (
                    new AssignRolesRequest
                    {
                        SystemAccoutnKey = key,
                        AddRoles = false,
                        Roles = roleKeys,
                    } );
                var response = await requestDisplacther.GetAsync<AssignRolesResponse> ();
            }
            return new JsonResult
                   {
                       Data = new { }
                   };
        }

        #endregion

        #region Methods

        private void SetupAvailableRoles ( SystemAccountDto systemAccountDto )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                var allRoles =
                    connection.Query<RoleDto> (
                        "SELECT RoleKey as 'Key', Name FROM SecurityModule.Role WHERE OrganizationKey = @OrganizationKey",
                        new { OrganizationKey = UserContext.Current.OrganizationKey.ToString () } );
                if ( systemAccountDto == null || systemAccountDto.Roles == null )
                {
                    ViewData["AvailableRoles"] =
                        allRoles.Select ( ( r => new SelectListItem { Selected = false, Text = r.Name, Value = r.Key.ToString () } ) ).OrderBy ( s => s.Text ).ToList ();
                }
                else
                {
                    var availableRoles = allRoles.Where ( r => systemAccountDto.Roles.All ( role => role.Key != r.Key ) );
                    ViewData["AvailableRoles"] =
                        availableRoles.Select ( ( r => new SelectListItem { Selected = false, Text = r.Name, Value = r.Key.ToString () } ) ).OrderBy ( s => s.Text ).ToList ();
                }
            }
        }

        private string ValidateSystemAccount ( SystemAccountDto systemAccount )
        {
            var msgBuilder = new StringBuilder ();
            if ( string.IsNullOrWhiteSpace ( systemAccount.Identifier ) )
            {
                msgBuilder.Append ( "Identifier is required. " );
            }
            if ( string.IsNullOrWhiteSpace ( systemAccount.Email ) )
            {
                msgBuilder.Append ( "Email is required." );
            }
            return msgBuilder.ToString ();
        }

        #endregion
    }
}