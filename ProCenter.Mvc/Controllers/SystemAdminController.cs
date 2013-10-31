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
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Agatha.Common;
    using Common;
    using Infrastructure.Security;
    using Models;
    using NLog;
    using Service.Message.Common;
    using Service.Message.Organization;
    using Service.Message.Security;

    public class SystemAdminController : BaseController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public SystemAdminController ( IRequestDispatcherFactory requestDispatcherFactory )
            : base ( requestDispatcherFactory )
        {
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<PartialViewResult> Edit(Guid key)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new GetDtoByKeyRequest<OrganizationDto> { Key = key });
            AddLookupRequests(requestDispatcher, typeof(OrganizationAddressDto));
            AddLookupRequests(requestDispatcher, typeof(OrganizationPhoneDto));
            AddLookupRequests(requestDispatcher, typeof(AddressDto));
            var response = await requestDispatcher.GetAsync<DtoResponse<OrganizationDto>>();
            AddLookupResponsesToViewData(requestDispatcher);

            return PartialView("../Organization/Edit", response.DataTransferObject);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Guid key, string name = null, OrganizationAddressDto organizationAddressDto = null, OrganizationPhoneDto organizationPhoneDto = null)
        {
            if (name != null)
            {
                var requestDispatcher = CreateAsyncRequestDispatcher();
                requestDispatcher.Add(new UpdateOrganizationNameRequest { Key = key, Name = name });
                var response = await requestDispatcher.GetAsync<DtoResponse<OrganizationDto>>();

                //TODO: Handle Errors
                return new JsonResult { Data = new { sucess = true } };
            }
            if (organizationAddressDto.Address != null)
            {
                var result = await Edit(key, organizationAddressDto);
                return result;
            }
            if (organizationPhoneDto.Phone != null)
            {
                var result = await Edit(key, organizationPhoneDto);
                return result;
            }
            return new JsonResult();
        }

        private async Task<ActionResult> Edit(Guid key, OrganizationAddressDto organizationAddressDto)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new AddDtoRequest<OrganizationAddressDto> { AggregateKey = key, DataTransferObject = organizationAddressDto });
            var response = await requestDispatcher.GetAsync<AddDtoResponse<OrganizationAddressDto>>();

            //TODO: Handle Errors
            return new JsonResult
            {
                Data = new
                {
                    originalHash = organizationAddressDto.OriginalHash,
                    newHash = response.DataTransferObject.OriginalHash,
                    newIsPrimary = organizationAddressDto.IsPrimary
                }
            };
        }

        private async Task<ActionResult> Edit(Guid key, OrganizationPhoneDto organizationPhoneDto)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new AddDtoRequest<OrganizationPhoneDto> { AggregateKey = key, DataTransferObject = organizationPhoneDto });
            var response = await requestDispatcher.GetAsync<AddDtoResponse<OrganizationPhoneDto>>();

            //TODO: Handle Errors
            return new JsonResult
            {
                Data = new
                {
                    originalHash = organizationPhoneDto.OriginalHash,
                    newHash = response.DataTransferObject.OriginalHash,
                    newIsPrimary = organizationPhoneDto.IsPrimary
                }
            };
        }

        [HttpPost]
        public async Task<ActionResult> AddAddress(Guid key, OrganizationAddressDto organizationAddressDto)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new AddDtoRequest<OrganizationAddressDto> { AggregateKey = key, DataTransferObject = organizationAddressDto });
            AddLookupRequests(requestDispatcher, typeof(OrganizationAddressDto));
            AddLookupRequests(requestDispatcher, typeof(AddressDto));
            var response = await requestDispatcher.GetAsync<AddDtoResponse<OrganizationAddressDto>>();
            AddLookupResponsesToViewData(requestDispatcher);

            return PartialView("../Organization/EditorTemplates/OrganizationAddressDto", organizationAddressDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddPhone(Guid key, OrganizationPhoneDto organizationPhoneDto)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new AddDtoRequest<OrganizationPhoneDto> { AggregateKey = key, DataTransferObject = organizationPhoneDto });
            AddLookupRequests(requestDispatcher, typeof(OrganizationPhoneDto));
            var response = await requestDispatcher.GetAsync<AddDtoResponse<OrganizationPhoneDto>>();
            AddLookupResponsesToViewData(requestDispatcher);

            return PartialView("../Organization/EditorTemplates/OrganizationPhoneDto", organizationPhoneDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrganizationAdmin (Guid key, string email)
        {
            var requestDispacther = CreateAsyncRequestDispatcher();

            //var federationAuthenticationModule = FederatedAuthentication.WSFederationAuthenticationModule ?? new WSFederationAuthenticationModule();
            requestDispacther.Add(new CreateOrganizationAdminRequest
            {
                OrganizationKey = key,
                Email = email,
                BaseIdentityServerUri = IdentityServerUtil.BaseAddress,
                Token = JwtTokenContext.Current.Token,
            });
            var response = await requestDispacther.GetAsync<CreateOrganizationAdminResponse>();
            
            if (response.SystemAccountDto.DataErrorInfoCollection.Any())
            {
                var msg = response.SystemAccountDto.DataErrorInfoCollection.FirstOrDefault().Message;
                Logger.Error(msg);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, msg);
            }
            return Json ( new {success = true} );
        }
    }
}