namespace ProCenter.Service.Message.Report
{
    using System;
    using Agatha.Common;

    public class GetReportCustomizationModelRequest : Request
    {
        public Guid SourceKey { get; set; }
        public string ReportName { get; set; }
    }
}