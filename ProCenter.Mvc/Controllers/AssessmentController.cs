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

namespace ProCenter.Mvc.Controllers
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Agatha.Common;

    using Dapper;

    using NLog;

    using Pillar.Agatha.Message;

    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule.Metadata;
    using ProCenter.Mvc.Models;
    using ProCenter.Service.Message.Assessment;
    using ProCenter.Service.Message.Common;
    using ProCenter.Service.Message.Common.Lookups;
    using ProCenter.Service.Message.Message;
    using ProCenter.Service.Message.Patient;

    #endregion

    /// <summary>The assessment controller class.</summary>
    public class AssessmentController : BaseController
    {
        #region Constants

        private const string QueryActive = @"SELECT [OrganizationKey]
                                           ,[AssessmentDefinitionKey]
                                           ,[AssessmentName]
                                           ,[AssessmentCode]
                                           ,[ScoreType]
                                         FROM [OrganizationModule].[OrganizationAssessmentDefinition]
                                         WHERE OrganizationKey = '{0}' 
                                         AND AssessmentDefinitionKey = '{1}'";

        #endregion

        #region Fields

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IResourcesManager _resourcesManager;

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssessmentController" /> class.
        /// </summary>
        /// <param name="requestDispatcherFactory">The request dispatcher factory.</param>
        /// <param name="resourcesManager">The resources manager.</param>
        /// <param name="dbConnectionFactory">The database connection factory.</param>
        public AssessmentController (
            IRequestDispatcherFactory requestDispatcherFactory,
            IResourcesManager resourcesManager,
            IDbConnectionFactory dbConnectionFactory)
            : base ( requestDispatcherFactory )
        {
            _resourcesManager = resourcesManager;
            _connectionFactory = dbConnectionFactory;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Creates the specified patient key.
        /// </summary>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="assessmentDefinitionKey">The assessment definition key.</param>
        /// <param name="workflowKey">The workflow key.</param>
        /// <param name="assessmentReminderKey">The assessment reminder key.</param>
        /// <param name="recurrenceKey">The recurrence key.</param>
        /// <returns>
        ///     A <see cref="ActionResult" />.
        /// </returns>
        public async Task<ActionResult> Create (
            Guid patientKey,
            Guid assessmentDefinitionKey,
            Guid? workflowKey = null,
            Guid? assessmentReminderKey = null,
            Guid? recurrenceKey = null )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            bool canSelfAdminister = false;
            if (assessmentReminderKey != null)
            {
                requestDispatcher.Add(new GetAssessmentReminderByKeyRequest
                {
                    AssessmentReminderKey = recurrenceKey.GetValueOrDefault()
                });
                var assessmentReminderResponse = await requestDispatcher.GetAsync<DtoResponse<AssessmentReminderDto>>();
                canSelfAdminister = assessmentReminderResponse.DataTransferObject.ForSelfAdministration;
            }

            requestDispatcher.Clear();
            requestDispatcher.Add (
                new CreateAssessmentRequest
                {
                    AssessmentDefinitionKey = assessmentDefinitionKey,
                    PatientKey = patientKey,
                    WorkflowKey = workflowKey,
                    ForSelfAdministration = canSelfAdminister
                } );
            var response = await requestDispatcher.GetAsync<CreateAssessmentResponse> ();

            if ( assessmentReminderKey != null )
            {
                var reminderDto = new AssessmentReminderDto
                                  {
                                      OrganizationKey = UserContext.Current.OrganizationKey.Value,
                                      AssessmentInstanceKey = response.AssessmentInstanceKey,
                                      RecurrenceKey = recurrenceKey.Value
                                  };
                requestDispatcher.Clear ();
                requestDispatcher.Add (
                    new UpdateAssessmentReminderRequest
                    {
                        AssessmentReminderDto = reminderDto,
                        AssessmentReminderKey = assessmentReminderKey.Value
                    } );
                await requestDispatcher.GetAsync<DtoResponse<AssessmentReminderDto>> ();
            }

            return RedirectToAction ( "Edit", new { key = response.AssessmentInstanceKey, patientKey } );
        }

        /// <summary>
        ///     Creates for self administration.
        /// </summary>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="assessmentDefinitionKey">The assessment definition key.</param>
        /// <param name="administerNow">
        ///     If set to <c>true</c> [administer now].
        /// </param>
        /// <param name="workflowKey">The workflow key.</param>
        /// <returns>
        ///     A <see cref="ActionResult" />.
        /// </returns>
        public async Task<ActionResult> CreateForSelfAdministration ( Guid patientKey, Guid assessmentDefinitionKey, bool administerNow = false, Guid? workflowKey = null )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add (
                new CreateAssessmentRequest
                {
                    AssessmentDefinitionKey = assessmentDefinitionKey,
                    PatientKey = patientKey,
                    WorkflowKey = workflowKey,
                    ForSelfAdministration = true,
                    AssessmentInstanceUrl = Url.Action ( "Edit", "Assessment", null, Request.Url.Scheme )
                } );
            var response = await requestDispatcher.GetAsync<CreateAssessmentResponse> ();

            if ( administerNow )
            {
                return RedirectToAction ( "Edit", new { key = response.AssessmentInstanceKey, patientKey } );
            }
            return RedirectToAction ( "Index", "Patient", new { key = patientKey } );
        }

        /// <summary>
        ///     Edits the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <returns>
        ///     A <see cref="ActionResult" />.
        /// </returns>
        public virtual async Task<ActionResult> Edit ( Guid key, Guid patientKey )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetDtoByKeyRequest<AssessmentSectionSummaryDto> { Key = key } );
            requestDispatcher.Add ( new GetSectionDtoByKeyRequest { Key = key } );
            requestDispatcher.Add ( new GetPatientDtoByKeyRequest { PatientKey = patientKey } );

            var response = await requestDispatcher.GetAsync<DtoResponse<AssessmentSectionSummaryDto>> ();
            var sectionDtoResponse = await requestDispatcher.GetAsync<GetSectionDtoByKeyResponse> ();
            var patientResponse = requestDispatcher.Get<GetPatientDtoResponse> ();

            ViewData["Patient"] = patientResponse.DataTransferObject;
            var messages = new List<IMessageDto> ( ( TempData["Messages"] as IEnumerable<IMessageDto> ) ?? Enumerable.Empty<IMessageDto> () );
            messages.AddRange ( response.DataTransferObject.Messages.Where ( m => !messages.Contains ( m ) ) );
            ViewData["Messages"] = messages;

            var assessmentViewModel = new AssessmentViewModel ( response.DataTransferObject, sectionDtoResponse.DataTransferObject );

            ViewData["ResourceManager"] = _resourcesManager.GetResourceManagerByName ( assessmentViewModel.AssessmentSectionSummaryDto.AssessmentName );
            ViewData["ResourcesManager"] = _resourcesManager;

            return View ( assessmentViewModel );
        }

        /// <summary>
        ///     Edits the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="itemDefinitionCode">The item definition code.</param>
        /// <param name="value">The value.</param>
        /// <param name="values">The values.</param>
        /// <param name="isLookup">
        ///     If set to <c>true</c> [is lookup].
        /// </param>
        /// <param name="nonResponseType">Type of the non response.</param>
        /// <returns>
        ///     A <see cref="Task{PartialViewResult}" />.
        /// </returns>
        [HttpPost]
        public virtual async Task<JsonResult> Edit (
            Guid key,
            Guid patientKey,
            string itemDefinitionCode,
            string value = null,
            object values = null,
            bool isLookup = false,
            string nonResponseType = null )
        {
            ItemDto item;
            if ( !isLookup )
            {
                item = new ItemDto
                       {
                           ItemDefinitionCode = itemDefinitionCode,
                           Value = nonResponseType ?? value ?? values
                       };
            }
            else
            {
                item = new ItemDto
                       {
                           ItemDefinitionCode = itemDefinitionCode
                       };
                if ( value != null )
                {
                    item.Value = new LookupDto { Code = value };
                }
                else if ( values != null && values as IEnumerable<string> != null )
                {
                    item.Value = ( values as IEnumerable<string> ).Select ( v => new LookupDto { Code = v } );
                }
            }

            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add (
                new SaveAssessmentItemRequest
                {
                    Key = key,
                    Item = item,
                } );

            var response = await requestDispatcher.GetAsync<SaveAssessmentItemResponse> ();

            if ( response.HasErrors () )
            {
                return new JsonResult
                       {
                           Data = new
                                  {
                                      error = true,
                                      errors = new[] { response.Exception.Message }
                                  }
                       };
            }

            _logger.Info("Can Submit Assessment: " + response.CanSubmit);
            _logger.Info("Percent Complete: " + response.PercentComplete);
            return new JsonResult
                   {
                       Data = new
                              {
                                  response.CanSubmit,
                                  response.PercentComplete,
                                  Value = nonResponseType ?? value
                              }
                   };
        }

        /// <summary>Edits the section.</summary>
        /// <param name="key">The key.</param>
        /// <param name="sectionCode">The section code.</param>
        /// <returns>
        ///     A <see cref="Task{PartialViewResult}" />.
        /// </returns>
        public virtual async Task<PartialViewResult> EditSection ( Guid key, string sectionCode )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetSectionDtoByKeyRequest { Key = key, SectionItemDefinitionCode = sectionCode } );

            var sectionDtoResponse = await requestDispatcher.GetAsync<GetSectionDtoByKeyResponse> ();

            ViewData["ResourceManager"] = _resourcesManager.GetResourceManagerByName ( sectionDtoResponse.DataTransferObject.AssessmentName );
            ViewData["ResourcesManager"] = _resourcesManager;

            return PartialView ( "Section", sectionDtoResponse.DataTransferObject );
        }

        /// <summary>
        /// Gets the question definition.
        /// </summary>
        /// <param name="assessmentDefinitionKey">The assessment definition key.</param>
        /// <param name="itemDefinitionCode">The item definition code.</param>
        /// <param name="parentName">Name of the parent.</param>
        /// <returns>
        /// Returns a partial view for the question.
        /// </returns>
        public async Task<PartialViewResult> GetQuestionDefinition ( string assessmentDefinitionKey, string itemDefinitionCode, string parentName = null )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetItemDtoByKeyRequest { ItemDefinitionCode = itemDefinitionCode, AssessmentDefinitionKey = new Guid ( assessmentDefinitionKey ) } );
            var itemDtoResponse = await requestDispatcher.GetAsync<GetItemDtoByKeyResponse> ();
            if ( itemDtoResponse == null )
            {
                return null;
            }
            var metaData = itemDtoResponse.DataTransferObject.Metadata.MetadataItems.FirstOrDefault ( a => a.GetType () == typeof(ItemTemplateMetadataItem) );
            var templateMetaData = metaData as ItemTemplateMetadataItem;
            if ( templateMetaData == null )
            {
                return null;
            }
            switch ( templateMetaData.TemplateName )
            {
                case "Int32":
                    templateMetaData.TemplateName = "IntRange";
                    break;
                case "LookupDto":
                    templateMetaData.TemplateName = "MultipleSelect";
                    break;
            }
            var assessmentDef = GetAssessmentDefinition ( assessmentDefinitionKey );
            var assessmentName = string.Empty;
            if ( assessmentDef != null )
            {
                assessmentName = assessmentDef.AssessmentName;
            }
            ViewData["ResourceManager"] = _resourcesManager.GetResourceManagerByName(assessmentName);
            ViewData["ResourcesManager"] = _resourcesManager;
            itemDtoResponse.DataTransferObject.ParentName = parentName;
            return PartialView ( "QuestionForLookup", itemDtoResponse.DataTransferObject );
        }

        /// <summary>
        ///     Sends the email.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The email sent date.</returns>
        [HttpPost]
        public async Task<JsonResult> SendEmail ( Guid key )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            var assessmentInstanceUrl = Url.Action ( "Edit", "Assessment", null, Request.Url.Scheme );
            requestDispatcher.Add ( new SendEmailRequest { AssessmentInstanceKey = key, AssessmentInstanceUrl = assessmentInstanceUrl } );
            var response = await requestDispatcher.GetAsync<SendEmailResponse> ();

            return new JsonResult
                   {
                       Data = new
                              {
                                  IsSuccessful = !response.EmailFailedDate.HasValue,
                                  EmailSentDate = response.EmailSentDate.HasValue
                                      ? response.EmailSentDate.Value.ToString ()
                                      : string.Empty,
                                  EmailFailedDate = response.EmailFailedDate.HasValue
                                      ? response.EmailFailedDate.Value.ToString ()
                                      : string.Empty,
                                  ErrorMessage = AssessmentResources.ErrorOccurred,
                                  Key = key
                              }
                   };
        }

        /// <summary>
        ///     Submits the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="assessmentName">Name of the assessment.</param>
        /// <returns>
        ///     A <see cref="PartialViewResult" />.
        /// </returns>
        [HttpPost]
        public async Task<PartialViewResult> Submit ( Guid key, Guid patientKey, string assessmentName )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new SubmitAssessmentRequest { AssessmentKey = key, Submit = true } );
            requestDispatcher.Add ( new GetPatientDtoByKeyRequest { PatientKey = patientKey } );
            var response = await requestDispatcher.GetAsync<SubmitAssessmentResponse> ();
            var patientResponse = requestDispatcher.Get<GetPatientDtoResponse> ();

            ViewData["Patient"] = patientResponse.DataTransferObject;
            ViewData["ResourceManager"] = _resourcesManager.GetResourceManagerByName ( assessmentName );
            ViewData["ResourcesManager"] = _resourcesManager;

            return PartialView ( "ScoreHeader", new ScoreHeaderViewModel { Score = response.ScoreDto, Messages = response.Messages } );
        }

        #endregion

        #region Methods

        private AssessmentDefinitionDto GetAssessmentDefinition ( string assessmentCode )
        {
            var completeQuery = string.Format ( QueryActive, UserContext.Current.OrganizationKey, assessmentCode );

            using ( var connection = _connectionFactory.CreateConnection () )
            using ( var multiQuery = connection.QueryMultiple ( completeQuery ) )
            {
                return multiQuery.Read<AssessmentDefinitionDto> ().FirstOrDefault ();
            }
        }

        #endregion
    }
}