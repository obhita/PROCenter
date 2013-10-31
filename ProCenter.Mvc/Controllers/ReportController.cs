namespace ProCenter.Mvc.Controllers
{
    #region

    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Agatha.Common;
    using Common;
    using DevExpress.Web.Mvc;
    using DevExpress.XtraReports.UI;
    using Infrastructure;
    using Models;
    using Service.Message.Assessment;
    using Service.Message.Report;

    #endregion

    public class ReportController : BaseController
    {
        private readonly IResourcesManager _resourcesManager;

        public ReportController(IRequestDispatcherFactory requestDispatcherFactory, IResourcesManager resourcesManager ) : base(requestDispatcherFactory)
        {
            _resourcesManager = resourcesManager;
        }

        public ActionResult Get(Guid key, string reportName)
        {
            ViewData["SourceKey"] = key;
            ViewData["ReportName"] = reportName;
            return View();
        }

        public async Task<ActionResult> ReportViewerPartial(Guid key, string reportName)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new GetReportRequest {SourceKey = key, ReportName = reportName});
            var response = await requestDispatcher.GetAsync<GetReportResonse>();
            ViewData["SourceKey"] = key;
            ViewData["ReportName"] = reportName;
            
            return PartialView("ReportViewerPartial",response.Report);
        }

        public async Task<ActionResult> ExportReportViewer(Guid key, string reportName)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new GetReportRequest { SourceKey = key, ReportName = reportName });
            var response = await requestDispatcher.GetAsync<GetReportResonse>();
            return ReportViewerExtension.ExportTo((XtraReport)response.Report);
        }

        [HttpGet]
        public async Task<PartialViewResult> Customize ( Guid key, string reportName )
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new GetReportCustomizationModelRequest { SourceKey = key, ReportName = reportName });
            var response = await requestDispatcher.GetAsync<GetReportCustomizationModelResponse>();

            ViewData["ResourceManager"] = _resourcesManager.GetResourceManagerByName ( reportName );

            return PartialView ( response.ReportModelDto );
        }

        [HttpPost]
        public async Task<ActionResult> Customize(Guid key, string reportName, ReportItemDto reportItemDto)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new SaveReportCustomizationModelRequest { SourceKey = key, ReportName = reportName, ReportItemDto = reportItemDto });
            var response = await requestDispatcher.GetAsync<SaveReportCustomizationModelResponse>();
            return null;
        }
    }
}