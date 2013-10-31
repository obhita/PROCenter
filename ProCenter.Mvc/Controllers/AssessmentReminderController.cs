namespace ProCenter.Mvc.Controllers
{
    #region

    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Agatha.Common;
    using Common;
    using Models;
    using Service.Message.Common;
    using Service.Message.Message;

    #endregion

    public class AssessmentReminderController : BaseController
    {
        public AssessmentReminderController(IRequestDispatcherFactory requestDispatcherFactory) : base(requestDispatcherFactory)
        {
        }

        [HttpPost]
        public async Task<ActionResult> Create(AssessmentReminderDto assessmentReminderDto)
        {
            assessmentReminderDto.OrganizationKey = UserContext.Current.OrganizationKey.Value;
            assessmentReminderDto.CreatedByStaffKey = UserContext.Current.StaffKey.Value;
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new AddDtoRequest<AssessmentReminderDto>
                {
                    DataTransferObject = assessmentReminderDto,
                });

            var response = await requestDispatcher.GetAsync<DtoResponse<AssessmentReminderDto>>();

            var dto = response.DataTransferObject;
            return new JsonResult
                {
                    Data = new { success = true }
                };
        }

        [HttpPost]
        public async Task<ActionResult> UpdateDate(Guid key, string dayDelta)
        {
            int day;
            if (int.TryParse(dayDelta, out day) && day != 0)
            {
                var requestDispatcher = CreateAsyncRequestDispatcher();
                requestDispatcher.Add(new UpdateAssessmentReminderRequest
                    {
                        AssessmentReminderKey = key,
                        DayDelta = day,
                    });

                var response = await requestDispatcher.GetAsync<DtoResponse<AssessmentReminderDto>>();
                var dto = response.DataTransferObject;
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AssessmentReminderDto assessmentReminder)
        {
            assessmentReminder.OrganizationKey = UserContext.Current.OrganizationKey.Value;
            assessmentReminder.CreatedByStaffKey = UserContext.Current.StaffKey.Value;
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new UpdateAssessmentReminderRequest
                {
                    AssessmentReminderDto = assessmentReminder,
                });

            var response = await requestDispatcher.GetAsync<DtoResponse<AssessmentReminderDto>>();

            var dto = response.DataTransferObject;
            return new JsonResult
                {
                    Data = new { success = true }
                };
        }

        public async Task<ActionResult> Get(Guid key)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new GetAssessmentReminderByKeyRequest {AssessmentReminderKey = key});
            var response = await requestDispatcher.GetAsync<DtoResponse<AssessmentReminderDto>>();

            var dto = response.DataTransferObject;
            ViewData.TemplateInfo.HtmlFieldPrefix = "assessmentReminder"; //note: matches the Edit action parameter name
            if ( !dto.ForSelfAdministration && UserContext.Current.PatientKey != null )
            {
                dto = new AssessmentReminderDto ();
                ModelState.AddModelError ( "error", "You do not have access to this reminder." );
            }
            return PartialView("~/Views/Shared/EditorTemplates/AssessmentReminderDto.cshtml", dto);
        }

        [HttpPost]
        public async Task<ActionResult> Cancel(Guid key)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new CancelAssessmentReminderRequest {AssessmentReminderKey = key});

            var response = await requestDispatcher.GetAsync<DtoResponse<AssessmentReminderDto>>();

            var dto = response.DataTransferObject;
            return new JsonResult
                {
                    Data = new { success = true }
                };
        }

        public async Task<ActionResult> AdministerAssessment(Guid key, Guid patientKey, Guid assessmentDefinitionKey)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new AcknowledgeAssessmentReminderRequest { Key = key });
            var response = await requestDispatcher.GetAsync<DtoResponse<AssessmentReminderDto>>();
            //TODO:check for errors

            return RedirectToAction("Create", "Assessment", new { patientKey, assessmentDefinitionKey });
        }

        [HttpPost]
        public async Task<ActionResult> Achknowledge(Guid key, Guid assessmentKey, Guid patientKey)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new AcknowledgeAssessmentReminderRequest { Key = key });
            var response = await requestDispatcher.GetAsync<DtoResponse<AssessmentReminderDto>>();
            //TODO:check for errors

            return new JsonResult
            {
                Data = new { key }
            };
        }
    }
}