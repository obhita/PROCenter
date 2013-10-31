namespace ProCenter.Mvc.Controllers.Api
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using Agatha.Common;
    using Common;
    using Dapper;
    using Infrastructure;
    using Models;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using Service.Message.Assessment;
    using Service.Message.Patient;

    #endregion

    public class AssessmentController : BaseApiController
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IResourcesManager _resourcesManager;

        public AssessmentController(IRequestDispatcherFactory requestDispatcherFactory, IDbConnectionFactory connectionFactory, IResourcesManager resourcesManager)
            : base(requestDispatcherFactory)
        {
            _connectionFactory = connectionFactory;
            _resourcesManager = resourcesManager;
        }

        //[HttpGet]
        //public async Task<AssessmentDto> Get(Guid key)
        //{
        //    AssessmentDto assessmentDto = null;
        //    var patientRequestDisptacher = CreateAsyncRequestDispatcher();
        //    patientRequestDisptacher.Add(new GetPatientDtoByAssessmentKeyRequest {AssessmentKey = key});

        //    var patientResponse = patientRequestDisptacher.Get<GetPatientDtoResponse>();

        //    if (patientResponse != null && patientResponse.DataTransferObject != null)
        //    {
        //        var requestDispatcher = CreateAsyncRequestDispatcher();
        //        requestDispatcher.Add(new GetAssessmentByKeyRequest {AssessmentKey = key});
        //        var response = await requestDispatcher.GetAsync<GetAssessmentResponse>();

        //        assessmentDto = response.DataTransferObject;
        //    }
        //    if (assessmentDto == null)
        //    {
        //        HttpContext.Current.Response.StatusCode = (int) HttpStatusCode.NotFound;
        //    }
        //    return assessmentDto;
        //}

        //public async Task<KeyResult> Post(Guid key) // Note: key = patient key
        //{
        //    var requestDispatcher = CreateAsyncRequestDispatcher();
        //    requestDispatcher.Add(new CreateAssessmentRequest {PatientKey = key});
        //    var response = await requestDispatcher.GetAsync<CreateAssessmentResponse>();
        //    return new KeyResult {Key = response.AssessmentKey};
        //}

        public DataTableResponse<ReportSummaryDto> GetReportDataTable(string sEcho, int iDisplayStart, int iDisplayLength, Guid? patientKey = null, string sSearch = null)
        {
            const string whereConstraint = "PatientKey = @patientKey";
            const string whereSearchConstraint = "(Name LIKE @search+'%')";
            const string wherePatientViewableConstraint = "IsPatientViewable = 1";
            const string query = @"
                             SELECT COUNT(*) as TotalCount FROM AssessmentModule.Report
                                 {0}
                             SELECT [t].SourceKey,
                                    [t].CreatedTimestamp as 'CreatedTime', 
                                    [t].Name,  
                                    [t].CanCustomize,
                                    [t].PatientKey,   
                                    [t].ReportSeverity, 
                                    [t].ReportStatus,  
                                    [t].ReportKey as 'Key' ,
                                    [t].OrganizationKey  
                             FROM ( 
                                 SELECT ROW_NUMBER() OVER ( 
                                    ORDER BY [t1].Name) AS [ROW_NUMBER],   
                                             [t1].Name,
                                             [t1].CreatedTimestamp, 
                                             [t1].CanCustomize,  
                                             [t1].PatientKey  ,
                                             [t1].ReportSeverity, 
                                             [t1].ReportStatus,     
                                             [t1].SourceKey  ,   
                                             [t1].ReportKey ,
                                             [t1].OrganizationKey  
                                 FROM AssessmentModule.Report AS [t1]
                                 {0}
                                 ) AS [t] 
                             WHERE [t].[ROW_NUMBER] BETWEEN @start + 1 AND @end 
                             ORDER BY [t].[ROW_NUMBER] ";

            if ( UserContext.Current.PatientKey.HasValue && UserContext.Current.PatientKey != patientKey )
            {
                return new DataTableResponse<ReportSummaryDto>
                    {
                        Data = Enumerable.Empty<ReportSummaryDto> (),
                        Echo = sEcho,
                        TotalDisplayRecords = 0,
                        TotalRecords = 0
                    };
            }

            var start = iDisplayStart;
            var end = start + iDisplayLength;
            var whereConstraintBuilder = new StringBuilder("WHERE OrganizationKey = @OrganizationKey ");
            if ( patientKey.HasValue || sSearch != null || UserContext.Current.PatientKey.HasValue )
            {
                if ( patientKey.HasValue )
                {
                    whereConstraintBuilder.Append( " AND " + whereConstraint );
                }
                if ( sSearch != null )
                {
                    whereConstraintBuilder.Append ( " AND " +  whereSearchConstraint );
                }
                if ( UserContext.Current.PatientKey.HasValue )
                {
                    whereConstraintBuilder.Append( " AND " + wherePatientViewableConstraint );
                }

            }
            var completeQuery = string.Format(query, whereConstraintBuilder);
            var totalCount = 0;
            IEnumerable<ReportSummaryDto> reportDtos = null;
            try
            {
                using ( var connection = _connectionFactory.CreateConnection () )
                using ( var multiQuery = connection.QueryMultiple ( completeQuery, new {start, end, patientKey, search = sSearch, UserContext.Current.OrganizationKey } ) )
                {
                    totalCount = multiQuery.Read<int> ().Single ();
                    reportDtos = multiQuery.Read<ReportSummaryDto> ().ToList ();

                    foreach ( var dto in reportDtos )
                    {
                        dto.DisplayName = _resourcesManager.GetResourceManagerByName ( dto.Name ).GetString ( SharedStringNames.ReportName );
                    }
                }
            }
            catch ( Exception e )
            {

            }

            return new DataTableResponse<ReportSummaryDto>
            {
                Data = reportDtos,
                Echo = sEcho,
                TotalDisplayRecords = totalCount,
                TotalRecords = totalCount,
            };
        }
    }
}