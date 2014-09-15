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

namespace ProCenter.Domain.ReportsModule.PatientScoreRangeReport
{
    #region Using Statements

    using System;
    using System.Linq;

    using Dapper;

    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>
    ///     The PatientScoreRangeReportEngine class.
    /// </summary>
    [ReportEngine ( ReportNames.PatientScoreRange )]
    public class PatientScoreRangeReportEngine : IReportEngine
    {
        #region Fields

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IRecentReportRepository _recentReportRepository;

        private readonly IReportTemplateRepository _reportTemplateRepository;

        private readonly IResourcesManager _resourcesManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientScoreRangeReportEngine" /> class.
        /// </summary>
        /// <param name="resourcesManager">The resources manager.</param>
        /// <param name="reportTemplateRepository">The report template repository.</param>
        /// <param name="reportHistoryRepository">The report history repository.</param>
        /// <param name="dbConnectionFactory">The database connection factory.</param>
        public PatientScoreRangeReportEngine (
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
            var reportParams = (PatientScoreRangeParameters)parameters;
            var data = GetData ( reportParams );
            if ( data == null )
            {
                return null;
            }
            SetStrings(data, reportParams, reportName);
            var reportDataCollection = new PatientScoreRangeDataCollection
                                       {
                                           data,
                                       };
            var report = new PatientScoreRangeReport
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
                    return reportTemplate.Parameters as PatientScoreRangeParameters;
                }
                var reporthistory = _recentReportRepository.GetByKey ( key );
                if ( reporthistory != null )
                {
                    return reporthistory.Parameters as PatientScoreRangeParameters;
                }
            }

            return new PatientScoreRangeParameters { ReportName = reportName };
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

        private void SetStrings(PatientScoreRangeData data, PatientScoreRangeParameters reportParams, string reportName)
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
            data.AssessmentParameter = _resourcesManager.GetResourceManagerByName(reportName).GetString("Assessment") + ": " + reportParams.AssessmentName;
            data.DateRangeParameter = _resourcesManager.GetResourceManagerByName(reportName).GetString("DateRange") + ": " + dateRange;
            data.RiskCategoryParameter = _resourcesManager.GetResourceManagerByName(reportName).GetString("RiskCategory") + ": " + GetScoreTypeValue(reportParams);
            data.AgeGroupParameter = _resourcesManager.GetResourceManagerByName(reportName).GetString("AgeGroup") + ": " + ageRange;
            data.GenderParameter = _resourcesManager.GetResourceManagerByName(reportName).GetString("Gender") + ": " + gender;

            data.ReportName = _resourcesManager.GetResourceManagerByName(reportName).GetString("ReportName");
            data.HeaderName = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderName");
            data.HeaderAge = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderAge");
            data.HeaderGender = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderGender");
            data.HeaderScore = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderScore");
            data.HeaderChart = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderChart");
            data.HeaderChange = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderChange");
            data.HeaderDate = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderDate");
            data.HeaderViewAssessment = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderViewAssessment");
        }

        private string GetQuery ( PatientScoreRangeParameters parameters )
        {
            const string Query = @"SELECT [AssessmentInstanceKey]
                              ,[PatientKey]
                              ,[AssessmentName]
                              ,[AssessmentScore]
                              ,[ScoreDate]
                              ,[PatientBirthDate]
                              ,[PatientFirstName]
                              ,[PatientLastName]
                              ,[PatientGender]
                              ,[ScoreChange]
                              ,FLOOR((CAST (GetDate() AS INTEGER) - CAST(PatientBirthDate AS INTEGER)) / 365.25) AS PatientAge
                          FROM [ReportModule].[PatientScoreRangeReport]
                          WHERE AssessmentCode = '{0}'
                          {1}
                          {2}
                          {3}
                          {4}
                          ORDER BY PatientFirstName, PatientLastName, ScoreDate DESC";

            const string ScoreDateWhereString = " AND (ScoreDate >= '{0}' AND ScoreDate <= DATEADD(day,1, '{1}'))";
            var startDate = parameters.StartDate;
            var endDate = parameters.EndDate;
            if (parameters.TimePeriod != null)
            {
                parameters.TimePeriod.GetRange(out startDate, out endDate);
            }
            var scoreDateWhere = string.Format(ScoreDateWhereString, startDate.GetValueOrDefault().ToShortDateString(), endDate.GetValueOrDefault().ToShortDateString());
            const string AgeWhereString = @"AND FLOOR((CAST (GetDate() AS INTEGER) - CAST(PatientBirthDate AS INTEGER)) / 365.25) >= {0} 
                                              AND FLOOR((CAST (GetDate() AS INTEGER) - CAST(PatientBirthDate AS INTEGER)) / 365.25) <= {1}";
            var ageWhere = string.Empty;
            if (parameters.AgeRangeLow != null && parameters.AgeRangeHigh != null)
            {
                ageWhere = string.Format(AgeWhereString, parameters.AgeRangeLow, parameters.AgeRangeHigh);
            }
            const string GenderWhereString = "AND PatientGender = '{0}'";
            var genderWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(parameters.Gender))
            {
                genderWhere = string.Format(GenderWhereString, parameters.Gender);
            }
            var assessmentScoreWhere = GetScoreTypeParameterString(parameters);
            var finalQuery = string.Format(Query, parameters.AssessmentDefinitionCode, assessmentScoreWhere, scoreDateWhere, ageWhere, genderWhere);
            return finalQuery;
        }

        private PatientScoreRangeData GetData ( PatientScoreRangeParameters parameters )
        {
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                var reportDtos = connection.Query<PatientScoreRangeDto>(GetQuery(parameters)).ToList();
                if ( !reportDtos.Any () )
                {
                    return null;
                }
                var returnData = new PatientScoreRangeData ();
                var data = reportDtos.Select (
                    reportDto => new PatientScoreRangeDataObject
                                 {
                                     Age = reportDto.PatientAge,
                                     AssessmentDate = reportDto.ScoreDate.ToShortDateString (),
                                     Change = reportDto.ScoreChange,
                                     Gender = reportDto.PatientGender,
                                     PatientName = reportDto.PatientFirstName + " " + reportDto.PatientLastName,
                                     Score = reportDto.AssessmentScore,
                                     PatientKey = reportDto.PatientKey,
                                     AssessmentInstanceKey = reportDto.AssessmentInstanceKey,
                                     AssessmentName = reportDto.AssessmentName
                                 } ).ToList ();
                returnData.Data = data;
                return returnData;
            }
        }

        private string GetScoreTypeParameterString ( PatientScoreRangeParameters parameters )
        {
            const string AssessmentScoreIntWhereString = " AND AssessmentScore >= {0} AND AssessmentScore <= {1}";
            const string AssessmentScoreBoolWhereString = " AND AssessmentScore = '{0}'";

            var assessmentScoreWhere = string.Empty;
            if ( parameters.ScoreType.GetType () == typeof(ScoreTypeInt) )
            {
                var scoreInt = (ScoreTypeInt)parameters.ScoreType;
                assessmentScoreWhere = string.Format ( AssessmentScoreIntWhereString, scoreInt.RiskStart, scoreInt.RiskEnd );
            }
            else if ( parameters.ScoreType.GetType () == typeof(ScoreTypeBoolean) )
            {
                var scoreBool = (ScoreTypeBoolean)parameters.ScoreType;
                assessmentScoreWhere = string.Format ( AssessmentScoreBoolWhereString, scoreBool.IsAtRisk );
            }
            return assessmentScoreWhere;
        }

        private string GetScoreTypeValue ( PatientScoreRangeParameters parameters )
        {
            var returnValue = string.Empty;
            if ( parameters.ScoreType.GetType () == typeof(ScoreTypeInt) )
            {
                const string ReturnIntValue = "{0} to {1}";
                var scoreInt = (ScoreTypeInt)parameters.ScoreType;
                returnValue = string.Format ( ReturnIntValue, scoreInt.RiskStart, scoreInt.RiskEnd );
            }
            if ( parameters.ScoreType.GetType () == typeof(ScoreTypeBoolean) )
            {
                var returnIntValue = PatientScoreRange.IsAtRisk + "{0}";
                var scoreBool = (ScoreTypeBoolean)parameters.ScoreType;
                returnValue = string.Format ( returnIntValue, scoreBool.IsAtRisk );
            }
            return returnValue;
        }

        #endregion
    }
}