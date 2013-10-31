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
namespace ProCenter.Mvc.Controllers
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IdentityModel.Services;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Resources;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Agatha.Common;
    using Common;
    using Dapper;
    using Infrastructure.Security;
    using Models;
    using NLog;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Pillar.Common.Configuration;
    using Primitive;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using Resources;
    using Service.Message.Common;
    using Service.Message.Organization;
    using Service.Message.Security;
    using Thinktecture.IdentityModel.Extensions;

    #endregion

    public class StaffController : BaseController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IDbConnectionFactory _connectionFactory;

        public StaffController(IRequestDispatcherFactory requestDispatcherFactory, IDbConnectionFactory connectionFactory) : base(requestDispatcherFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PartialViewResult> Create(PersonName name)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new CreateStaffRequest
                {
                    OrganizationKey = UserContext.Current.OrganizationKey.Value,
                    Name = name,
                });
            var response = await requestDispatcher.GetAsync<GetStaffDtoResponse>();
            SetupAvailableRoles(response.DataTransferObject.SystemAccount); 
            return PartialView("Edit", response.DataTransferObject);
        }

        public async Task<PartialViewResult> Edit(Guid key)
        {
            var requestDispathcer = CreateAsyncRequestDispatcher();
            requestDispathcer.Add(new GetStaffDtoByKeyRequest {Key = key});
            var response = await requestDispathcer.GetAsync<GetStaffDtoResponse>();

            if (response.DataTransferObject == null)
            {
                throw new HttpException(404, "Staff record not found.");
            }
            SetupAvailableRoles(response.DataTransferObject.SystemAccount);
            return PartialView(response.DataTransferObject);
        }

        private void SetupAvailableRoles(SystemAccountDto systemAccountDto)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var allRoles =
                    connection.Query<RoleDto>("SELECT RoleKey as 'Key', Name FROM SecurityModule.Role WHERE OrganizationKey = @OrganizationKey",
                                              new {OrganizationKey = UserContext.Current.OrganizationKey.ToString()});
                if (systemAccountDto == null || systemAccountDto.Roles == null)
                {
                    ViewData["AvailableRoles"] =
                        allRoles.Select((r => new SelectListItem {Selected = false, Text = r.Name, Value = r.Key.ToString()})).OrderBy(s => s.Text).ToList();
                }
                else
                {
                    var availableRoles = allRoles.Where(r => systemAccountDto.Roles.All(role => role.Key != r.Key));
                    ViewData["AvailableRoles"] =
                        availableRoles.Select((r => new SelectListItem {Selected = false, Text = r.Name, Value = r.Key.ToString()})).OrderBy(s => s.Text).ToList();
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Guid key, PersonName name = null, string email = null, string location = null, string npi = null)
        {
            var updateStaffRequest = new UpdateStaffRequest
                {
                    StaffKey = key,
                };
            if (name != null)
            {
                updateStaffRequest.UpdateType = UpdateStaffRequest.StaffUpdateType.Name;
                updateStaffRequest.Value = name;
            }
            else if (email != null)
            {
                updateStaffRequest.UpdateType = UpdateStaffRequest.StaffUpdateType.Email;
                updateStaffRequest.Value = email;
            }
            else if (location != null)
            {
                updateStaffRequest.UpdateType = UpdateStaffRequest.StaffUpdateType.Location;
                updateStaffRequest.Value = location;
            }
            else if (npi != null)
            {
                updateStaffRequest.UpdateType = UpdateStaffRequest.StaffUpdateType.NPI;
                updateStaffRequest.Value = npi;
            }

            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(updateStaffRequest);
            var response = await requestDispatcher.GetAsync<DtoResponse<StaffDto>>();
            return new JsonResult {Data = new {sucess = true}};
        }

        private string ValidateSystemAccount(SystemAccountDto systemAccount)
        {
            var msgBuilder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(systemAccount.Identifier))
            {
                msgBuilder.Append("Identifier is required. ");
            }
            if (string.IsNullOrWhiteSpace(systemAccount.Email))
            {
                msgBuilder.Append("Email is required.");
            }
            return msgBuilder.ToString();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAccount(Guid key, SystemAccountDto systemAccount)
        {
            systemAccount.Identifier = systemAccount.Email;
            systemAccount.CreateNew = true;
            var validationMsg = ValidateSystemAccount(systemAccount);
            if (validationMsg != string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, validationMsg);
            }

            var requestDispacther = CreateAsyncRequestDispatcher();

            //var federationAuthenticationModule = FederatedAuthentication.WSFederationAuthenticationModule ?? new WSFederationAuthenticationModule();
            requestDispacther.Add(new AssignAccountRequest
                {
                    OrganizationKey = (Guid) UserContext.Current.OrganizationKey,
                    StaffKey = key,
                    SystemAccountDto = systemAccount,
                    BaseIdentityServerUri = IdentityServerUtil.BaseAddress,
                    Token = JwtTokenContext.Current.Token,
                });
            var response = await requestDispacther.GetAsync<AssignAccountResponse>();
            SetupAvailableRoles(response.SystemAccountDto);
            if (response.SystemAccountDto.DataErrorInfoCollection.Any())
            {
                var msg = response.SystemAccountDto.DataErrorInfoCollection.FirstOrDefault().Message;
                Logger.Error(msg);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, msg);
            }
            return PartialView("EditorTemplates/SystemAccountDto", response.SystemAccountDto);
        }

        [HttpPost]
        public async Task<ActionResult> LinkAccount(Guid key, SystemAccountDto systemAccount)
        {
            systemAccount.Identifier = systemAccount.Email;
            systemAccount.CreateNew = false;
            var validationMsg = ValidateSystemAccount(systemAccount);
            if (validationMsg != string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, validationMsg);
            }

            var requestDispacther = CreateAsyncRequestDispatcher();
            requestDispacther.Add(new AssignAccountRequest
                {
                    OrganizationKey = (Guid) UserContext.Current.OrganizationKey,
                    StaffKey = key,
                    SystemAccountDto = systemAccount,
                    Token = JwtTokenContext.Current.Token,
                });
            var response = await requestDispacther.GetAsync<AssignAccountResponse>();

            if (response.SystemAccountDto.DataErrorInfoCollection.Any())
            {
                var msg = response.SystemAccountDto.DataErrorInfoCollection.FirstOrDefault().Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, msg);
            }
            return PartialView("EditorTemplates/SystemAccountDto", response.SystemAccountDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddRoles(Guid key, Guid[] roleKeys)
        {
            if (roleKeys != null)
            {
                var requestDisplacther = CreateAsyncRequestDispatcher();
                requestDisplacther.Add(new AssignRolesRequest
                    {
                        SystemAccoutnKey = key,
                        AddRoles = true,
                        Roles = roleKeys,
                    });
                var response = await requestDisplacther.GetAsync<AssignRolesResponse>();
            }
            return new JsonResult
                {
                    Data = new {}
                };
        }

        [HttpPost]
        public async Task<ActionResult> RemoveRoles(Guid key, Guid[] roleKeys)
        {
            if (roleKeys != null)
            {
                var requestDisplacther = CreateAsyncRequestDispatcher();
                requestDisplacther.Add(new AssignRolesRequest
                    {
                        SystemAccoutnKey = key,
                        AddRoles = false,
                        Roles = roleKeys,
                    });
                var response = await requestDisplacther.GetAsync<AssignRolesResponse>();
            }
            return new JsonResult
                {
                    Data = new {}
                };
        }

        [HttpPost]
        public async Task<ActionResult> ChangPassword(ChangePasswordViewModel changePasswordViewModel)
        {
            var requestDisplacther = CreateAsyncRequestDispatcher();
            requestDisplacther.Add(new ChangePasswordRequest
                {
                    SystemAccountKey = UserContext.Current.SystemAccountKey.Value,
                    BaseBaseIdentityServerUri = IdentityServerUtil.BaseAddress,
                    OldPassword = changePasswordViewModel.OldPassword,
                    NewPassword = changePasswordViewModel.NewPassword,
                    Token = JwtTokenContext.Current.Token,
                });
            var response = await requestDisplacther.GetAsync<ChangePasswordResponse>();

            var resourceManager = new ResourceManager("ProCenter.Mvc.Resources.UserControlResources", typeof(UserControlResources).Assembly);
            var results = new[] {response.ResultCode, resourceManager.GetString(response.ResultCode)};
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        // For testing only
        public string GetUser(string username, string admin, string adminPassword)
        {
            // Note: using JwtTokenContext instead of the following code if not for testing
            var baseAddress = new Uri(IdentityServerUtil.BaseAddress);
            string token;
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.SetBasicAuthentication(admin, adminPassword);

                var response = httpClient.GetAsync("issue/simple?realm=" + baseAddress + "api/&tokenType=jwt").Result;
                response.EnsureSuccessStatusCode();

                var tokenResponse = response.Content.ReadAsStringAsync().Result;
                var json = JObject.Parse(tokenResponse);
                token = json["access_token"].ToString();
                var expiresIn = int.Parse(json["expires_in"].ToString());
                var expiration = DateTime.UtcNow.AddSeconds(expiresIn);
            }
            var membershipUserDto = CallService(baseAddress, token, "api/membership/get/leo.smith");
            return membershipUserDto;
        }

        private static string CallService(Uri baseAddress, string token, string path)
        {
            using (var httpClient = new HttpClient() { BaseAddress = baseAddress })
            {
                httpClient.SetToken("Session", token);

                var response = httpClient.GetAsync(path).Result;
                response.EnsureSuccessStatusCode();
                var result =  response.Content.ReadAsStringAsync().Result;
                return result;
            }
        }

        private static T CallService<T>(Uri baseAddress, string token, string path)
        {
            using (var httpClient = new HttpClient() { BaseAddress = baseAddress })
            {
                httpClient.SetToken("Session", token);

                var response = httpClient.GetAsync(path).Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsAsync<T>().Result;
            }
        }
    }
}