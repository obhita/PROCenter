namespace ProCenter.Mvc.Controllers.Api
{
    #region Using Statements

    using System.Data;
    using System.Linq;
    using Common;
    using Dapper;
    using Infrastructure;
    using Models;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using Service.Message.Assessment;

    #endregion

    public class AllAssessmentsDataTableController : BaseApiController
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IResourcesManager _resourcesManager;

        public AllAssessmentsDataTableController(IDbConnectionFactory connectionFactory, IResourcesManager resourcesManager)
        {
            _connectionFactory = connectionFactory;
            _resourcesManager = resourcesManager;
        }

        public DataTableResponse<AssessmentSummaryDto> Get(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch = null)
        {
            const string whereConstraint = "WHERE [p1].OrganizationKey = @OrganizationKey AND ( [p1].FirstName LIKE @search+'%' OR [p1].LastName LIKE @search+'%')";
            const string query = @"
                             SELECT COUNT(*) as TotalCount FROM AssessmentModule.AssessmentInstance
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
                                             [t1].PatientKey,
                                             [p1].FirstName AS PatientFirstName,
                                             [p1].LastName AS PatientLastName
                                 FROM AssessmentModule.AssessmentInstance AS [t1]
                                 JOIN PatientModule.Patient AS [p1]
                                 ON t1.PatientKey=p1.PatientKey
                                 {0}
                                 ) AS [t] 
                             WHERE [t].[ROW_NUMBER] BETWEEN @start + 1 AND @end 
                             ORDER BY [t].[ROW_NUMBER] ";

            var start = iDisplayStart;
            var end = start + iDisplayLength;
            var replaceString = string.IsNullOrWhiteSpace(sSearch) ? "" : whereConstraint;
            var completeQuery = string.Format(query, replaceString);

            using (var conn = _connectionFactory.CreateConnection())
            using (var multiQuery = conn.QueryMultiple(completeQuery, new { start, end, search = sSearch, UserContext.Current.OrganizationKey }))
            {
                var totalCount = multiQuery.Read<int>().Single();
                var assessmentSummaryDtos = multiQuery.Read<AssessmentSummaryDto>().ToList();
                foreach (var assessmentSummaryDto in assessmentSummaryDtos)
                {
                    assessmentSummaryDto.AssessmentName =
                        _resourcesManager.GetResourceManagerByName(assessmentSummaryDto.AssessmentName).GetString(SharedStringNames.ResourceKeyPrefix + assessmentSummaryDto.AssessmentCode);
                }
                var dataTableResponse = new DataTableResponse<AssessmentSummaryDto>
                    {
                        Data = assessmentSummaryDtos,
                        Echo = sEcho,
                        TotalDisplayRecords = totalCount,
                        TotalRecords = totalCount,
                    };

                return dataTableResponse;
            }
        }
    }
}