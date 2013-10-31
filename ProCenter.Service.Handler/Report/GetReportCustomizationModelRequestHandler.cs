namespace ProCenter.Service.Handler.Report
{
    #region Using Statements

    using Common;
    using Domain.AssessmentModule;
    using Pillar.Common.InversionOfControl;
    using Service.Message.Report;
    using global::AutoMapper;

    #endregion

    /// <summary>
    /// Handler for getting report customization model.
    /// </summary>
    public class GetReportCustomizationModelRequestHandler : ServiceRequestHandler<GetReportCustomizationModelRequest, GetReportCustomizationModelResponse>
    {
        #region Methods

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( GetReportCustomizationModelRequest request, GetReportCustomizationModelResponse response )
        {
            var reportEngine = IoC.CurrentContainer.Resolve<IReportEngine> ( request.ReportName );
            var reportModel = reportEngine.GetCustomizationModel ( request.SourceKey, request.ReportName );
            response.ReportModelDto = Mapper.Map<ReportModel, ReportModelDto>(reportModel);
            response.ReportModelDto.Key = request.SourceKey;
        }

        #endregion
    }
}