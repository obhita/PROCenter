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
namespace ProCenter.Mvc.Controllers.Api
{
    #region

    using System.Linq;
    using System.Web.Http;
    using Common;
    using Dapper;
    using Infrastructure;
    using Models;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using Service.Message.Assessment;

    #endregion

    public class OrganizationController : BaseApiController
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IResourcesManager _resourcesManager;
        const string WhereConstraintActive = " AND (AssessmentName LIKE @search+'%')";
            const string QueryActive = @"
                             SELECT COUNT(*) as TotalCount FROM OrganizationModule.OrganizationAssessmentDefinition
                                 WHERE OrganizationKey=@OrganizationKey{0}
                             SELECT [t].AssessmentName,
                                    [t].AssessmentDefinitionKey AS 'Key',
                                    [t].AssessmentCode                                      
                             FROM ( 
                                 SELECT ROW_NUMBER() OVER ( 
                                    ORDER BY [t1].AssessmentName) AS [ROW_NUMBER],   
                                             [t1].AssessmentName,
                                             [t1].AssessmentDefinitionKey,
                                             [t1].AssessmentCode  
                                 FROM OrganizationModule.OrganizationAssessmentDefinition AS [t1]
                                 WHERE OrganizationKey=@OrganizationKey{0}
                                 ) AS [t] 
                             WHERE [t].[ROW_NUMBER] BETWEEN @start + 1 AND @end 
                             ORDER BY [t].[ROW_NUMBER] ";


        public OrganizationController(IDbConnectionFactory connectionFactory, IResourcesManager resourcesManager)
        {
            _connectionFactory = connectionFactory;
            _resourcesManager = resourcesManager;
        }

        [HttpGet]
        public DataTableResponse<AssessmentDefinitionDto> Get(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch = null)
        {
            var start = iDisplayStart;
            var end = start + iDisplayLength;
            var replaceString = string.IsNullOrWhiteSpace(sSearch) ? "" : WhereConstraintActive;
            var completeQuery = string.Format(QueryActive, replaceString);

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var multiQuery = connection.QueryMultiple(completeQuery, new {start, end, search = sSearch, UserContext.Current.OrganizationKey}))
                {
                    var totalCount = multiQuery.Read<int>().Single();
                    var assessmentDefinitionDtos = multiQuery.Read<AssessmentDefinitionDto>().ToList();
                    foreach (var assessmentDefinitionDto in assessmentDefinitionDtos)
                    {
                        assessmentDefinitionDto.AssessmentName =
                            _resourcesManager.GetResourceManagerByName(assessmentDefinitionDto.AssessmentName)
                                             .GetString(SharedStringNames.ResourceKeyPrefix + assessmentDefinitionDto.AssessmentCode);
                    }
                    var dataTableResponse = new DataTableResponse<AssessmentDefinitionDto>
                        {
                            Data = assessmentDefinitionDtos,
                            Echo = sEcho,
                            TotalDisplayRecords = totalCount,
                            TotalRecords = totalCount,
                        };

                    return dataTableResponse;
                }
            }
        }

        [HttpGet]
        public FinderResults<AssessmentDefinitionDto> FinderSearchActive(int page, int pageSize, string search = null)
        {
            var start = page * pageSize;
            var end = start + pageSize;
            var replaceString = string.IsNullOrWhiteSpace(search) ? "" : WhereConstraintActive;
            var completeQuery = string.Format(QueryActive, replaceString);

            using (var connection = _connectionFactory.CreateConnection())
            using (var multiQuery = connection.QueryMultiple(completeQuery, new { start, end, search, UserContext.Current.OrganizationKey }))
            {
                var totalCount = multiQuery.Read<int>().Single();
                var assessmentDefinitionDtos = multiQuery.Read<AssessmentDefinitionDto>().ToList();
                foreach (var assessmentDefinitionDto in assessmentDefinitionDtos)
                {
                    assessmentDefinitionDto.AssessmentName =
                        _resourcesManager.GetResourceManagerByName(assessmentDefinitionDto.AssessmentName)
                                         .GetString(SharedStringNames.ResourceKeyPrefix + assessmentDefinitionDto.AssessmentCode);
                }

                var findResults = new FinderResults<AssessmentDefinitionDto>
                {
                    Data = assessmentDefinitionDtos,
                    TotalCount = totalCount
                };

                return findResults;
            }
        }

        [HttpGet]
        public FinderResults<AssessmentDefinitionDto> FinderSearch(int page, int pageSize, string search = null)
        {
            const string whereConstraint = " AND ([t1].AssessmentName LIKE @search+'%')";
            const string query = @"
                             SELECT COUNT(*) as TotalCount 
                             FROM AssessmentModule.AssessmentDefinition [t1] LEFT JOIN OrganizationModule.OrganizationAssessmentDefinition [t2] 
                                 ON [t1].AssessmentDefinitionKey = [t2].AssessmentDefinitionKey AND OrganizationKey=@OrganizationKey 
                                 WHERE [t2].AssessmentDefinitionKey IS NULL{0}
                             SELECT [t].AssessmentName,
                                    [t].AssessmentDefinitionKey AS 'Key',
                                    [t].AssessmentCode                                      
                             FROM ( 
                                 SELECT ROW_NUMBER() OVER ( 
                                    ORDER BY [t1].AssessmentName) AS [ROW_NUMBER],   
                                             [t1].AssessmentName,
                                             [t1].AssessmentDefinitionKey,
                                             [t1].AssessmentCode  
                                 FROM AssessmentModule.AssessmentDefinition [t1] LEFT JOIN OrganizationModule.OrganizationAssessmentDefinition [t2] 
                                 ON [t1].AssessmentDefinitionKey = [t2].AssessmentDefinitionKey AND OrganizationKey=@OrganizationKey 
                                 WHERE [t2].AssessmentDefinitionKey IS NULL{0}
                                 ) AS [t] 
                             WHERE [t].[ROW_NUMBER] BETWEEN @start + 1 AND @end 
                             ORDER BY [t].[ROW_NUMBER] ";
            var start = page * pageSize;
            var end = start + pageSize;
            var replaceString = string.IsNullOrWhiteSpace(search) ? "" : whereConstraint;
            var completeQuery = string.Format(query, replaceString);

            using (var connection = _connectionFactory.CreateConnection())
            using (var multiQuery = connection.QueryMultiple(completeQuery, new { start, end, search, UserContext.Current.OrganizationKey }))
            {
                var totalCount = multiQuery.Read<int>().Single();
                var assessmentDefinitionDtos = multiQuery.Read<AssessmentDefinitionDto>().ToList();
                foreach (var assessmentDefinitionDto in assessmentDefinitionDtos)
                {
                    assessmentDefinitionDto.AssessmentName =
                        _resourcesManager.GetResourceManagerByName(assessmentDefinitionDto.AssessmentName)
                                         .GetString(SharedStringNames.ResourceKeyPrefix + assessmentDefinitionDto.AssessmentCode);
                }

                var findResults = new FinderResults<AssessmentDefinitionDto>
                    {
                        Data = assessmentDefinitionDtos,
                        TotalCount = totalCount
                    };

                return findResults;
            }
        }
    }
}