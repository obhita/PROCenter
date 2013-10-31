namespace ProCenter.Mvc.Controllers.Api
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using Agatha.Common;
    using Common;
    using Dapper;
    using Models;
    using Primitive;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using Service.Message.Common;
    using Service.Message.Common.Lookups;
    using Service.Message.Patient;

    public class PatientController : BaseApiController
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IResourcesManager _resourcesManager;

        const string WhereConstraint = " AND (FirstName LIKE @search+'%' OR LastName LIKE @search+'%')";
        const string Query = @"
                             SELECT COUNT(*) as TotalCount FROM PatientModule.Patient
                                 WHERE OrganizationKey = @OrganizationKey{0}
                             SELECT [t].FirstName,
                                    [t].LastName, 
                                    [t].GenderCode as Code,  
                                    [t].PatientKey as 'Key'   
                             FROM ( 
                                 SELECT ROW_NUMBER() OVER ( 
                                    ORDER BY [t1].LastName) AS [ROW_NUMBER],   
                                             [t1].FirstName,
                                             [t1].LastName, 
                                             [t1].GenderCode,  
                                             [t1].PatientKey   
                                 FROM PatientModule.Patient AS [t1]
                                 WHERE OrganizationKey = @OrganizationKey{0}
                                 ) AS [t] 
                             WHERE [t].[ROW_NUMBER] BETWEEN @start + 1 AND @end 
                             ORDER BY [t].[ROW_NUMBER] ";

        public PatientController(IRequestDispatcherFactory requestDispatcherFactory, IDbConnectionFactory connectionFactory, IResourcesManager resourcesManager) : base(requestDispatcherFactory)
        {
            _connectionFactory = connectionFactory;
            _resourcesManager = resourcesManager;
        }

        public async Task<PatientDto> Get(Guid key)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new GetPatientDtoByKeyRequest {PatientKey = key});
            var response = await requestDispatcher.GetAsync<GetPatientDtoResponse>();

            var patientDto = response.DataTransferObject;
            if (patientDto == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return patientDto;
        }

        public async Task<KeyResult> Post(PatientDto patientDto)
        {
            if (ModelState.IsValid)
            {
                var requestDispatcher = CreateAsyncRequestDispatcher();
                requestDispatcher.Add(new CreatePatientRequest {PatientDto = patientDto});
                var response = await requestDispatcher.GetAsync<SaveDtoResponse<PatientDto>>();
                return new KeyResult {Key = response.DataTransferObject.Key};
            }
            HttpContext.Current.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            return null;
        }

        public async Task<KeyResult> Put(PatientDto patientDto)
        {
            if (ModelState.IsValid)
            {
                var requestDispatcher = CreateAsyncRequestDispatcher();
                requestDispatcher.Add(new SaveDtoRequest<PatientDto> {DataTransferObject = patientDto});
                var response = await requestDispatcher.GetAsync<SaveDtoResponse<PatientDto>>();
                return new KeyResult {Key = response.DataTransferObject.Key};
            }
            HttpContext.Current.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            return null;
        }

        [HttpGet]
        public DataTableResponse<PatientDto> DataTableSearch(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch = null)
        {
            var start = iDisplayStart;
            var end = start + iDisplayLength;
            var replaceString = string.IsNullOrWhiteSpace(sSearch) ? "" : WhereConstraint;
            var completeQuery = string.Format(Query, replaceString);

            using (var connection = _connectionFactory.CreateConnection())
            using (var multiQuery = connection.QueryMultiple(completeQuery, new { start, end, UserContext.Current.OrganizationKey, search = sSearch }))
            {
                var totalCount = multiQuery.Read<int>().Single();
                var patientDtos =
                    multiQuery.Read<PersonName, string, PatientDto, PatientDto>((personName, code, patientDto) =>
                    {
                        patientDto.Name = personName;
                        var lookupDto = new LookupDto
                        {
                            Code = code,
                            Name = _resourcesManager.GetResourceManagerByName("Gender").GetString(code)
                        };
                        patientDto.Gender = lookupDto;
                        return patientDto;
                    }, "Code,Key");

                var dataTableResponse = new DataTableResponse<PatientDto>
                {
                    Data = patientDtos.ToList(),
                    Echo = sEcho,
                    TotalDisplayRecords = totalCount,
                    TotalRecords = totalCount,
                };

                return dataTableResponse;
            }
        }

        [HttpGet]
        public FinderResults<PatientDto> FinderSearch(int page, int pageSize, string search = null)
        {
            var start = page * pageSize;
            var end = start + pageSize;
            var replaceString = string.IsNullOrWhiteSpace(search) ? "" : WhereConstraint;
            var completeQuery = string.Format(Query, replaceString);

            using (var connection = _connectionFactory.CreateConnection())
            using (var multiQuery = connection.QueryMultiple(completeQuery, new { start, end, UserContext.Current.OrganizationKey, search }))
            {
                var totalCount = multiQuery.Read<int>().Single();
                var patientDtos =
                    multiQuery.Read<PersonName, string, PatientDto, PatientDto>((personName, code, patientDto) =>
                        {
                            patientDto.Name = personName;
                            var lookupDto = new LookupDto
                                {
                                    Code = code,
                                    Name = _resourcesManager.GetResourceManagerByName("Gender").GetString(code)
                                };
                            patientDto.Gender = lookupDto;
                            return patientDto;
                        }, "Code,Key").ToList();

                var finderResults = new FinderResults<PatientDto>()
                    {
                        Data = patientDtos,
                        TotalCount = totalCount
                    };

                return finderResults;
            }
        }
    }
}
