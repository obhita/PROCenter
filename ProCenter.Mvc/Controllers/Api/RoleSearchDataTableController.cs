namespace ProCenter.Mvc.Controllers.Api
{
    #region

    using System.Linq;
    using Common;
    using Dapper;
    using Models;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using Service.Message.Organization;
    using Service.Message.Security;

    #endregion

    public class RoleSearchDataTableController : BaseApiController
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public RoleSearchDataTableController(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public DataTableResponse<RoleDto> Get(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch = null)
        {
            const string whereConstraint = " AND (Name LIKE @Search+'%')";
            const string query = @"
                            SELECT COUNT(*) as TotalCount FROM SecurityModule.Role
                                WHERE OrganizationKey='{0}'{1}
                            SELECT [t].Name,
                                   [t].RoleKey AS 'Key'
                            FROM (
                                SELECT ROW_NUMBER() OVER (
                                    ORDER BY [t1].Name) AS [ROW_NUMBER],
                                             [T1].Name,   
                                             [t1].RoleKey
                                FROM SecurityModule.Role AS [t1]
                                WHERE OrganizationKey='{0}'{1}   
                                ) AS [t]
                            WHERE [t].[ROW_NUMBER] BETWEEN @start +1 AND @end
                            ORDER BY [t].[ROW_NUMBER]";

            var start = iDisplayStart;
            var end = start + iDisplayLength;
            var replaceString = string.IsNullOrWhiteSpace(sSearch) ? "" : whereConstraint;
            var completeQuery = string.Format(query, UserContext.Current.OrganizationKey, replaceString);
            using (var connection = _connectionFactory.CreateConnection())
            using (var multiQuery = connection.QueryMultiple(completeQuery, new {start, end, search = sSearch}))
            {
                var totalCount = multiQuery.Read<int>().Single();
                var roleDtos = multiQuery.Read<RoleDto>();
                var dataTableResponse = new DataTableResponse<RoleDto>
                    {
                        Data = roleDtos.ToList(),
                        Echo = sEcho,
                        TotalDisplayRecords = totalCount,
                        TotalRecords = totalCount,
                    };

                return dataTableResponse;
            }
        }
    }
}