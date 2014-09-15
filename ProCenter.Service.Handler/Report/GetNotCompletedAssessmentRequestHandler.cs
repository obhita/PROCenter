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

namespace ProCenter.Service.Handler.Report
{
    #region Using Statements

    using System;

    using Pillar.Common.InversionOfControl;

    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.PatientModule;
    using ProCenter.Domain.ReportsModule;
    using ProCenter.Domain.ReportsModule.NotCompletedAssessmentReport;
    using ProCenter.Domain.ReportsModule.PatientScoreRangeReport;
    using ProCenter.Service.Handler.Common;
    using ProCenter.Service.Message.Report;

    using global::AutoMapper;

    #endregion

    /// <summary>The get not completed assessment request handler class.</summary>
    public class GetNotCompletedAssessmentRequestHandler : ServiceRequestHandler<GetNotCompletedAssessmentReportRequest, GetReportResponse>
    {
        #region Fields

        private readonly ILookupProvider _lookupProvider;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GetNotCompletedAssessmentRequestHandler" /> class.
        /// </summary>
        /// <param name="lookupProvider">The lookup provider.</param>
        public GetNotCompletedAssessmentRequestHandler(ILookupProvider lookupProvider)
        {
            _lookupProvider = lookupProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( GetNotCompletedAssessmentReportRequest request, GetReportResponse response )
        {
            if ( request.NotCompletedAssessmentParametersDto != null )
            {
                var reportEngine = IoC.CurrentContainer.Resolve<IReportEngine> ( ReportNames.NotCompletedAssessment );
                var parameters = new NotCompletedAssessmentParameters
                                     {
                                         OrganizationKey = request.NotCompletedAssessmentParametersDto.OrganizationKey,
                                         AssessmentDefinitionCode = request.NotCompletedAssessmentParametersDto.AssessmentDefinitionCode,
                                         AssessmentName = request.NotCompletedAssessmentParametersDto.AssessmentName,
                                         EndDate = request.NotCompletedAssessmentParametersDto.EndDate,
                                         PatientKey = request.NotCompletedAssessmentParametersDto.PatientKey.GetValueOrDefault(),
                                         StartDate = request.NotCompletedAssessmentParametersDto.StartDate,
                                         TimePeriod = _lookupProvider.Find<ReportTimePeriod>(request.NotCompletedAssessmentParametersDto.TimePeriod.Code),
                                         AgeRangeHigh = request.NotCompletedAssessmentParametersDto.AgeRangeHigh,
                                         AgeRangeLow = request.NotCompletedAssessmentParametersDto.AgeRangeLow,
                                         Gender = request.NotCompletedAssessmentParametersDto.Gender.Code ==
                                            null ? string.Empty : _lookupProvider.Find<Gender>(request.NotCompletedAssessmentParametersDto.Gender.Code),
                                     };
                var report = reportEngine.Generate (
                    Guid.Empty,
                    ReportNames.NotCompletedAssessment,
                    parameters
                    );
                response.Report = report;
                var assessmentName = request.NotCompletedAssessmentParametersDto.AssessmentName;
                if ( string.IsNullOrWhiteSpace ( assessmentName ) )
                {
                    assessmentName = Report.All;
                }
                new RecentReport(
                    ReportNames.NotCompletedAssessment,
                    UserContext.Current.SystemAccountKey.Value,
                    assessmentName,
                    DateTime.Now,
                    parameters);
            }
        }

        #endregion
    }
}