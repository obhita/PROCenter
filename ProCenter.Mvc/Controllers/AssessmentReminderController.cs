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

    using Pillar.Agatha.Message;

    using Service.Message.Common;
    using Service.Message.Message;

    #endregion

    /// <summary>The assessment reminder controller class.</summary>
    public class AssessmentReminderController : BaseController
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentReminderController"/> class.
        /// </summary>
        /// <param name="requestDispatcherFactory">The request dispatcher factory.</param>
        public AssessmentReminderController ( IRequestDispatcherFactory requestDispatcherFactory )
            : base ( requestDispatcherFactory )
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Achknowledges the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="assessmentKey">The assessment key.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="recurrenceKey">The recurrence key.</param>
        /// <returns>
        /// A <see cref="ActionResult" />.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> Acknowledge ( Guid key, Guid assessmentKey, Guid patientKey, Guid recurrenceKey )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new AcknowledgeAssessmentReminderRequest {Key = key, RecurrenceKey = recurrenceKey} );
            var response = await requestDispatcher.GetAsync<DtoResponse<AssessmentReminderDto>> ();

            //TODO:check for errors

            return new JsonResult
            {
                Data = new {key}
            };
        }

        /// <summary>
        /// Administers the assessment.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="assessmentDefinitionKey">The assessment definition key.</param>
        /// <param name="recurrenceKey">The recurrence key.</param>
        /// <returns>
        /// A <see cref="ActionResult" />.
        /// </returns>
        public async Task<ActionResult> AdministerAssessment(Guid key, Guid patientKey, Guid assessmentDefinitionKey, Guid recurrenceKey)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new AcknowledgeAssessmentReminderRequest { Key = recurrenceKey });
            var response = await requestDispatcher.GetAsync<DtoResponse<AssessmentReminderDto>>();

            ////TODO:check for errors
            return RedirectToAction("Create", "Assessment", new { patientKey, assessmentDefinitionKey, assessmentReminderKey = key, recurrenceKey });
        }

        /// <summary>
        /// Cancels the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="recurrenceKey">The recurrence key.</param>
        /// <returns>
        /// A <see cref="ActionResult" />.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> Cancel(Guid key, Guid recurrenceKey)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add(new CancelAssessmentReminderRequest { AssessmentReminderKey = key, RecurrenceKey = recurrenceKey});

            var response = await requestDispatcher.GetAsync<DtoResponse<AssessmentReminderDto>> ();

            var dto = response.DataTransferObject;
            return new JsonResult
            {
                Data = new {success = true}
            };
        }

        /// <summary>
        /// Creates the specified assessment reminder dto.
        /// </summary>
        /// <param name="assessmentReminderDto">The assessment reminder dto.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> Create ( AssessmentReminderDto assessmentReminderDto )
        {
            assessmentReminderDto.OrganizationKey = UserContext.Current.OrganizationKey.Value;
            assessmentReminderDto.CreatedByStaffKey = UserContext.Current.StaffKey.Value;
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new AddDtoRequest<AssessmentReminderDto>
            {
                DataTransferObject = assessmentReminderDto,
            });

            var response = await requestDispatcher.GetAsync<DtoResponse<AssessmentReminderDto>> ();
            if (response.DataTransferObject == null)
            {
                throw new HttpException(500, "Assessment Reminder cannot be saved.");
            }

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

            return new JsonResult { Data = new { sucess = true } };
        }

        /// <summary>
        /// Edits the specified assessment reminder.
        /// </summary>
        /// <param name="assessmentReminder">The assessment reminder.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> Edit ( AssessmentReminderDto assessmentReminder )
        {
            assessmentReminder.OrganizationKey = UserContext.Current.OrganizationKey.Value;
            assessmentReminder.CreatedByStaffKey = UserContext.Current.StaffKey.Value;
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new UpdateAssessmentReminderRequest
            {
                AssessmentReminderDto = assessmentReminder,
            });

            var response = await requestDispatcher.GetAsync<DtoResponse<AssessmentReminderDto>>();

            if (response.DataTransferObject == null)
            {
                throw new HttpException(500, "Assessment Reminder cannot be saved.");
            }
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
            return new JsonResult { Data = new { sucess = true } };
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// A <see cref="ActionResult" />.
        /// </returns>
        public async Task<ActionResult> Get ( Guid key )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetAssessmentReminderByKeyRequest {AssessmentReminderKey = key} );
            var response = await requestDispatcher.GetAsync<DtoResponse<AssessmentReminderDto>> ();

            var dto = response.DataTransferObject;
            if ( dto == null )
            {
                return null;
            }
            ViewData.TemplateInfo.HtmlFieldPrefix = "assessmentReminder"; //note: matches the Edit action parameter name
            if (UserContext.Current != null && !dto.ForSelfAdministration && UserContext.Current.PatientKey != null) 
            {
                dto = new AssessmentReminderDto();
                ModelState.AddModelError("error", "You do not have access to this reminder.");
            }
            return PartialView ( "~/Views/Shared/EditorTemplates/AssessmentReminderDto.cshtml", dto );
        }

        /// <summary>
        /// Updates the date.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="dayDelta">The day delta.</param>
        /// <returns>A <see cref="ActionResult"/>.</returns>
        [HttpPost]
        public async Task<ActionResult> UpdateDate ( Guid key, string dayDelta )
        {
            int day;
            if ( int.TryParse ( dayDelta, out day ) && day != 0 )
            {
                var requestDispatcher = CreateAsyncRequestDispatcher ();
                requestDispatcher.Add ( new UpdateAssessmentReminderRequest
                {
                    AssessmentReminderKey = key,
                    DayDelta = day,
                } );

                var response = await requestDispatcher.GetAsync<DtoResponse<AssessmentReminderDto>> ();
                var dto = response.DataTransferObject;
            }
            return Json ( new {success = true}, JsonRequestBehavior.AllowGet );
        }

        #endregion
    }
}