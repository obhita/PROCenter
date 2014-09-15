namespace ProCenter.Service.Handler.Report
{
    #region Using Statements

    using System;
    using System.Runtime.Remoting.Contexts;

    using Common;
    using Domain.AssessmentModule;
    using Domain.CommonModule;
    using Domain.ReportsModule;
    using Pillar.Common.InversionOfControl;

    using ProCenter.Common;

    using Service.Message.Report;

    #endregion

    /// <summary>The get assessment score over time request handler class.</summary>
    public class GetAssessmentScoreOverTimeRequestHandler : ServiceRequestHandler<GetAssessmentScoreOverTimeReportRequest, GetReportResponse>
    {
        #region Fields

        private readonly ILookupProvider _lookupProvider;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAssessmentScoreOverTimeRequestHandler"/> class.
        /// </summary>
        /// <param name="lookupProvider">The lookup provider.</param>
        public GetAssessmentScoreOverTimeRequestHandler ( ILookupProvider lookupProvider )
        {
            _lookupProvider = lookupProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( GetAssessmentScoreOverTimeReportRequest request, GetReportResponse response )
        {
            if ( request.AssessmentScoreOverTimeParametersDto.AssessmentDefinitionCode != null )
            {
                var reportEngine = IoC.CurrentContainer.Resolve<IReportEngine> ( ReportNames.AssessmentScoreOverTime );
                var parameters = new AssessmentScoreOverTimeParameters
                    {
                        AssessmentDefinitionCode = request.AssessmentScoreOverTimeParametersDto.AssessmentDefinitionCode,
                        AssessmentName = request.AssessmentScoreOverTimeParametersDto.AssessmentName,
                        EndDate = request.AssessmentScoreOverTimeParametersDto.EndDate,
                        PatientKey = request.AssessmentScoreOverTimeParametersDto.PatientKey.GetValueOrDefault(),
                        StartDate = request.AssessmentScoreOverTimeParametersDto.StartDate,
                        TimePeriod = _lookupProvider.Find<ReportTimePeriod> ( request.AssessmentScoreOverTimeParametersDto.TimePeriod.Code )
                                 };
                var report = reportEngine.Generate ( Guid.Empty,
                    ReportNames.AssessmentScoreOverTime,
                    parameters
                     );
                response.Report = report;
                new RecentReport(ReportNames.AssessmentScoreOverTime, 
                                 UserContext.Current.SystemAccountKey.Value, 
                                 request.AssessmentScoreOverTimeParametersDto.AssessmentName, 
                                 DateTime.Now, 
                                 parameters);
            }
        }

        #endregion
    }
}