#region Licence Header
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