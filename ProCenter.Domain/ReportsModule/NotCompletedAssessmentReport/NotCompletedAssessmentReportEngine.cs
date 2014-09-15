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

namespace ProCenter.Domain.ReportsModule.NotCompletedAssessmentReport
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Dapper;

    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Primitive;

    #endregion

    /// <summary>
    ///     The NotCompletedAssessmentReportEngine class.
    /// </summary>
    [ReportEngine ( ReportNames.NotCompletedAssessment )]
    public class NotCompletedAssessmentReportEngine : IReportEngine
    {
        #region Fields

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IRecentReportRepository _recentReportRepository;

        private readonly IReportTemplateRepository _reportTemplateRepository;

        private readonly IResourcesManager _resourcesManager;

        private Guid? _organizationKey;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotCompletedAssessmentReportEngine" /> class.
        /// </summary>
        /// <param name="resourcesManager">The resources manager.</param>
        /// <param name="reportTemplateRepository">The report template repository.</param>
        /// <param name="reportHistoryRepository">The report history repository.</param>
        /// <param name="dbConnectionFactory">The database connection factory.</param>
        public NotCompletedAssessmentReportEngine(
            IResourcesManager resourcesManager,
            IReportTemplateRepository reportTemplateRepository,
            IRecentReportRepository reportHistoryRepository,
            IDbConnectionFactory dbConnectionFactory )
        {
            _resourcesManager = resourcesManager;
            _reportTemplateRepository = reportTemplateRepository;
            _recentReportRepository = reportHistoryRepository;
            _connectionFactory = dbConnectionFactory;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Generates the specified key.</summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        ///     A <see cref="IReport" />.
        /// </returns>
        /// <exception cref="System.ArgumentException">Invalid parameters.</exception>
        public IReport Generate ( Guid key, string reportName, object parameters = null )
        {
            var reportParams = (NotCompletedAssessmentParameters)parameters;
            if ( reportParams == null )
            {
                return null;
            }
            _organizationKey = reportParams.OrganizationKey;
            var data = GetData ( reportParams, reportName );
            if ( data == null )
            {
                return null;
            }
            SetStrings(data, reportParams, reportName);
            var reportDataCollection = new NotCompletedAssessmentDataCollection
                                       {
                                           data,
                                       };
            var report = new NotCompletedAssessmentReport
                         {
                             DataSource = reportDataCollection,
                         };
            return report;
        }

        /// <summary>
        ///     Gets the customization model.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <returns>
        ///     A <see cref="IReportModel" />.
        /// </returns>
        public IReportModel GetCustomizationModel ( Guid key, string reportName, Guid? patientKey = null )
        {
            if ( key != Guid.Empty )
            {
                var reportTemplate = _reportTemplateRepository.GetByKey ( key );
                if ( reportTemplate != null )
                {
                    return reportTemplate.Parameters as NotCompletedAssessmentParameters;
                }
                var reporthistory = _recentReportRepository.GetByKey ( key );
                if ( reporthistory != null )
                {
                    return reporthistory.Parameters as NotCompletedAssessmentParameters;
                }
            }

            return new NotCompletedAssessmentParameters { ReportName = reportName };
        }

        /// <summary>Updates the customization model.</summary>
        /// <param name="key">The key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="name">The name.</param>
        /// <param name="shouldShow">The should show.</param>
        /// <param name="text">The text.</param>
        public void UpdateCustomizationModel ( Guid key, string reportName, string name, bool? shouldShow, string text )
        {
        }

        #endregion

        #region Methods

        private void SetStrings(NotCompletedAssessmentData data, NotCompletedAssessmentParameters reportParams, string reportName)
        {
            if ( reportParams == null || data == null )
            {
                return;
            }
            var start = reportParams.StartDate;
            var end = reportParams.EndDate;
            var dateRange = start.GetValueOrDefault().ToShortDateString() + " - " + end.GetValueOrDefault().ToShortDateString();
            if (reportParams.TimePeriod != null)
            {
                reportParams.TimePeriod.GetRange(out start, out end);
                dateRange = reportParams.TimePeriod.DisplayName;
            }
            var ageRange = _resourcesManager.GetResourceManagerByName(reportName).GetString("NA");
            if (reportParams.AgeRangeLow != null && reportParams.AgeRangeHigh != null)
            {
                ageRange = reportParams.AgeRangeLow + " - " + reportParams.AgeRangeHigh;
            }
            var gender = _resourcesManager.GetResourceManagerByName(reportName).GetString("NA");
            if (!string.IsNullOrWhiteSpace(reportParams.Gender))
            {
                gender = reportParams.Gender;
            }
            var assessment = _resourcesManager.GetResourceManagerByName(reportName).GetString("All");
            if ( !string.IsNullOrWhiteSpace ( reportParams.AssessmentName ) )
            {
                assessment = reportParams.AssessmentName;
            }
            data.AssessmentParameter = _resourcesManager.GetResourceManagerByName(reportName).GetString("Assessment") + ": " + assessment;
            data.DateRangeParameter = _resourcesManager.GetResourceManagerByName(reportName).GetString("DateRange") + ": " + dateRange;
            data.AgeGroupParameter = _resourcesManager.GetResourceManagerByName(reportName).GetString("AgeGroup") + ": " + ageRange;
            data.GenderParameter = _resourcesManager.GetResourceManagerByName(reportName).GetString("Gender") + ": " + gender;

            data.ReportName = _resourcesManager.GetResourceManagerByName(reportName).GetString("ReportName");
            data.HeaderName = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderName");
            data.HeaderAge = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderAge");
            data.HeaderGender = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderGender");
            data.HeaderAssessmentName = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderAssessmentName");
            data.HeaderAssessmentStatus = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderAssessmentStatus");
            data.HeaderViewAssessment = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderViewAssessment");
        }

        private IEnumerable<AssessmentDefinitionDto> GetAssessmentDefinitions(Guid? organizationKey)
        {
            const string Query = @"SELECT 
                                [AssessmentName]
                                ,[AssessmentCode]
                                FROM [OrganizationModule].[OrganizationAssessmentDefinition]
                                WHERE OrganizationKey = '{0}' 
                                ORDER BY [AssessmentName]";
            using (var connection = _connectionFactory.CreateConnection())
            {
                return connection.Query<AssessmentDefinitionDto>(string.Format(Query, organizationKey)).ToList();
            }
        }

        private string GetQuery(NotCompletedAssessmentParameters parameters)
        {
            const string Query = @"SELECT AssessmentInstanceKey
                                      ,[AssessmentName]
                                      ,[AssessmentCode]
                                      ,[AssessmentModule].[AssessmentInstance].[OrganizationKey]
                                      ,[AssessmentModule].[AssessmentInstance].[PatientKey]
                                      ,[PercentComplete]
                                      ,[CreatedTime]
                                      ,[LastModifiedTime]
                                      ,[IsSubmitted]
                                      ,[CanSelfAdminister]
                                      ,CASE  
                                        WHEN CreatedTime IS NULL THEN 'Not Started'
                                        ELSE 'Started'
                                      END AS AssessmentStatus
                                      ,PatientModule.Patient.PatientKey
                                      ,PatientModule.Patient.FirstName AS PatientFirstName
                                      ,PatientModule.Patient.LastName AS PatientLastName
                                      ,PatientModule.Patient.GenderCode AS PatientGender
                                      ,PatientModule.Patient.DateOfBirth
                                      ,FLOOR((CAST (GetDate() AS INTEGER) - CAST(DateOfBirth AS INTEGER)) / 365.25) AS PatientAge
                                  FROM PatientModule.Patient
                                  LEFT OUTER JOIN [AssessmentModule].[AssessmentInstance] ON PatientModule.Patient.PatientKey = [AssessmentModule].[AssessmentInstance].PatientKey
                                  {0}
                                  {1}
                                  ORDER BY Patient.LastName, Patient.FirstName";
            var and = string.Empty;
            var where = " WHERE ";
            const string AgeWhereString = @" FLOOR((CAST (GetDate() AS INTEGER) - CAST(DateOfBirth AS INTEGER)) / 365.25) >= {0} 
                                             AND FLOOR((CAST (GetDate() AS INTEGER) - CAST(DateOfBirth AS INTEGER)) / 365.25) <= {1}";
            var ageWhere = string.Empty;
            if (parameters.AgeRangeLow != null && parameters.AgeRangeHigh != null)
            {
                ageWhere = where + string.Format(AgeWhereString, parameters.AgeRangeLow, parameters.AgeRangeHigh);
                and = " AND ";
                where = string.Empty;
            }
            var genderWhereString = and + " PatientModule.Patient.GenderCode = '{0}'";
            var genderWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(parameters.Gender))
            {
                genderWhere = where + string.Format(genderWhereString, parameters.Gender);
            }
            var finalQuery = string.Format(Query, genderWhere, ageWhere);
            return finalQuery;
        }

        private List<Assessment> GetListOfAssessments(NotCompletedAssessmentParameters parameters)
        {
            var assessmentDefinitions = GetAssessmentDefinitions(_organizationKey);
            var assessments = assessmentDefinitions.Select(assessmentDefintion => new Assessment
            {
                AssessmentCode = assessmentDefintion.AssessmentCode,
                AssessmentName = assessmentDefintion.AssessmentName,
            }).ToList();
            if (parameters.AssessmentDefinitionCode != null)
            {
                assessments = assessments.Where(a => a.AssessmentCode == parameters.AssessmentDefinitionCode).ToList();
            }
            return assessments;
        }

        private void GetParameterStartEndDate(NotCompletedAssessmentParameters parameters, out DateTime? startDate, out DateTime? endDate)
        {
            startDate = parameters.StartDate;
            endDate = parameters.EndDate;
            if (parameters.TimePeriod != null)
            {
                parameters.TimePeriod.GetRange(out startDate, out endDate);
                if ( startDate != null )
                {
                    startDate = DateTime.Parse(((DateTime)startDate).ToShortDateString());
                }
                if ( endDate != null )
                {
                    endDate = DateTime.Parse(((DateTime)endDate).ToShortDateString());
                }
            }
        }

        private NotCompletedAssessmentData AddNotStartedAssessmentsForPatient(
            NotCompletedAssessmentParameters parameters,
            List<NotCompletedAssessmentDto> notCompletedAssessmentDtos,
            PatientDto patientDto, 
            IEnumerable<Assessment> assessments, 
            string reportName)
        {
            DateTime? startDate;
            DateTime? endDate;
            GetParameterStartEndDate (parameters, out startDate, out endDate);
            var ncData = new NotCompletedAssessmentData { Data = new List<NotCompletedAssessmentDataObject>() };
            foreach (var data in assessments)
            {
                var assessmentNameNotComplete = _resourcesManager.GetResourceManagerByName(
                                                data.AssessmentName).GetString(
                                                "_" + data.AssessmentCode); 
                Guid? assessmentInstanceKey = null;
                var notCompletedAssessmentDto = notCompletedAssessmentDtos.Where (a => a.AssessmentCode == data.AssessmentCode).ToList ();
                var status = _resourcesManager.GetResourceManagerByName(reportName).GetString("NotStarted");
                var process = true;
                foreach ( var notCompleted in notCompletedAssessmentDto )
                {
                    var createdTime = DateTime.Parse(notCompleted.CreatedTime.ToShortDateString () );
                    process = true;
                    if (notCompleted.AssessmentInstanceKey != null)
                    {
                        if (notCompleted.IsSubmitted == false && startDate != null && endDate != null
                             && createdTime >= startDate && createdTime <= endDate)
                        {
                            assessmentInstanceKey = notCompleted.AssessmentInstanceKey;
                            status = _resourcesManager.GetResourceManagerByName ( reportName ).GetString ( "Started" );
                            break;
                        }
                        if (notCompleted.IsSubmitted && startDate != null && endDate != null
                            && createdTime >= startDate && createdTime <= endDate)
                        {
                            process = false;
                        }
                    }
                }
                if ( !process )
                {
                    continue;
                }
                var assessment = new NotCompletedAssessmentDataObject
                        {
                            Age = patientDto.PatientAge,
                            AssessmentInstanceKey = assessmentInstanceKey,
                            AssessmentName = assessmentNameNotComplete,
                            Gender = patientDto.Gender.Name,
                            PatientFirstName = patientDto.Name.FirstName,
                            PatientLastName = patientDto.Name.LastName,
                            PatientKey = new Guid(patientDto.UniqueIdentifier),
                            PatientName = patientDto.Name.FirstName + " " + patientDto.Name.LastName,
                            AssessmentStatus = status
                        };

                ncData.Data.Add(assessment);
            }
            return ncData;
        }

        private NotCompletedAssessmentData GetData(NotCompletedAssessmentParameters parameters, string reportName)
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                var assessmentsNotCompleted = connection.Query<NotCompletedAssessmentDto>(GetQuery(parameters)).ToList();
                if (!assessmentsNotCompleted.Any())
                {
                    return null;
                }
                var assessments = GetListOfAssessments(parameters);
                var ncData = new NotCompletedAssessmentData { Data = new List<NotCompletedAssessmentDataObject> () };
                var patientKey = new Guid();
                
                foreach ( var assessmentNotComplete in assessmentsNotCompleted )
                {
                    if ( patientKey == assessmentNotComplete.PatientKey )
                    {
                        continue;
                    }
                    patientKey = assessmentNotComplete.PatientKey;
                    var key = patientKey;
                    var rows = assessmentsNotCompleted.Where(a => a.PatientKey == key).ToList ();

                    var patientDto = new PatientDto
                                            {
                                                Name = new PersonName(assessmentNotComplete.PatientFirstName, assessmentNotComplete.PatientLastName),
                                                Gender = new LookupDto { Name = assessmentNotComplete.PatientGender },
                                                UniqueIdentifier = assessmentNotComplete.PatientKey.ToString(),
                                                PatientAge = assessmentNotComplete.PatientAge
                                            };
                    ncData.Data.AddRange(AddNotStartedAssessmentsForPatient(
                        parameters,
                        rows,
                        patientDto, 
                        assessments, 
                        reportName).Data);
                }
                return ncData.Data.Any () ? ncData : null;
            }
        }
        #endregion
    }
}