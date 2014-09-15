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

    using DevExpress.Web.Mvc;
    using DevExpress.XtraReports.UI;

    using Newtonsoft.Json;

    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Metadata;
    using ProCenter.Domain.ReportsModule;
    using ProCenter.Service.Message.Assessment;
    using ProCenter.Service.Message.Report;

    using LookupDto = ProCenter.Service.Message.Common.Lookups.LookupDto;
    using QuestionResponse = ProCenter.Domain.ReportsModule.PatientsWithSpecificResponseReport.QuestionResponse;

    #endregion

    /// <summary>The report controller class.</summary>
    public class ReportController : BaseController
    {
        #region Fields

        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;

        private readonly IDbConnectionFactory _dbConnectionFactory;

        private readonly IResourcesManager _resourcesManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReportController" /> class.
        /// </summary>
        /// <param name="requestDispatcherFactory">The request dispatcher factory.</param>
        /// <param name="resourcesManager">The resources manager.</param>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="assessmentDefinitionRepository">The assessment definition repository.</param>
        public ReportController (
            IRequestDispatcherFactory requestDispatcherFactory,
            IResourcesManager resourcesManager,
            IDbConnectionFactory connectionFactory,
            IAssessmentDefinitionRepository assessmentDefinitionRepository )
            : base ( requestDispatcherFactory )
        {
            _resourcesManager = resourcesManager;
            _dbConnectionFactory = connectionFactory;
            _assessmentDefinitionRepository = assessmentDefinitionRepository;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Assessments the score over time export report builder viewer.
        /// </summary>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Returns a Task as type ActionResult.</returns>
        public async Task<ActionResult> AssessmentScoreOverTimeExportReportBuilderViewer ( string reportName, AssessmentScoreOverTimeParametersDto parameters )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetAssessmentScoreOverTimeReportRequest { AssessmentScoreOverTimeParametersDto = parameters } );
            var response = await requestDispatcher.GetAsync<GetReportResponse> ();
            return ReportViewerExtension.ExportTo ( (XtraReport)response.Report );
        }

        /// <summary>
        ///     Assessments the score over time parameters dto report builder.
        /// </summary>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Returns a Task of type ActionResult.</returns>
        public async Task<ActionResult> AssessmentScoreOverTimeParametersDtoReportBuilder ( string reportName, AssessmentScoreOverTimeParametersDto parameters )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetAssessmentScoreOverTimeReportRequest { AssessmentScoreOverTimeParametersDto = parameters } );
            var response = await requestDispatcher.GetAsync<GetReportResponse> ();
            ViewData["ReportName"] = reportName;
            return GetReportBuilderPartial ( reportName, parameters, response.Report );
        }

        /// <summary>
        ///     Customizes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <returns>
        ///     A <see cref="PartialViewResult" />.
        /// </returns>
        [HttpGet]
        public async Task<PartialViewResult> Customize ( Guid key, string reportName )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetReportCustomizationModelRequest { SourceKey = key, ReportName = reportName } );
            var response = await requestDispatcher.GetAsync<GetReportCustomizationModelResponse> ();

            ViewData["ResourceManager"] = _resourcesManager.GetResourceManagerByName ( reportName );

            return PartialView ( response.ReportModelDto );
        }

        /// <summary>
        ///     Customizes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="reportItemDto">The report item dto.</param>
        /// <returns>
        ///     A <see cref="ActionResult" />.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> Customize ( Guid key, string reportName, ReportItemDto reportItemDto )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new SaveReportCustomizationModelRequest { SourceKey = key, ReportName = reportName, ReportItemDto = reportItemDto } );
            await requestDispatcher.GetAsync<SaveReportCustomizationModelResponse> ();
            return null;
        }

        /// <summary>
        ///     Exports the report builder viewer.
        /// </summary>
        /// <param name="reportName">Name of the report.</param>
        /// <returns>Returns a Task as type ActionResult.</returns>
        public async Task<ActionResult> ExportReportBuilderViewer ( string reportName )
        {
            var methodName = reportName + "ExportReportBuilderViewer";
            var method = GetType ().GetMethod ( methodName );
            var parametersInfo = method.GetParameters ()[1];
            var parameters = GetParametersFromType ( parametersInfo.ParameterType );
            var awaiter = method.Invoke ( this, new[] { reportName, parameters } ) as Task<ActionResult>;
            return await awaiter;
        }

        /// <summary>
        ///     Exports the report viewer.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <returns>
        ///     A <see cref="ActionResult" />.
        /// </returns>
        public async Task<ActionResult> ExportReportViewer ( Guid key, string reportName )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetReportRequest { SourceKey = key, ReportName = reportName } );
            var response = await requestDispatcher.GetAsync<GetReportResponse> ();
            return ReportViewerExtension.ExportTo ( (XtraReport)response.Report );
        }

        /// <summary>
        ///     Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <returns>
        ///     A <see cref="ActionResult" />.
        /// </returns>
        public ActionResult Get ( Guid key, string reportName )
        {
            ViewData["SourceKey"] = key;
            ViewData["ReportName"] = reportName;
            return View ();
        }

        /// <summary>
        ///     Gets the report builder.
        /// </summary>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        ///     A <see cref="PartialViewResult" />.
        /// </returns>
        public PartialViewResult GetReportBuilder ( string reportName, object parameters )
        {
            ViewData["Parameters"] = parameters;
            ViewData["ReportName"] = reportName;
            return PartialView ();
        }

        /// <summary>
        ///     Gets the report container.
        /// </summary>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Returns a PartialViewResult.</returns>
        public PartialViewResult GetReportContainer ( string reportName, object parameters )
        {
            ViewData["Parameters"] = parameters;
            ViewData["ReportName"] = reportName;
            return PartialView ( "~/Views/Widgets/ReportContainer.cshtml" );
        }

        /// <summary>
        ///     Gets the report parameters for assessment.
        /// </summary>
        /// <param name="scoreType">Type of the score.</param>
        /// <returns>
        ///     A <see cref="PartialViewResult" />.
        /// </returns>
        public ActionResult GetReportParametersForAssessment ( string scoreType )
        {
            ViewData.TemplateInfo.HtmlFieldPrefix = "Parameters.ScoreType";
            //// todo: we can fetch data and return the model if we need to
            //// todo: get rid of the switch statement, find a better way
            switch ( scoreType )
            {
                case "ScoreTypeInt":
                    return PartialView ( scoreType, new ScoreTypeIntDto () );
                case "ScoreTypeBoolean":
                    return PartialView ( scoreType, new ScoreTypeBooleanDto () );
                default:
                    return null;
            }
        }

        /// <summary>
        ///     Nots the completed assessment export report builder viewer.
        /// </summary>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Returns a Task as type ActionResult.</returns>
        public async Task<ActionResult> NotCompletedAssessmentExportReportBuilderViewer ( string reportName, NotCompletedAssessmentParametersDto parameters )
        {
            parameters.OrganizationKey = UserContext.Current.OrganizationKey;
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetNotCompletedAssessmentReportRequest { NotCompletedAssessmentParametersDto = parameters } );
            var response = await requestDispatcher.GetAsync<GetReportResponse> ();
            return ReportViewerExtension.ExportTo ( (XtraReport)response.Report );
        }

        /// <summary>
        ///     Nots the completed assessment parameters dto report builder.
        /// </summary>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Returns a Task of type ActionResult.</returns>
        public async Task<ActionResult> NotCompletedAssessmentParametersDtoReportBuilder ( string reportName, NotCompletedAssessmentParametersDto parameters )
        {
            parameters.OrganizationKey = UserContext.Current.OrganizationKey;
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetNotCompletedAssessmentReportRequest { NotCompletedAssessmentParametersDto = parameters } );
            var response = await requestDispatcher.GetAsync<GetReportResponse> ();
            ViewData["ReportName"] = reportName;
            return GetReportBuilderPartial ( reportName, parameters, response.Report );
        }

        /// <summary>Parameterses the specified key.</summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <returns>
        ///     A <see cref="PartialViewResult" />.
        /// </returns>
        [HttpGet]
        public async Task<PartialViewResult> Parameters ( Guid? key, string reportName, Guid? patientKey = null )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetReportParametersRequest { ReportName = reportName, TemplateKey = key, PatientKey = patientKey } );
            var response = await requestDispatcher.GetAsync<ReportParametersResponse> ();
            var dto = response.Parameters;

            requestDispatcher.Clear ();
            //// todo: the lookups should be populated when the parameters is built
            AddLookupRequests ( requestDispatcher, response.Parameters.GetType () );
            AddLookupResponsesToViewData ( requestDispatcher );
            ViewData["ResourceManager"] = _resourcesManager.GetResourceManagerByName ( reportName );
            ViewData["ReportTemplateKey"] = key;
            ViewData["PatientKey"] = patientKey;
            ViewData["ReportName"] = reportName;
            ViewData["ReportDisplayName"] = _resourcesManager.GetResourceManagerByName ( reportName ).GetString ( "ReportName" );
            ViewData.TemplateInfo.HtmlFieldPrefix = "Parameters";
            
            if (reportName.Equals(ReportNames.PatientsWithSpecificResponse) || reportName.Equals(ReportNames.PatientsWithSpecificResponseAcrossAssessments))
            {
                var parameters = dto as PatientsWithSpecificResponseParametersDto;
                if ( parameters != null && parameters.Responses.Any () )
                {
                    var assessmentDefKeyNamePairs = new Dictionary<Guid, string> ();
                    foreach ( var question in parameters.Responses )
                    {
                        var questionItemDto = GetQuestionItemDto(question);
                        if (questionItemDto != null)
                        {
                            if (!assessmentDefKeyNamePairs.ContainsKey(question.AssessmentDefinitionKey))
                            {
                                var assessmentDefinition = _assessmentDefinitionRepository.GetByKey(question.AssessmentDefinitionKey);
                                assessmentDefKeyNamePairs[question.AssessmentDefinitionKey] = assessmentDefinition.CodedConcept.Name;
                            }
                            questionItemDto.AssessmentName = assessmentDefKeyNamePairs[question.AssessmentDefinitionKey];
                            parameters.Items.Add(questionItemDto);
                            ViewData["AssessmentDefinitionKey"] = question.AssessmentDefinitionKey;
                        }
                    }
                    parameters.JsonResponse = JsonConvert.SerializeObject ( parameters.Responses );
                    ViewData["ResourcesManager"] = _resourcesManager;
                    parameters.ReportName = reportName;
                }
            }

            return PartialView ( reportName + "Parameters", dto );
        }

        /// <summary>
        ///     Patients the score range export report builder viewer.
        /// </summary>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Returns a Task as type ActionResult.</returns>
        public async Task<ActionResult> PatientScoreRangeExportReportBuilderViewer ( string reportName, PatientScoreRangeParametersDto parameters )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetPatientScoreRangeReportRequest { PatientScoreRangeParametersDto = parameters } );
            var response = await requestDispatcher.GetAsync<GetReportResponse> ();
            return ReportViewerExtension.ExportTo ( (XtraReport)response.Report );
        }

        /// <summary>
        ///     Patients the score range parameters dto report builder.
        /// </summary>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        ///     A <see cref="PartialViewResult" />.
        /// </returns>
        public async Task<ActionResult> PatientScoreRangeParametersDtoReportBuilder ( string reportName, PatientScoreRangeParametersDto parameters )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetPatientScoreRangeReportRequest { PatientScoreRangeParametersDto = parameters } );
            var response = await requestDispatcher.GetAsync<GetReportResponse> ();
            ViewData["ReportName"] = reportName;
            return GetReportBuilderPartial ( reportName, parameters, response.Report );
        }

        /// <summary>
        /// Patients with specific response across assessments export report builder viewer.
        /// </summary>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Returns a Task as type ActionResult.</returns>
        public async Task<ActionResult> PatientsWithSpecificResponseAcrossAssessmentsExportReportBuilderViewer (
            string reportName, PatientsWithSpecificResponseParametersDto parameters )
        {
            return await PatientsWithSpecificResponseAndAcrossAssessmentsExport ( reportName, parameters );
        }

        /// <summary>
        ///     Patientses the with specific response export report builder viewer.
        /// </summary>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Returns a Task as type ActionResult.</returns>
        public async Task<ActionResult> PatientsWithSpecificResponseExportReportBuilderViewer ( string reportName, PatientsWithSpecificResponseParametersDto parameters )
        {
            return await PatientsWithSpecificResponseAndAcrossAssessmentsExport ( reportName, parameters );
        }

        /// <summary>
        ///     Patientses the with specific response parameters dto report builder.
        /// </summary>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A task as type ActionResult.</returns>
        public async Task<ActionResult> PatientsWithSpecificResponseParametersDtoReportBuilder ( string reportName, PatientsWithSpecificResponseParametersDto parameters )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            ViewData["ReportName"] = reportName;
            parameters.ReportName = reportName;
            parameters.Responses = parameters.JsonResponse == null ? null : JsonConvert.DeserializeObject<List<QuestionResponse>> ( parameters.JsonResponse );
            parameters.OrganizationKey = UserContext.Current.OrganizationKey;
            requestDispatcher.Add ( new GetPatientsWithSpecificResponseReportRequest { PatientsWithSpecificResponseParametersDto = parameters } );
            var response = await requestDispatcher.GetAsync<GetReportResponse> ();
            return GetReportBuilderPartial ( reportName, parameters, response.Report );
        }

        /// <summary>
        ///     Reports the builder viewer partial.
        /// </summary>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        ///     A <see cref="PartialViewResult" />.
        /// </returns>
        public ActionResult ReportBuilderViewerPartial ( string reportName, AssessmentScoreOverTimeParametersDto parameters )
        {
            ViewData["Parameters"] = parameters;
            ViewData["ReportName"] = reportName;

            return PartialView ();
        }

        /// <summary>
        ///     Reports the viewer partial.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <returns>
        ///     A <see cref="ActionResult" />.
        /// </returns>
        public async Task<ActionResult> ReportViewerPartial ( Guid key, string reportName )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add ( new GetReportRequest { SourceKey = key, ReportName = reportName } );
            var response = await requestDispatcher.GetAsync<GetReportResponse> ();
            ViewData["SourceKey"] = key;
            ViewData["ReportName"] = reportName;

            return PartialView ( "ReportViewerPartial", response.Report );
        }

        /// <summary>
        ///     Saves the report parameters.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        ///     A <see cref="ActionResult" />.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> SaveReportParametersAssessmentScoreOverTime ( Guid? key, string reportName, AssessmentScoreOverTimeParametersDto parameters )
        {
            if ( parameters.TimePeriod != null && !string.IsNullOrEmpty ( parameters.TimePeriod.Code ) )
            {
                DateTime? startDate;
                DateTime? endDate;
                GetRange ( parameters.TimePeriod.Code, out startDate, out endDate );
                parameters.TimePeriod = null;
                parameters.StartDate = startDate;
                parameters.EndDate = endDate;
            }
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add (
                new SaveReportTemplateRequest
                    {
                        ReportTemplate =
                            new ReportTemplateDto
                                {
                                    Key = key ?? Guid.Empty,
                                    Name = reportName,
                                    SystemAccountKey = UserContext.Current.SystemAccountKey.GetValueOrDefault (),
                                    PatientKey = parameters.PatientKey,
                                    Parameters = parameters,
                                    ReportType = ReportType.Saved,
                                    ReportState = new LookupDto { Code = "Normal" }
                                }
                    } );
            if ( DoesReportTemplateExist ( reportName, ReportType.Saved.ToString (), parameters ) )
            {
                return Json ( new { success = true, error = Controllers.SavedReportAlreadyExists }, JsonRequestBehavior.AllowGet );
            }
            var response = await requestDispatcher.GetAsync<SaveReportTemplateResponse> ();
            return Json ( new { success = true, reportTempate = response.ReportTemplate }, JsonRequestBehavior.AllowGet );
        }

        /// <summary>
        ///     Saves the report parameters not completed assessment.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Returns a Task of type ActionResult.</returns>
        [HttpPost]
        public async Task<ActionResult> SaveReportParametersNotCompletedAssessment ( Guid? key, string reportName, NotCompletedAssessmentParametersDto parameters )
        {
            parameters.OrganizationKey = UserContext.Current.OrganizationKey;
            if ( parameters.TimePeriod != null && !string.IsNullOrEmpty ( parameters.TimePeriod.Code ) )
            {
                DateTime? startDate;
                DateTime? endDate;
                GetRange ( parameters.TimePeriod.Code, out startDate, out endDate );
                parameters.TimePeriod = null;
                parameters.StartDate = startDate;
                parameters.EndDate = endDate;
            }
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add (
                new SaveReportTemplateRequest
                    {
                        ReportTemplate =
                            new ReportTemplateDto
                                {
                                    Key = key ?? Guid.Empty,
                                    Name = reportName,
                                    SystemAccountKey = UserContext.Current.SystemAccountKey.GetValueOrDefault (),
                                    PatientKey = parameters.PatientKey,
                                    Parameters = parameters,
                                    ReportType = ReportType.Saved,
                                    ReportState = new LookupDto { Code = "Normal" }
                                }
                    } );
            if ( DoesReportTemplateExist ( reportName, ReportType.Saved.ToString (), parameters ) )
            {
                return Json ( new { success = true, error = Controllers.SavedReportAlreadyExists }, JsonRequestBehavior.AllowGet );
            }
            var response = await requestDispatcher.GetAsync<SaveReportTemplateResponse> ();
            return Json ( new { success = true, reportTempate = response.ReportTemplate }, JsonRequestBehavior.AllowGet );
        }

        /// <summary>
        ///     Saves the report parameters patient score range.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Returns a Task of type ActionResult.</returns>
        [HttpPost]
        public async Task<ActionResult> SaveReportParametersPatientScoreRange ( Guid? key, string reportName, PatientScoreRangeParametersDto parameters )
        {
            if ( parameters.TimePeriod != null && !string.IsNullOrEmpty ( parameters.TimePeriod.Code ) )
            {
                DateTime? startDate;
                DateTime? endDate;
                GetRange ( parameters.TimePeriod.Code, out startDate, out endDate );
                parameters.TimePeriod = null;
                parameters.StartDate = startDate;
                parameters.EndDate = endDate;
            }
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add (
                new SaveReportTemplateRequest
                    {
                        ReportTemplate =
                            new ReportTemplateDto
                                {
                                    Key = key ?? Guid.Empty,
                                    Name = reportName,
                                    SystemAccountKey = UserContext.Current.SystemAccountKey.GetValueOrDefault (),
                                    PatientKey = parameters.PatientKey,
                                    Parameters = parameters,
                                    ReportType = ReportType.Saved,
                                    ReportState = new LookupDto { Code = "Normal" }
                                }
                    } );
            if ( DoesReportTemplateExist ( reportName, ReportType.Saved.ToString (), parameters ) )
            {
                return Json ( new { success = true, error = Controllers.SavedReportAlreadyExists }, JsonRequestBehavior.AllowGet );
            }
            var response = await requestDispatcher.GetAsync<SaveReportTemplateResponse> ();
            return Json ( new { success = true, reportTempate = response.ReportTemplate }, JsonRequestBehavior.AllowGet );
        }

        /// <summary>
        ///     Saves the report parameters patients with specific response.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A task as type ActionResult.</returns>
        public async Task<ActionResult> SaveReportParametersPatientsWithSpecificResponse ( Guid? key, string reportName, PatientsWithSpecificResponseParametersDto parameters )
        {
            return await SaveReportPatientsWithSpecificResponseAndAcrossAssessments ( key, reportName, parameters );
        }

        /// <summary>
        ///     Saves the report parameters patients with specific response across assessments.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A task as type ActionResult.</returns>
        public async Task<ActionResult> SaveReportParametersPatientsWithSpecificResponseAcrossAssessments (
            Guid? key, string reportName, PatientsWithSpecificResponseParametersDto parameters )
        {
            return await SaveReportPatientsWithSpecificResponseAndAcrossAssessments ( key, reportName, parameters );
        }

        /// <summary>
        ///     Saves the report template.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        ///     A <see cref="ActionResult" />.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> SaveReportTemplateAssessmentScoreOverTime ( Guid? key, string reportName, AssessmentScoreOverTimeParametersDto parameters )
        {
            //todo: need to think about saving organizationKey instead of patientKey
            if ( parameters.TimePeriod != null && !string.IsNullOrEmpty ( parameters.TimePeriod.Code ) )
            {
                parameters.StartDate = parameters.EndDate = null;
            }
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add (
                new SaveReportTemplateRequest
                    {
                        ReportTemplate =
                            new ReportTemplateDto
                                {
                                    Key = key ?? Guid.Empty,
                                    Name = reportName,
                                    SystemAccountKey = UserContext.Current.SystemAccountKey.GetValueOrDefault (),
                                    PatientKey = parameters.PatientKey,
                                    Parameters = parameters,
                                    ReportType = ReportType.Template,
                                    ReportState = new LookupDto { Code = "Normal" }
                                }
                    } );
            if ( DoesReportTemplateExist ( reportName, ReportType.Template.ToString (), parameters ) )
            {
                return Json ( new { success = true, error = Controllers.TemplateAlreadyExists }, JsonRequestBehavior.AllowGet );
            }
            var response = await requestDispatcher.GetAsync<SaveReportTemplateResponse> ();
            return Json ( new { success = true, reportTempate = response.ReportTemplate }, JsonRequestBehavior.AllowGet );
        }

        /// <summary>
        ///     Saves the report template not completed assessment.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Returns a Task of type ActionResult.</returns>
        [HttpPost]
        public async Task<ActionResult> SaveReportTemplateNotCompletedAssessment ( Guid? key, string reportName, NotCompletedAssessmentParametersDto parameters )
        {
            parameters.OrganizationKey = UserContext.Current.OrganizationKey;
            ////todo: need to think about saving organizationKey instead of patientKey
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add (
                new SaveReportTemplateRequest
                    {
                        ReportTemplate =
                            new ReportTemplateDto
                                {
                                    Key = key ?? Guid.Empty,
                                    Name = reportName,
                                    SystemAccountKey = UserContext.Current.SystemAccountKey.GetValueOrDefault (),
                                    PatientKey = parameters.PatientKey,
                                    Parameters = parameters,
                                    ReportType = ReportType.Template,
                                    ReportState = new LookupDto { Code = "Normal" }
                                }
                    } );
            if ( DoesReportTemplateExist ( reportName, ReportType.Template.ToString (), parameters ) )
            {
                return Json ( new { success = true, error = Controllers.TemplateAlreadyExists }, JsonRequestBehavior.AllowGet );
            }
            var response = await requestDispatcher.GetAsync<SaveReportTemplateResponse> ();
            return Json ( new { success = true, reportTempate = response.ReportTemplate }, JsonRequestBehavior.AllowGet );
        }

        /// <summary>
        ///     Saves the report template patient score range.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Returns a Task of type ActionResult.</returns>
        [HttpPost]
        public async Task<ActionResult> SaveReportTemplatePatientScoreRange ( Guid? key, string reportName, PatientScoreRangeParametersDto parameters )
        {
            ////todo: need to think about saving organizationKey instead of patientKey
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add (
                new SaveReportTemplateRequest
                    {
                        ReportTemplate =
                            new ReportTemplateDto
                                {
                                    Key = key ?? Guid.Empty,
                                    Name = reportName,
                                    SystemAccountKey = UserContext.Current.SystemAccountKey.GetValueOrDefault (),
                                    PatientKey = parameters.PatientKey,
                                    Parameters = parameters,
                                    ReportType = ReportType.Template,
                                    ReportState = new LookupDto { Code = "Normal" }
                                }
                    } );
            if ( DoesReportTemplateExist ( reportName, ReportType.Template.ToString (), parameters ) )
            {
                return Json ( new { success = true, error = Controllers.TemplateAlreadyExists }, JsonRequestBehavior.AllowGet );
            }
            var response = await requestDispatcher.GetAsync<SaveReportTemplateResponse> ();
            return Json ( new { success = true, reportTempate = response.ReportTemplate }, JsonRequestBehavior.AllowGet );
        }

        /// <summary>
        ///     Saves the report template patients with specific response.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A task as type ActionResult.</returns>
        [HttpPost]
        public async Task<ActionResult> SaveReportTemplatePatientsWithSpecificResponse ( Guid? key, string reportName, PatientsWithSpecificResponseParametersDto parameters )
        {
            return await SaveTemplatePatientsWithSpecificResponseAndAcrossAssessments ( key, reportName, parameters );
        }

        /// <summary>
        /// Saves the report template patients with specific response across assessments.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Returns a Task as type ActionResult.</returns>
        [HttpPost]
        public async Task<ActionResult> SaveReportTemplatePatientsWithSpecificResponseAcrossAssessments (
            Guid? key, string reportName, PatientsWithSpecificResponseParametersDto parameters )
        {
            return await SaveTemplatePatientsWithSpecificResponseAndAcrossAssessments ( key, reportName, parameters );
        }

        #endregion

        #region Methods

        private bool DoesReportTemplateExist ( string reportName, string reportType, object parameters )
        {
            const string Query = @"SELECT
                                   [ReportTemplateKey]
                                  ,[SystemAccountKey]
                                  ,[Name]
                                  ,[ReportType]
                                  ,[Parameters]
                                  ,[ReportStateCode]
                                  ,[PatientKey]
                                  ,[OrganizationKey]
                              FROM [ReportModule].[ReportTemplate] 
                              WHERE Name = @Name AND ReportType = @ReportType AND Parameters = @Parameters";
            using ( var connection = _dbConnectionFactory.CreateConnection () )
            {
                var parms = parameters.ToString ();
                var dto = connection.Query<ReportTemplateDto> ( Query, new { Name = reportName, ReportType = reportType, Parameters = parms } ).ToList ();
                return dto.Any ();
            }
        }

        private object GetParametersFromType ( Type parameterType )
        {
            var binder = Binders.GetBinder ( parameterType );
            var model = Activator.CreateInstance ( parameterType );
            var bindingContext = new ModelBindingContext
                                     {
                                         ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType ( () => model, parameterType ),
                                         ModelName = "Parameters",
                                         ModelState = ModelState,
                                         ValueProvider = ValueProvider
                                     };
            binder.BindModel ( ControllerContext, bindingContext );
            return model;
        }

        private ItemDto GetQuestionItemDto ( QuestionResponse questionResponse )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add (
                new GetItemDtoByKeyRequest
                    {
                        ItemDefinitionCode = questionResponse.ItemDefinitionCode,
                        AssessmentDefinitionKey = questionResponse.AssessmentDefinitionKey
                    } );
            var itemDtoResponse = requestDispatcher.Get<GetItemDtoByKeyResponse> ();
            if (itemDtoResponse.DataTransferObject == null)
            {
                return null;
            }
            var dto = itemDtoResponse.DataTransferObject;
            var metaData = dto.Metadata.MetadataItems.FirstOrDefault ( a => a.GetType () == typeof(ItemTemplateMetadataItem) );
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
            dto.ParentName = questionResponse.ParentName;
            if ( !questionResponse.IsLookup )
            {
                if ( templateMetaData.TemplateName.Equals ( "IntRange" ) )
                {
                    dto.Value = questionResponse.Responses.Select ( v => Convert.ToInt32 ( v ) ).ToList ();
                }
                else
                {
                    dto.Value = questionResponse.Responses.FirstOrDefault ();
                }
            }
            else
            {
                dto.Value = questionResponse.Responses.Select ( v => new LookupDto { Code = v } );
            }

            return dto;
        }

        private void GetRange ( string timePeriodCode, out DateTime? start, out DateTime? end )
        {
            start = end = DateTime.Now;
            if ( timePeriodCode == "LastMonth" )
            {
                start = end.Value.AddMonths (-1);
            }
            else if ( timePeriodCode == "LastThreeMonths" )
            {
                start = end.Value.AddMonths (-3);
            }
            else if ( timePeriodCode == "LastSixMonths" )
            {
                start = end.Value.AddMonths (-6);
            }
            else if ( timePeriodCode == "LastYear" )
            {
                start = end.Value.AddYears (-1);
            }
        }

        /// <summary>
        ///     Gets the report builder partial.
        /// </summary>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="report">The report.</param>
        /// <returns>
        ///     A <see cref="PartialViewResult" />.
        /// </returns>
        private PartialViewResult GetReportBuilderPartial ( string reportName, object parameters, object report )
        {
            ViewData["Parameters"] = parameters;
            ViewData["ReportName"] = reportName;

            return PartialView ( "ReportBuilderViewerPartial", report );
        }

        private async Task<ActionResult> PatientsWithSpecificResponseAndAcrossAssessmentsExport ( string reportName, PatientsWithSpecificResponseParametersDto parameters )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            ViewData["ReportName"] = reportName;
            parameters.ReportName = reportName;
            parameters.Responses = JsonConvert.DeserializeObject<List<QuestionResponse>> ( parameters.JsonResponse );
            parameters.OrganizationKey = UserContext.Current.OrganizationKey;
            requestDispatcher.Add ( new GetPatientsWithSpecificResponseReportRequest { PatientsWithSpecificResponseParametersDto = parameters } );
            var response = await requestDispatcher.GetAsync<GetReportResponse> ();
            return ReportViewerExtension.ExportTo ( (XtraReport)response.Report );
        }

        /// <summary>
        /// Saves the report patients with specific response and across assessments.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Returns a Task as type ActionResult.</returns>
        private async Task<ActionResult> SaveReportPatientsWithSpecificResponseAndAcrossAssessments (
            Guid? key, string reportName, PatientsWithSpecificResponseParametersDto parameters )
        {
            parameters.Responses = parameters.JsonResponse == null ? null : JsonConvert.DeserializeObject<List<QuestionResponse>> ( parameters.JsonResponse );
            if ( parameters.TimePeriod != null && !string.IsNullOrEmpty ( parameters.TimePeriod.Code ) )
            {
                DateTime? startDate;
                DateTime? endDate;
                GetRange ( parameters.TimePeriod.Code, out startDate, out endDate );
                parameters.TimePeriod = null;
                parameters.StartDate = startDate;
                parameters.EndDate = endDate;
            }

            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add (
                new SaveReportTemplateRequest
                    {
                        ReportTemplate =
                            new ReportTemplateDto
                                {
                                    Key = key ?? Guid.Empty,
                                    Name = reportName,
                                    SystemAccountKey = UserContext.Current.SystemAccountKey.GetValueOrDefault (),
                                    PatientKey = parameters.PatientKey,
                                    Parameters = parameters,
                                    ReportType = ReportType.Saved,
                                    ReportState = new LookupDto { Code = "Normal" }
                                }
                    } );

            if ( DoesReportTemplateExist ( reportName, ReportType.Saved.ToString (), parameters ) )
            {
                return Json ( new { success = true, error = Controllers.SavedReportAlreadyExists }, JsonRequestBehavior.AllowGet );
            }

            var response = await requestDispatcher.GetAsync<SaveReportTemplateResponse> ();
            return Json ( new { success = true, reportTempate = response.ReportTemplate }, JsonRequestBehavior.AllowGet );
        }

        /// <summary>
        ///     Saves the template patients with specific response and across assessments.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A task as type ActionResult.</returns>
        private async Task<ActionResult> SaveTemplatePatientsWithSpecificResponseAndAcrossAssessments (
            Guid? key, string reportName, PatientsWithSpecificResponseParametersDto parameters )
        {
            ViewData["ReportName"] = reportName;
            parameters.ReportName = reportName;
            parameters.Responses = parameters.JsonResponse == null ? null : JsonConvert.DeserializeObject<List<QuestionResponse>> ( parameters.JsonResponse );

            ////todo: need to think about saving organizationKey instead of patientKey
            var requestDispatcher = CreateAsyncRequestDispatcher ();
            requestDispatcher.Add (
                new SaveReportTemplateRequest
                    {
                        ReportTemplate =
                            new ReportTemplateDto
                                {
                                    Key = key ?? Guid.Empty,
                                    Name = reportName,
                                    SystemAccountKey = UserContext.Current.SystemAccountKey.GetValueOrDefault (),
                                    PatientKey = parameters.PatientKey,
                                    Parameters = parameters,
                                    ReportType = ReportType.Template,
                                    ReportState = new LookupDto { Code = "Normal" }
                                }
                    } );
            if ( DoesReportTemplateExist ( reportName, ReportType.Template.ToString (), parameters ) )
            {
                return Json ( new { success = true, error = Controllers.TemplateAlreadyExists }, JsonRequestBehavior.AllowGet );
            }
            var response = await requestDispatcher.GetAsync<SaveReportTemplateResponse> ();
            return Json ( new { success = true, reportTempate = response.ReportTemplate }, JsonRequestBehavior.AllowGet );
        }

        #endregion
    }
}