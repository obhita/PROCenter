namespace ProCenter.Service.Handler.Report
{
    #region Using Statements

    using Common;
    using Domain.AssessmentModule;
    using Pillar.Common.InversionOfControl;
    using Service.Message.Report;

    #endregion

    /// <summary>
    ///     Handler for saving a report customization.
    /// </summary>
    public class SaveReportCustomizationModelRequestHandler : ServiceRequestHandler<SaveReportCustomizationModelRequest, SaveReportCustomizationModelResponse>
    {
        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( SaveReportCustomizationModelRequest request, SaveReportCustomizationModelResponse response )
        {
            var reportEngine = IoC.CurrentContainer.Resolve<IReportEngine> ( request.ReportName );
            reportEngine.UpdateCustomizationModel ( request.SourceKey, request.ReportName, request.ReportItemDto.Name, request.ReportItemDto.ShouldShow, request.ReportItemDto.Text );
        }

        #endregion
    }
}