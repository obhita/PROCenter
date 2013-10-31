namespace ProCenter.Service.Message.Report
{
    using System;
    using Agatha.Common;

    public class SaveReportCustomizationModelRequest : Request
    {
        public Guid SourceKey { get; set; }
        public string ReportName { get; set; }
        public ReportItemDto ReportItemDto { get; set; }
    }
}