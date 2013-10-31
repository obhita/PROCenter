namespace ProCenter.Mvc.Controllers
{
    #region

    using System;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Agatha.Common;
    using Common;
    using Dapper;
    using Infrastructure;
    using Infrastructure.Security;
    using Models;
    using NLog;
    using Primitive;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using Service.Message.Assessment;
    using Service.Message.Common;
    using Service.Message.Patient;
    using Service.Message.Security;

    #endregion

    public class PatientController : BaseController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IResourcesManager _resourcesManager;

        public PatientController(IRequestDispatcherFactory requestDispatcherFactory, IResourcesManager resourcesManager)
            : base(requestDispatcherFactory)
        {
            _resourcesManager = resourcesManager;
        }

        public async Task<ActionResult> Index(Guid? key = null)
        {
            object model = null;
            if (key.HasValue)
            {
                var requestDispatcher = CreateAsyncRequestDispatcher();
                requestDispatcher.Add(new GetPatientDtoByKeyRequest {PatientKey = key.Value});
                var response = await requestDispatcher.GetAsync<GetPatientDtoResponse>();

                if (response.DataTransferObject == null)
                {
                    throw new HttpException(404, "Patient record not found.");
                }
                model = response.DataTransferObject;
                ViewData["Patient"] = response.DataTransferObject;
            }

            return View(model);
        }

        public async Task<PartialViewResult> Create()
        {
            var patientDto = new PatientDto {Name = new PersonName()};
            var requestDispatcher = CreateAsyncRequestDispatcher();
            AddLookupRequests(requestDispatcher, typeof (PatientDto));
            await requestDispatcher.GetAllAsync();
            AddLookupResponsesToViewData(requestDispatcher);

            ViewData["Patient"] = patientDto;

            return PartialView("Create", patientDto);
        }

        public async Task<ActionResult> Edit(Guid key)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new GetPatientDtoByKeyRequest {PatientKey = key});
            AddLookupRequests(requestDispatcher, typeof (PatientDto));
            var response = await requestDispatcher.GetAsync<GetPatientDtoResponse>();
            AddLookupResponsesToViewData(requestDispatcher);

            if (response.DataTransferObject == null)
            {
                throw new HttpException(404, "Patient record not found.");
            }

            ViewData["Patient"] = response.DataTransferObject;

            return View(response.DataTransferObject);
        }


        [HttpPost]
        public async Task<ActionResult> Edit(Guid key, PatientDto patientDto)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new SaveDtoRequest<PatientDto> { DataTransferObject = patientDto });
            var response = await requestDispatcher.GetAsync<SaveDtoResponse<PatientDto>>();

            if (response.DataTransferObject == null)
            {
                throw new HttpException(500, "Patient cannot be saved.");
            }

            if ( response.DataTransferObject.DataErrorInfoCollection.Any () )
            {
                return new JsonResult { Data = new
                    {
                        error = true,
                        errors = response.DataTransferObject.DataErrorInfoCollection
                    } };
            }

            return new JsonResult { Data = new { sucess = true } };
        }

        [HttpPost]
        public async Task<ActionResult> Create(PatientDto patientDto)
        {
            patientDto.OrganizationKey = UserContext.Current.OrganizationKey.Value;
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new CreatePatientRequest {PatientDto = patientDto});
            var response = await requestDispatcher.GetAsync<SaveDtoResponse<PatientDto>>();

            if (response.DataTransferObject == null)
            {
                throw new HttpException(500, "Patient cannot be saved.");
            }

            return RedirectToAction("Edit", new {key = response.DataTransferObject.Key});
        }

        public async Task<ActionResult> PatientFeed(Guid key)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new GetPatientDashboardRequest {PatientKey = key});
            requestDispatcher.Add(new GetPatientDtoByKeyRequest {PatientKey = key});
            var response = await requestDispatcher.GetAsync<GetPatientDashboardResponse>();
            var patientDtoResponse = await requestDispatcher.GetAsync<GetPatientDtoResponse>();

            ViewData["Patient"] = patientDtoResponse.DataTransferObject;
            ViewData["ResourcesManager"] = _resourcesManager;

            return PartialView(response.DashboardItems);
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
            var assignAccountRequest = new AssignAccountRequest
                {
                    OrganizationKey = (Guid) UserContext.Current.OrganizationKey,
                    PatientKey = key,
                    SystemAccountDto = systemAccount,
                    BaseIdentityServerUri = IdentityServerUtil.BaseAddress,
                    Token = JwtTokenContext.Current.Token,
                };

            requestDispacther.Add(assignAccountRequest);
            var response = await requestDispacther.GetAsync<AssignAccountResponse>();
            //SetupAvailableRoles(response.SystemAccountDto);
            if (response.SystemAccountDto.DataErrorInfoCollection.Any())
            {
                var msg = response.SystemAccountDto.DataErrorInfoCollection.FirstOrDefault().Message;
                Logger.Error(msg);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, msg);
            }
            return PartialView("EditorTemplates/SystemAccountDto", response.SystemAccountDto);
        }
    }
}