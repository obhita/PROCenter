namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using Agatha.Common;

    #endregion

    public class GetReportCustomizationModelResponse : Response
    {
        #region Public Properties

        public ReportModelDto ReportModelDto { get; set; }

        #endregion
    }
}