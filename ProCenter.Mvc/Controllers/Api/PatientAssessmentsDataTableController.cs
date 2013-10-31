namespace ProCenter.Mvc.Controllers.Api
{
    #region Using Statements

    using System;
    using System.Linq;
    using System.Web.Http;
    using Common;
    using Dapper;
    using Infrastructure;
    using Models;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using Service.Message.Assessment;

    #endregion

    public class PatientAssessmentsDataTableController : BaseApiController
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IResourcesManager _resourcesManager;

        public PatientAssessmentsDataTableController(IDbConnectionFactory connectionFactory, IResourcesManager resourcesManager)
        {
            _connectionFactory = connectionFactory;
            _resourcesManager = resourcesManager;
        }

        [HttpGet]
        public DataTableResponse<AssessmentSummaryDto> Get(Guid key, string sEcho, int iDisplayStart, int iDisplayLength, string sSearch = null)
        {
            const string whereSearchConstraint = " AND (AssessmentName LIKE @search+'%')";
            var selfAdminConstraint = "AND CanSelfAdminister=1";
            const string query = @"
                             SELECT COUNT(*) as TotalCount FROM AssessmentModule.AssessmentInstance
                                 WHERE OrganizationKey=@OrganizationKey AND PatientKey=@PatientKey {0} {1}
                             SELECT [t].*                                    
                             FROM ( 
                                 SELECT ROW_NUMBER() OVER ( 
                                    ORDER BY [t1].CreatedTime DESC) AS [ROW_NUMBER],   
                                             [t1].AssessmentInstanceKey,
                                             [t1].AssessmentName, 
                                             [t1].AssessmentCode,
                                             [t1].PercentComplete,  
                                             [t1].CreatedTime,  
                                             [t1].IsSubmitted,  
                                             [t1].PatientKey   
                                 FROM AssessmentModule.AssessmentInstance AS [t1]
                                 WHERE PatientKey=@PatientKey {0} {1}
                                 ) AS [t] 
                             WHERE [t].[ROW_NUMBER] BETWEEN @start + 1 AND @end 
                             ORDER BY [t].[ROW_NUMBER] ";

            if (UserContext.Current.PatientKey != null && key != UserContext.Current.PatientKey)
            {
                return new DataTableResponse<AssessmentSummaryDto>
                {
                    Data = Enumerable.Empty<AssessmentSummaryDto>(),
                    Echo = sEcho,
                    TotalDisplayRecords = 0,
                    TotalRecords = 0,
                };
            }

            if (UserContext.Current.PatientKey == null)
            {
                selfAdminConstraint = string.Empty;
            }

            var start = iDisplayStart;
            var end = start + iDisplayLength;

            using(var connection = _connectionFactory.CreateConnection())
            using (var multiQuery = connection.QueryMultiple(string.Format ( query, sSearch == null ? "" : whereSearchConstraint, selfAdminConstraint ), new { start, end, PatientKey = key, search = sSearch, UserContext.Current.OrganizationKey }))
            {
                var totalCount = multiQuery.Read<int>().Single();
                var assessmentSummaryDto = multiQuery.Read<AssessmentSummaryDto>().ToList();

                var dataTableResponse = new DataTableResponse<AssessmentSummaryDto>
                    {
                        Data = assessmentSummaryDto, 
                        Echo = sEcho,
                        TotalDisplayRecords = totalCount,
                        TotalRecords = totalCount,
                    };

                foreach (var data in dataTableResponse.Data)
                {
                    data.AssessmentName = _resourcesManager.GetResourceManagerByName ( data.AssessmentName ).GetString ( SharedStringNames.ResourceKeyPrefix + data.AssessmentCode );
                }

                return dataTableResponse;
            }
        }
    }
}