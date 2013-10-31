namespace ProCenter.Mvc.Controllers.Api
{
    #region

    using System.Linq;
    using Common;
    using Dapper;
    using Models;
    using Primitive;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using Service.Message.Organization;

    #endregion

    public class StaffSearchDataTableController : BaseApiController
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public StaffSearchDataTableController(IDbConnectionFactory dbConnectionFactory)
        {
            _connectionFactory = dbConnectionFactory;
        }

        public DataTableResponse<StaffDto> Get(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch = null)
        {
            const string whereConstraint = " AND (FirstName LIKE @search+'%' OR LastName LIKE @search+'%')";
            const string query = @"
                             SELECT COUNT(*) as TotalCount FROM OrganizationModule.Staff
                                 WHERE OrganizationKey=@OrganizationKey{0}
                             SELECT [t].FirstName,
                                    [t].LastName, 
                                    [t].StaffKey AS 'Key',
                                    [t].Email, 
                                    [t].Location,
                                    [t].NPI                                       
                             FROM ( 
                                 SELECT ROW_NUMBER() OVER ( 
                                    ORDER BY [t1].LastName) AS [ROW_NUMBER],   
                                             [t1].FirstName,
                                             [t1].LastName, 
                                             [t1].Email,
                                             [t1].Location,
                                             [t1].NPI,
                                             [t1].StaffKey  
                                 FROM OrganizationModule.Staff AS [t1]
                                 WHERE OrganizationKey=@OrganizationKey{0}
                                 ) AS [t] 
                             WHERE [t].[ROW_NUMBER] BETWEEN @start + 1 AND @end 
                             ORDER BY [t].[ROW_NUMBER] ";

            var start = iDisplayStart;
            var end = start + iDisplayLength;
            var replaceString = string.IsNullOrWhiteSpace(sSearch) ? "" : whereConstraint;
            var completeQuery = string.Format(query, replaceString);

            using (var connection = _connectionFactory.CreateConnection())
            using (var multiQuery = connection.QueryMultiple(completeQuery, new {start, end, search = sSearch, UserContext.Current.OrganizationKey}))
            {
                var totalCount = multiQuery.Read<int>().Single();
                var staffDtos =
                    multiQuery.Read<PersonName, StaffDto, StaffDto>((personName, staffDto) =>
                        {
                            staffDto.Name = personName;
                            return staffDto;
                        }, "Key");
                var dataTableResponse = new DataTableResponse<StaffDto>
                    {
                        Data = staffDtos.ToList(),
                        Echo = sEcho,
                        TotalDisplayRecords = totalCount,
                        TotalRecords = totalCount,
                    };

                return dataTableResponse;
            }
        }
    }
}