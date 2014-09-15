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

    using ProCenter.Service.Message.Security;

    using Service.Message.Common;
    using Service.Message.Organization;

    #endregion

    /// <summary>The organization controller class.</summary>
    public class OrganizationController : BaseController
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationController"/> class.
        /// </summary>
        /// <param name="requestDispatcherFactory">The request dispatcher factory.</param>
        public OrganizationController ( IRequestDispatcherFactory requestDispatcherFactory )
            : base ( requestDispatcherFactory )
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Activates the assessment.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> ActivateAssessment ( Guid key )
        {
            if ( key != Guid.Empty )
            {
                var requestDispacther = CreateAsyncRequestDispatcher ();
                requestDispacther.Add ( new ActivateDeactivateAssessmentRequest
                {
                    IsActivating = true,
                    OrganizationKey = UserContext.Current.OrganizationKey.Value,
                    AssessmentDefinitionKey = key
                } );
                var response = await requestDispacther.GetAsync<Response> ();
            }

            //TODO: Handle Errors
            return new JsonResult
            {
                Data = new {},
            };
        }

        /// <summary>
        /// Adds the address.
        /// </summary>
        /// <param name="organizationAddressDto">The organization address dto.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> AddAddress ( OrganizationAddressDto organizationAddressDto )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new AddDtoRequest<OrganizationAddressDto>
            {
                AggregateKey = UserContext.Current.OrganizationKey.Value,
                DataTransferObject = organizationAddressDto
            } );
            AddLookupRequests ( requestDispatcher, typeof(OrganizationAddressDto) );
            AddLookupRequests ( requestDispatcher, typeof(AddressDto) );
            var response = await requestDispatcher.GetAsync<AddDtoResponse<OrganizationAddressDto>> ();
            if (response.DataTransferObject.DataErrorInfoCollection.Any())
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
            AddLookupResponsesToViewData ( requestDispatcher );
            return PartialView("EditorTemplates/OrganizationAddressDto", response.DataTransferObject);
        }

        /// <summary>
        /// Removes the address.
        /// </summary>
        /// <param name="addressHash">The address hash.</param>
        /// <returns>Returns an ActionResult.</returns>
        [HttpPost]
        public async Task<ActionResult> RemoveAddress(int addressHash)
        {
            var requestDispacther = CreateAsyncRequestDispatcher();
            requestDispacther.Add(new RemoveOrganizationAddressRequest { OrganizationKey = UserContext.Current.OrganizationKey.Value, OriginalHash = addressHash});
            var response = await requestDispacther.GetAsync<DtoResponse<OrganizationAddressDto>>();
            if (response.DataTransferObject.DataErrorInfoCollection.Any())
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
            return new JsonResult { Data = new { success = true } };
        }

        /// <summary>
        /// Removes the phone.
        /// </summary>
        /// <param name="phoneHash">The phone hash.</param>
        /// <returns>Returns an ActionResult.</returns>
        [HttpPost]
        public async Task<ActionResult> RemovePhone(int phoneHash)
        {
            var requestDispacther = CreateAsyncRequestDispatcher();
            requestDispacther.Add(new RemoveOrganizationPhoneRequest { OrganizationKey = UserContext.Current.OrganizationKey.Value, OriginalHash = phoneHash });
            var response = await requestDispacther.GetAsync<DtoResponse<OrganizationPhoneDto>>();
            if (response.DataTransferObject.DataErrorInfoCollection.Any())
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
            return new JsonResult { Data = new { success = true } };
        }            

        /// <summary>
        /// Adds the phone.
        /// </summary>
        /// <param name="organizationPhoneDto">The organization phone dto.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> AddPhone ( OrganizationPhoneDto organizationPhoneDto )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new AddDtoRequest<OrganizationPhoneDto> {AggregateKey = UserContext.Current.OrganizationKey.Value, DataTransferObject = organizationPhoneDto} );
            AddLookupRequests ( requestDispatcher, typeof(OrganizationPhoneDto) );
            var response = await requestDispatcher.GetAsync<AddDtoResponse<OrganizationPhoneDto>> ();
            AddLookupResponsesToViewData ( requestDispatcher );

            return PartialView("EditorTemplates/OrganizationPhoneDto", response.DataTransferObject);
        }

        /// <summary>
        /// Deactivates the assessment.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="iRow">The i row.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> DeactivateAssessment ( Guid key, int iRow )
        {
            if ( key != Guid.Empty )
            {
                var requestDispacther = CreateAsyncRequestDispatcher ();
                requestDispacther.Add ( new ActivateDeactivateAssessmentRequest
                {
                    IsActivating = false,
                    OrganizationKey = UserContext.Current.OrganizationKey.Value,
                    AssessmentDefinitionKey = key
                } );
                var response = await requestDispacther.GetAsync<Response> ();

                return new JsonResult
                {
                    Data = new {iRow},
                };
            }

            //TODO: Handle Errors
            return new JsonResult
            {
                Data = new {},
            };
        }

        /// <summary>
        /// Edits this instance.
        /// </summary>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        public async Task<ActionResult> Edit ()
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetDtoByKeyRequest<OrganizationDto> {Key = UserContext.Current.OrganizationKey.Value} );
            AddLookupRequests ( requestDispatcher, typeof(OrganizationAddressDto) );
            AddLookupRequests ( requestDispatcher, typeof(OrganizationPhoneDto) );
            AddLookupRequests ( requestDispatcher, typeof(AddressDto) );
            var response = await requestDispatcher.GetAsync<DtoResponse<OrganizationDto>> ();
            AddLookupResponsesToViewData ( requestDispatcher );

            return View ( response.DataTransferObject );
        }

        /// <summary>
        /// Edits the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="organizationAddressDto">The organization address dto.</param>
        /// <param name="organizationPhoneDto">The organization phone dto.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> Edit ( string name = null, OrganizationAddressDto organizationAddressDto = null, OrganizationPhoneDto organizationPhoneDto = null )
        {
            var key = UserContext.Current.OrganizationKey.Value;
            if ( name != null )
            {
                var requestDispatcher = CreateAsyncRequestDispatcher ();
                requestDispatcher.Add ( new UpdateOrganizationNameRequest {Key = key, Name = name} );
                var response = await requestDispatcher.GetAsync<DtoResponse<OrganizationDto>> ();

                //TODO: Handle Errors
                return new JsonResult {Data = new {sucess = true}};
            }
            if ( organizationAddressDto.Address != null )
            {
                var result = await Edit ( organizationAddressDto );
                return result;
            }
            if ( organizationPhoneDto.Phone != null )
            {
                var result = await Edit ( organizationPhoneDto );
                return result;
            }
            return new JsonResult ();
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        public async Task<ActionResult> Index ()
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetDtoByKeyRequest<OrganizationSummaryDto> {Key = UserContext.Current.OrganizationKey.Value} );
            var response = await requestDispatcher.GetAsync<DtoResponse<OrganizationSummaryDto>> ();

            return View ( response.DataTransferObject );
        }

        #endregion

        #region Methods

        private async Task<ActionResult> Edit ( OrganizationAddressDto organizationAddressDto )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new AddDtoRequest<OrganizationAddressDto>
            {
                AggregateKey = UserContext.Current.OrganizationKey.Value,
                DataTransferObject = organizationAddressDto
            } );
            var response = await requestDispatcher.GetAsync<AddDtoResponse<OrganizationAddressDto>> ();

            //TODO: Handle Errors
            if (response.DataTransferObject == null)
            {
                return new JsonResult
                {
                    Data = new
                    {
                        error = true,
                        errors = "DTO is null."
                    }
                };
            }

            if (response.DataTransferObject != null && response.DataTransferObject.DataErrorInfoCollection.Any())
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

        private async Task<ActionResult> Edit ( OrganizationPhoneDto organizationPhoneDto )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new AddDtoRequest<OrganizationPhoneDto> {AggregateKey = UserContext.Current.OrganizationKey.Value, DataTransferObject = organizationPhoneDto} );
            var response = await requestDispatcher.GetAsync<AddDtoResponse<OrganizationPhoneDto>> ();

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

        #endregion
    }
}