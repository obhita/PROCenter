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