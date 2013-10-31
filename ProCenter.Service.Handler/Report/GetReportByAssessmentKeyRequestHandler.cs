namespace ProCenter.Service.Handler.Report
{
    #region

    using Common;
    using Domain.AssessmentModule;
    using Pillar.Common.InversionOfControl;
    using Service.Message.Report;

    #endregion

    public class GetReportByAssessmentKeyRequestHandler : ServiceRequestHandler<GetReportRequest, GetReportResonse>
    {
        protected override void Handle(GetReportRequest request, GetReportResonse response)
        {
            var reportEngine = IoC.CurrentContainer.Resolve<IReportEngine>(request.ReportName);
            var report = reportEngine.Generate(request.SourceKey, request.ReportName);
            response.Report = report;
        }
    }
}