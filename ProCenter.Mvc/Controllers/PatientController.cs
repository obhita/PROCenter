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
    using Common;
    using NLog;
    using Primitive;

    using ProCenter.Mvc.Infrastructure;
    using ProCenter.Service.Message.Message;

    using Service.Message.Common;
    using Service.Message.Patient;
    using Service.Message.Security;

    #endregion

    /// <summary>The patient controller class.</summary>
    public class PatientController : BaseController
    {
        #region Static Fields

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger ();

        #endregion

        #region Fields

        private readonly IResourcesManager _resourcesManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientController"/> class.
        /// </summary>
        /// <param name="requestDispatcherFactory">The request dispatcher factory.</param>
        /// <param name="resourcesManager">The resources manager.</param>
        public PatientController ( IRequestDispatcherFactory requestDispatcherFactory, IResourcesManager resourcesManager )
            : base ( requestDispatcherFactory )
        {
            _resourcesManager = resourcesManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>A <see cref="PartialViewResult"/>.</returns>
        public async Task<PartialViewResult> Create ()
        {
            var patientDto = new PatientDto {Name = new PersonName ()};
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            AddLookupRequests ( requestDispatcher, typeof(PatientDto) );
            await requestDispatcher.GetAllAsync ();
            AddLookupResponsesToViewData ( requestDispatcher );

            ViewData["Patient"] = patientDto;

            return PartialView ( "Create", patientDto );
        }

        /// <summary>
        /// Creates the specified patient dto.
        /// </summary>
        /// <param name="patientDto">The patient dto.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        /// <exception cref="System.Web.HttpException">500;Patient cannot be saved.</exception>
        [HttpPost]
        public async Task<ActionResult> Create ( PatientDto patientDto )
        {
            patientDto.OrganizationKey = UserContext.Current.OrganizationKey.Value;
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new CreatePatientRequest {PatientDto = patientDto} );
            var response = await requestDispatcher.GetAsync<SaveDtoResponse<PatientDto>> ();

            if ( response.DataTransferObject == null )
            {
                throw new HttpException ( 500, "Patient cannot be saved." );
            }

            return RedirectToAction ( "Edit", new {key = response.DataTransferObject.Key} );
        }

        /// <summary>
        /// Creates the account.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="systemAccount">The system account.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
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
            var assignAccountRequest = new AssignAccountRequest
            {
                OrganizationKey = (Guid) UserContext.Current.OrganizationKey,
                PatientKey = key,
                SystemAccountDto = systemAccount,
            };

            requestDispacther.Add ( assignAccountRequest );
            var response = await requestDispacther.GetAsync<AssignAccountResponse> ();
            if ( response.SystemAccountDto.DataErrorInfoCollection.Any())
            {
                var msg = response.SystemAccountDto.DataErrorInfoCollection.FirstOrDefault().Message;
                _logger.Error(msg);
                return new JsonResult
                {
                    Data = new
                    {
                        error = true,
                        errors = response.SystemAccountDto.DataErrorInfoCollection
                    }
                };
            }
            return PartialView ( "EditorTemplates/SystemAccountDto", response.SystemAccountDto );
        }

        /// <summary>
        /// Edits the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        /// <exception cref="System.Web.HttpException">404;Patient record not found.</exception>
        public async Task<ActionResult> Edit ( Guid key )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetPatientDtoByKeyRequest {PatientKey = key} );
            AddLookupRequests ( requestDispatcher, typeof(PatientDto) );
            var response = await requestDispatcher.GetAsync<GetPatientDtoResponse> ();
            AddLookupResponsesToViewData ( requestDispatcher );

            if ( response.DataTransferObject == null )
            {
                throw new HttpException ( 404, "Patient record not found." );
            }

            ViewData["Patient"] = response.DataTransferObject;

            return View ( response.DataTransferObject );
        }

        /// <summary>
        /// Edits the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="patientDto">The patient dto.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        /// <exception cref="System.Web.HttpException">500;Patient cannot be saved.</exception>
        [HttpPost]
        public async Task<ActionResult> Edit ( Guid key, PatientDto patientDto )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new SaveDtoRequest<PatientDto> {DataTransferObject = patientDto} );
            var response = await requestDispatcher.GetAsync<SaveDtoResponse<PatientDto>> ();

            if ( response.DataTransferObject == null )
            {
                throw new HttpException ( 500, "Patient cannot be saved." );
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

            return new JsonResult {Data = new {sucess = true}};
        }

        /// <summary>
        /// Indexes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        /// <exception cref="System.Web.HttpException">404;Patient record not found.</exception>
        public async Task<ActionResult> Index ( Guid? key = null )
        {
            object model = null;
            if ( key.HasValue )
            {
                var requestDispatcher = CreateAsyncRequestDispatcher ();
                requestDispatcher.Add ( new GetPatientDtoByKeyRequest {PatientKey = key.Value} );
                var response = await requestDispatcher.GetAsync<GetPatientDtoResponse> ();

                if ( response.DataTransferObject == null )
                {
                    throw new HttpException ( 404, "Patient record not found." );
                }
                model = response.DataTransferObject;
                ViewData["Patient"] = response.DataTransferObject;
            }

            return View ( model );
        }

        /// <summary>
        /// Patients the feed.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        public async Task<ActionResult> PatientFeed ( Guid key )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetPatientDashboardRequest {PatientKey = key} );
            requestDispatcher.Add ( new GetPatientDtoByKeyRequest {PatientKey = key} );
            var response = await requestDispatcher.GetAsync<GetPatientDashboardResponse> ();
            var patientDtoResponse = await requestDispatcher.GetAsync<GetPatientDtoResponse> ();

            ViewData["Patient"] = patientDtoResponse.DataTransferObject;
            ViewData["ResourcesManager"] = _resourcesManager;

            if ( response.DashboardItems != null )
            {
                foreach (var dashBoardItem in response.DashboardItems.Where(dashBoardItem => dashBoardItem.GetType() == typeof(AssessmentReminderDto)))
                {
                    var reminderDto = ((AssessmentReminderDto)dashBoardItem);
                    ((AssessmentReminderDto)dashBoardItem).AssessmentName = _resourcesManager.GetResourceManagerByName(reminderDto.AssessmentName)
                        .GetString(SharedStringNames.ResourceKeyPrefix + reminderDto.AssessmentCode);
                }
            }

            return PartialView ( response.DashboardItems );
        }

        #endregion

        #region Methods

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