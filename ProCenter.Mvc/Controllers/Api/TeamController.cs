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
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Common;
    using Dapper;
    using Models;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using Service.Message.Organization;

    #endregion

    public class TeamController : BaseApiController
    {
        #region Fields

        private readonly IDbConnectionFactory _connectionFactory;

        #endregion

        #region Constructors and Destructors

        public TeamController ( IDbConnectionFactory dbConnectionFactory )
        {
            _connectionFactory = dbConnectionFactory;
        }

        #endregion

        #region Public Methods and Operators

        public DataTableResponse<TeamSummaryDto> Get ( string sEcho, int iDisplayStart, int iDisplayLength, string sSearch = null )
        {
            const string whereConstraint = " AND (Name LIKE @search+'%')";
            const string query = @"
                             SELECT COUNT(*) as TotalCount FROM OrganizationModule.Team
                                 WHERE OrganizationKey=@OrganizationKey{0}
                             SELECT [t].Name,
                                    [t].TeamKey as 'Key'                                      
                             FROM ( 
                                 SELECT ROW_NUMBER() OVER ( 
                                    ORDER BY [t1].Name) AS [ROW_NUMBER],   
                                             [t1].Name,
                                             [t1].TeamKey  
                                 FROM OrganizationModule.Team AS [t1]
                                 WHERE OrganizationKey=@OrganizationKey{0}
                                 ) AS [t] 
                             WHERE [t].[ROW_NUMBER] BETWEEN @start + 1 AND @end 
                             ORDER BY [t].[ROW_NUMBER] ";

            var start = iDisplayStart;
            var end = start + iDisplayLength;
            var replaceString = string.IsNullOrWhiteSpace(sSearch) ? "" : whereConstraint;
            var completeQuery = string.Format(query, replaceString);

            using ( var connection = _connectionFactory.CreateConnection () )
            using (var multiQuery = connection.QueryMultiple(completeQuery, new { start, end, search = sSearch, UserContext.Current.OrganizationKey }))
            {
                var totalCount = multiQuery.Read<int> ().Single ();
                var teamDtos = multiQuery.Read<TeamSummaryDto> ();
                var dataTableResponse = new DataTableResponse<TeamSummaryDto>
                    {
                        Data = teamDtos.ToList (),
                        Echo = sEcho,
                        TotalDisplayRecords = totalCount,
                        TotalRecords = totalCount,
                    };

                return dataTableResponse;
            }
        }

        [HttpGet]
        public FinderResults<TeamSummaryDto> FinderSearch(int page, int pageSize, string search = null)
        {
            const string whereConstraint = " AND (Name LIKE @search+'%')";
            const string query = @"
                             SELECT COUNT(*) as TotalCount FROM OrganizationModule.Team
                                 WHERE OrganizationKey=@OrganizationKey{0}
                             SELECT [t].Name,
                                    [t].TeamKey as 'Key'                                      
                             FROM ( 
                                 SELECT ROW_NUMBER() OVER ( 
                                    ORDER BY [t1].Name) AS [ROW_NUMBER],   
                                             [t1].Name,
                                             [t1].TeamKey  
                                 FROM OrganizationModule.Team AS [t1]
                                 WHERE OrganizationKey=@OrganizationKey{0}
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
                var teamDtos = multiQuery.Read<TeamSummaryDto>();
                var dataTableResponse = new FinderResults<TeamSummaryDto>
                {
                    Data = teamDtos.ToList(),
                    TotalCount = totalCount
                };

                return dataTableResponse;
            }
        }

        [HttpGet]
        public IEnumerable<TeamSummaryDto> GetByStaffKey ( Guid staffKey )
        {
            const string query = @"
                             SELECT TOP 500 t.TeamKey AS 'Key',t.Name FROM [OrganizationModule].[Team] AS t
                             INNER JOIN [OrganizationModule].[TeamStaff] as s on s.TeamKey = t.TeamKey
                             WHERE OrganizationKey=@OrganizationKey AND s.StaffKey=@StaffKey";
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                var results = connection.Query<TeamSummaryDto> ( query, new {StaffKey = staffKey, UserContext.Current.OrganizationKey} );

                return results;
            }
        }

        [HttpGet]
        public IEnumerable<TeamSummaryDto> GetByPatientKey(Guid patientKey)
        {
            const string query = @"
                             SELECT TOP 500 t.TeamKey AS 'Key',t.Name FROM [OrganizationModule].[Team] AS t
                             INNER JOIN [OrganizationModule].[TeamPatient] as p on p.TeamKey = t.TeamKey
                             WHERE OrganizationKey=@OrganizationKey AND p.PatientKey=@PatientKey";
            using (var connection = _connectionFactory.CreateConnection())
            {
                var results = connection.Query<TeamSummaryDto>(query, new { PatientKey = patientKey, UserContext.Current.OrganizationKey });

                return results;
            }
        }

        #endregion
    }
}