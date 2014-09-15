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

namespace ProCenter.Mvc.Controllers.Api
{
    #region Using Statements

    using System;
    using System.Linq;
    using System.Text;
    using System.Web.Http;

    using Agatha.Common;

    using Dapper;

    using ProCenter.Common;
    using ProCenter.Mvc.Infrastructure;
    using ProCenter.Mvc.Models;
    using ProCenter.Service.Message.Common.Lookups;
    using ProCenter.Service.Message.Report;

    #endregion

    /// <summary>The report controller class.</summary>
    public class ReportController : BaseApiController
    {
        #region Fields

        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IResourcesManager _resourcesManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReportController" /> class.
        /// </summary>
        /// <param name="requestDispatcherFactory">The request dispatcher factory.</param>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="resourcesManager">The resources manager.</param>
        public ReportController ( IRequestDispatcherFactory requestDispatcherFactory, IDbConnectionFactory connectionFactory, IResourcesManager resourcesManager )
            : base ( requestDispatcherFactory )
        {
            _connectionFactory = connectionFactory;
            _resourcesManager = resourcesManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the missed reminders for data table.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <param name="systemAccountKey">The system account key.</param>
        /// <param name="organizationKey">The organization key.</param>
        /// <param name="showAll">
        ///     If set to <c>true</c> [show all].
        /// </param>
        /// <returns>
        ///     Returns DataTableResponse of type MissedRemindersReportDto.
        /// </returns>
        [HttpGet]
        public DataTableResponse<MissedRemindersReportDto> GetMissedRemindersForDataTable (
            string sEcho,
            int iDisplayStart,
            int iDisplayLength,
            string sSearch = null,
            string systemAccountKey = null,
            string organizationKey = null,
            bool showAll = true )
        {
            const string MissedRemindersQuery = @"SELECT 
                                          [RecurrenceKey],
                                          [AssessmentReminderKey],
                                          [OrganizationKey],
                                          [PatientKey],
                                          [PatientFirstName],
                                          [PatientLastName],
                                          [PatientFirstName] + ' ' + [PatientLastName] AS PatientName,
                                          [AssessmentDefinitionKey],
                                          [AssessmentName],
                                          [AssessmentCode],
                                          [Title] AS Description,
                                          [AssessmentInstanceKey],
                                          [Start] AS ReminderStartDate
                                          FROM [MessageModule].[AssessmentReminder]
                                          WHERE 
                                          AssessmentInstanceKey IS NULL
                                          AND Status = 'Default'
                                          AND Start < @today
                                          {0}
                                          {1}
                                          {2}";

            using ( var connection = _connectionFactory.CreateConnection () )
            {
                const string WhereConstraint = " AND ([PatientFirstName] + ' ' + [PatientLastName] LIKE '%' + replace(@search, ' ', '') + " +
                                               "'%' OR AssessmentName LIKE '%' + @search + " +
                                               "'%' OR Title LIKE '%' + @search + '%')";
                var whereSystemAccountKey = string.IsNullOrWhiteSpace ( systemAccountKey ) || showAll ? string.Empty : " AND SystemAccountKey='" + systemAccountKey + "'";
                var whereOrganizationKey = string.IsNullOrWhiteSpace ( organizationKey ) ? string.Empty : " AND OrganizationKey='" + organizationKey + "'";
                var whereString = string.IsNullOrWhiteSpace ( sSearch ) ? string.Empty : WhereConstraint;
                var completeQuery = string.Format ( MissedRemindersQuery, whereOrganizationKey, whereSystemAccountKey, whereString );

                var missedRemindersReportDtos = connection.Query<MissedRemindersReportDto> ( completeQuery, new { search = sSearch, DateTime.Today } ).ToList ();
                foreach ( var missedReminder in missedRemindersReportDtos )
                {
                    missedReminder.AssessmentName = _resourcesManager.GetResourceManagerByName ( missedReminder.AssessmentName )
                                                                     .GetString ( SharedStringNames.ResourceKeyPrefix + missedReminder.AssessmentCode );
                }
                var dataTableResponse = new DataTableResponse<MissedRemindersReportDto>
                                            {
                                                Data = missedRemindersReportDtos,
                                                Echo = sEcho,
                                                TotalDisplayRecords = missedRemindersReportDtos.Count,
                                                TotalRecords = missedRemindersReportDtos.Count,
                                            };
                return dataTableResponse;
            }
        }

        /// <summary>
        ///     Recents the reports.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <param name="systemAccountKey">The system account key.</param>
        /// <returns>
        ///     Return a DataTableResponse of type RecentReportsDto.
        /// </returns>
        [HttpGet]
        public DataTableResponse<RecentReportsDto> GetRecentReportsForDataTable (
            string sEcho,
            int iDisplayStart,
            int iDisplayLength,
            string sSearch = null,
            Guid? systemAccountKey = null )
        {
            const string RecentReportsQuery = @"SELECT 
                                             [ReportKey],
                                             [SystemAccountKey],
                                             [Name],
                                             [Name] AS 'ReportTypeName',
                                             [Assessment],
                                             [RunDate]
                                             FROM [OrganizationModule].[RecentReports] 
                                             WHERE SystemAccountKey=@SystemAccountKey
                                             AND PatientKey IS NULL
                                             {0}
                                             ORDER BY [RunDate] DESC";
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                const string WhereConstraint = " AND (Name LIKE '%' + replace(@search, ' ', '') + '%' OR Assessment LIKE '%' + @search + '%')";
                var replaceString = string.IsNullOrWhiteSpace ( sSearch ) ? string.Empty : WhereConstraint;
                var completeQuery = string.Format ( RecentReportsQuery, replaceString );
                var recentReportsDtos = connection.Query<RecentReportsDto> ( completeQuery, new { search = sSearch, SystemAccountKey = systemAccountKey } ).ToList ();
                foreach ( var recentReport in recentReportsDtos )
                {
                    recentReport.ReportTypeName = recentReport.Name;
                    recentReport.Name = _resourcesManager.GetResourceManagerByName ( recentReport.Name ).GetString ( "ReportName" );
                }
                var dataTableResponse = new DataTableResponse<RecentReportsDto>
                                            {
                                                Data = recentReportsDtos,
                                                Echo = sEcho,
                                                TotalDisplayRecords = recentReportsDtos.Count,
                                                TotalRecords = recentReportsDtos.Count,
                                            };
                return dataTableResponse;
            }
        }

        /// <summary>
        ///     Gets the report templates.
        /// </summary>
        /// <param name="sEcho">The s echo.</param>
        /// <param name="iDisplayStart">The i display start.</param>
        /// <param name="iDisplayLength">Display length of the i.</param>
        /// <param name="sSearch">The s search.</param>
        /// <param name="systemAccountKey">The system account key.</param>
        /// <param name="organizationKey">The organization key.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <param name="showAll">
        ///     If set to <c>true</c> [show all].
        /// </param>
        /// <returns>
        ///     Returns a DataTableResponse of type ReportTemplateDto.
        /// </returns>
        [HttpGet]
        public DataTableResponse<ReportTemplateDto> GetReportTemplatesForDataTable (
            string sEcho,
            int iDisplayStart,
            int iDisplayLength,
            string sSearch = null,
            string systemAccountKey = null,
            string organizationKey = null,
            string reportType = null,
            bool showAll = true )
        {
            var start = iDisplayStart;
            var end = start + iDisplayLength;

            const string ReportTemplateQuery = @"SELECT COUNT(*) as TotalCount FROM ReportModule.ReportTemplate
                                            WHERE ReportType = @ReportType
                                            AND PatientKey IS NULL
                                            {0}
                                            {1}
                                            {2}                    
                                            SELECT [t].ReportStateCode,
                                                [t].ReportTemplateKey AS 'Key',
                                                [t].SystemAccountKey,
                                                [t].Name,
                                                [t].Name AS 'ReportTypeName',
                                                [t].Parameters, 
                                                [t].ReportType
                                            FROM ( 
                                                SELECT ROW_NUMBER() OVER ( 
                                                ORDER BY [t1].Name) AS [ROW_NUMBER],   
                                                            [t1].ReportStateCode,
                                                            [t1].ReportTemplateKey,
                                                            [t1].SystemAccountKey,
                                                            [t1].Name, 
                                                            [t1].Name AS 'ReportTypeName',
                                                            [t1].Parameters,
                                                            [t1].ReportType
                                                FROM ReportModule.ReportTemplate AS [t1]
                                                WHERE ReportType = @ReportType
                                                AND PatientKey IS NULL
                                                {0}
                                                {1}
                                                {2}
                                                ) AS [t] 
                                            WHERE [t].[ROW_NUMBER] BETWEEN @start + 1 AND @end 
                                            ORDER BY [t].[ROW_NUMBER] ";

            const string WhereConstraint = " AND (Name + Parameters LIKE '%' + replace(@search, ' ', '') + '%')";
            var whereOrganizationKey = " AND OrganizationKey='" + organizationKey + "'";
            var whereSystemAccountKey = string.IsNullOrWhiteSpace ( systemAccountKey ) || showAll ? string.Empty : " AND SystemAccountKey='" + systemAccountKey + "'";
            var replaceString = string.IsNullOrWhiteSpace ( sSearch ) ? string.Empty : WhereConstraint;
            var completeQuery = string.Format ( ReportTemplateQuery, whereOrganizationKey, whereSystemAccountKey, replaceString );

            using ( var connection = _connectionFactory.CreateConnection () )
            using ( var multiQuery = connection.QueryMultiple ( completeQuery, new { start, end, search = sSearch, reportType, systemAccountKey } ) )
            {
                var totalCount = multiQuery.Read<int> ().Single ();
                var reportTemplateDtos =
                    multiQuery.Read<string, ReportTemplateDto, ReportTemplateDto> (
                        ( reportStateCode, reportTemplateDto ) =>
                            {
                                var lookupDto = new LookupDto
                                                    {
                                                        Code = reportStateCode,
                                                        Name = _resourcesManager.GetResourceManagerByName ( "ReportState" ).GetString ( reportStateCode ),
                                                    };
                                reportTemplateDto.ReportState = lookupDto;
                                var reportName = _resourcesManager.GetResourceManagerByName ( reportTemplateDto.Name ).GetString ( "ReportName" );
                                reportTemplateDto.ReportTypeName = reportTemplateDto.Name;
                                reportTemplateDto.NameParameters = reportName + " " + reportTemplateDto.Parameters;
                                return reportTemplateDto;
                            },
                        "Key" ).ToList ();

                var dataTableResponse = new DataTableResponse<ReportTemplateDto>
                                            {
                                                Data = reportTemplateDtos,
                                                Echo = sEcho,
                                                TotalDisplayRecords = totalCount,
                                                TotalRecords = totalCount,
                                            };
                return dataTableResponse;
            }
        }

        /// <summary>
        ///     Reports the template search.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <param name="search">The search.</param>
        /// <param name="systemAccountKey">The system account key.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <returns>
        ///     A <see cref="FinderResults{ReportTemplateDto}" />.
        /// </returns>
        [HttpGet]
        public FinderResults<ReportTemplateDto> ReportTemplateSearch (
            int page,
            int pageSize,
            string reportType,
            string search = null,
            Guid? systemAccountKey = null,
            Guid? patientKey = null )
        {
            var whereConstraintBuilder = new StringBuilder ();
            if ( !string.IsNullOrWhiteSpace ( search ) )
            {
                whereConstraintBuilder.Append ( " AND (Name LIKE @search+'%')" );
            }

            if ( systemAccountKey != null )
            {
                whereConstraintBuilder.Append ( " AND SystemAccountKey= @systemAccountKey " );
            }

            if ( patientKey != null )
            {
                whereConstraintBuilder.Append ( " AND PatientKey= @patientKey " );
            }
            else
            {
                whereConstraintBuilder.Append(" AND PatientKey IS NULL ");
            }

            const string Query = @"
                             SELECT COUNT(*) as TotalCount FROM ReportModule.ReportTemplate
                                 WHERE ReportType = @ReportType {0}
                             SELECT [t].ReportStateCode,
                                    [t].ReportTemplateKey AS 'Key',
                                    [t].Name, 
                                    [t].Parameters, 
                                    [t].ReportType
                             FROM ( 
                                 SELECT ROW_NUMBER() OVER ( 
                                    ORDER BY [t1].Name) AS [ROW_NUMBER],   
                                             [t1].ReportStateCode,
                                             [t1].ReportTemplateKey,
                                             [t1].Name,
                                             [t1].Parameters,
                                             [t1].ReportType
                                 FROM ReportModule.ReportTemplate AS [t1]
                                 WHERE ReportType = @ReportType {0} 
                                 ) AS [t] 
                             WHERE [t].[ROW_NUMBER] BETWEEN @start + 1 AND @end 
                             ORDER BY [t].[ROW_NUMBER] ";

            var start = page * pageSize;
            var end = start + pageSize;
            var completeQuery = string.Format ( Query, whereConstraintBuilder );

            using ( var connection = _connectionFactory.CreateConnection () )
            using ( var multiQuery = connection.QueryMultiple ( completeQuery, new { start, end, reportType, search, patientKey, systemAccountKey } ) )
            {
                var totalCount = multiQuery.Read<int> ().Single ();
                var reportTemplateDtos =
                    multiQuery.Read<string, ReportTemplateDto, ReportTemplateDto> (
                        ( reportStateCode, reportTemplateDto ) =>
                            {
                                var lookupDto = new LookupDto
                                                    {
                                                        Code = reportStateCode,
                                                        Name = _resourcesManager.GetResourceManagerByName ( "ReportState" ).GetString ( reportStateCode ),
                                                    };
                                reportTemplateDto.ReportState = lookupDto;
                                reportTemplateDto.ReportDisplayName = _resourcesManager.GetResourceManagerByName ( reportTemplateDto.Name ).GetString ( "ReportName" );
                                return reportTemplateDto;
                            },
                        "Key" ).ToList ();

                var finderResults = new FinderResults<ReportTemplateDto>
                                        {
                                            Data = reportTemplateDtos,
                                            TotalCount = totalCount
                                        };

                return finderResults;
            }
        }

        /// <summary>
        /// Reports the type search.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="isPatientCentric">The is patient centric.</param>
        /// <param name="search">The search.</param>
        /// <returns>
        /// A <see cref="FinderResults{ReportTypeDto}" />.
        /// </returns>
        [HttpGet]
        public FinderResults<ReportDefinitionDto> ReportTypeSearch ( int page, int pageSize, bool? isPatientCentric, string search = null )
        {
            var whereConstraintBuilder = new StringBuilder ();
            if ( isPatientCentric.HasValue )
            {
                whereConstraintBuilder.AppendFormat ( " AND [IsPatientCentric] = '{0}'", isPatientCentric.Value );
            }

            const string Query = @"SELECT 
                                [ReportDefinitionKey],
                                [ReportName],
                                [DisplayName],
                                [IsPatientCentric]
                                FROM [ReportModule].[ReportDefinition] 
                                WHERE 1=1 {0}
                                ORDER BY ReportName";
            var completeQuery = string.Format ( Query, whereConstraintBuilder );

            using ( var connection = _connectionFactory.CreateConnection () )
            {
                var reportDefinitionDtos = connection.Query<ReportDefinitionDto> ( completeQuery ).ToList ();
                var finderResults = new FinderResults<ReportDefinitionDto>
                                        {
                                            Data = reportDefinitionDtos,
                                            TotalCount = reportDefinitionDtos.Count
                                        };
                return finderResults;
            }
        }

        #endregion
    }
}