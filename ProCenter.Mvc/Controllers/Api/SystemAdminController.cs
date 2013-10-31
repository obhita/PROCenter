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
    using System;
    using System.Linq;
    using System.Web.Http;
    using Dapper;
    using Models;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using Service.Message.Organization;
    using Service.Message.Security;

    public class SystemAdminController : BaseApiController
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SystemAdminController(IDbConnectionFactory dbConnectionFactory)
        {
            _connectionFactory = dbConnectionFactory;
        }

        [HttpGet]
        public DataTableResponse<OrganizationSummaryDto> OrganizationDataTableSearch(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch = null)
        {
            const string searchWhereConstraint = "WHERE Name LIKE @search+'%'";
            const string query = @"
                             SELECT COUNT(*) as TotalCount FROM OrganizationModule.Organization
                                 {0}
                             SELECT [t].Name,
                                    [t].OrganizationKey AS 'Key'                                    
                             FROM ( 
                                 SELECT ROW_NUMBER() OVER ( 
                                    ORDER BY [t1].Name) AS [ROW_NUMBER],   
                                             [t1].Name,
                                             [t1].OrganizationKey  
                                 FROM OrganizationModule.Organization AS [t1]
                                 {0}
                                 ) AS [t] 
                             WHERE [t].[ROW_NUMBER] BETWEEN @start + 1 AND @end 
                             ORDER BY [t].[ROW_NUMBER] ";

            var start = iDisplayStart;
            var end = start + iDisplayLength;
            var completeQuery = string.Format ( query, sSearch == null ? "" : searchWhereConstraint );

            using (var connection = _connectionFactory.CreateConnection())
            using (var multiQuery = connection.QueryMultiple( completeQuery, new { start, end, search = sSearch } ))
            {
                var totalCount = multiQuery.Read<int> ().Single ();
                var organizationDtos = multiQuery.Read<OrganizationSummaryDto>();
                var dataTableResponse = new DataTableResponse<OrganizationSummaryDto>
                    {
                        Data = organizationDtos.ToList (),
                        Echo = sEcho,
                        TotalDisplayRecords = totalCount,
                        TotalRecords = totalCount,
                    };

                return dataTableResponse;
            }
        }

        [HttpGet]
        public DataTableResponse<SystemAccountDto> SystemAdministratorsDataTableSearch(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch = null)
        {
            const string searchWhereConstraint = "AND Identifier LIKE @search+'%'";
            const string query = @"
                             SELECT COUNT(*) as TotalCount FROM SecurityModule.SystemAccount
                                 WHERE OrganizationKey = @OrganizationKey {0}
                             SELECT [t].Identifier,
                                    [t].OrganizationKey,
                                    [t].SystemAccountKey AS 'Key'                                  
                             FROM ( 
                                 SELECT ROW_NUMBER() OVER ( 
                                    ORDER BY [t1].Identifier) AS [ROW_NUMBER],   
                                             [t1].Identifier,
                                             [t1].OrganizationKey,
                                             [t1].SystemAccountKey
                                 FROM SecurityModule.SystemAccount AS [t1]
                                 WHERE OrganizationKey = @OrganizationKey {0}
                                 ) AS [t] 
                             WHERE [t].[ROW_NUMBER] BETWEEN @start + 1 AND @end 
                             ORDER BY [t].[ROW_NUMBER] ";

            var start = iDisplayStart;
            var end = start + iDisplayLength;
            var completeQuery = string.Format(query, sSearch == null ? "" : searchWhereConstraint);

            using (var connection = _connectionFactory.CreateConnection())
            using (var multiQuery = connection.QueryMultiple(completeQuery, new { start, end, search = sSearch, OrganizationKey = Guid.Empty }))
            {
                var totalCount = multiQuery.Read<int>().Single();
                var systemAccountDtos = multiQuery.Read<SystemAccountDto>();
                var dataTableResponse = new DataTableResponse<SystemAccountDto>
                {
                    Data = systemAccountDtos.ToList(),
                    Echo = sEcho,
                    TotalDisplayRecords = totalCount,
                    TotalRecords = totalCount,
                };

                return dataTableResponse;
            }
        }
    }
}