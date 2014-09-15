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
    using System.Linq;

    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.ReportsModule;
    using ProCenter.Service.Handler.Common;
    using ProCenter.Service.Message.Report;

    using global::AutoMapper;

    #endregion

    /// <summary>The save report template request handler class.</summary>
    public class SaveReportTemplateRequestHandler : ServiceRequestHandler<SaveReportTemplateRequest, SaveReportTemplateResponse>
    {
        #region Fields

        private readonly ILookupProvider _lookupProvider;

        private readonly IReportTemplateRepository _reportTemplateRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SaveReportTemplateRequestHandler" /> class.
        /// </summary>
        /// <param name="reportTemplateRepository">The report template repository.</param>
        /// <param name="lookupProvider">The lookup provider.</param>
        public SaveReportTemplateRequestHandler ( IReportTemplateRepository reportTemplateRepository, ILookupProvider lookupProvider )
        {
            _reportTemplateRepository = reportTemplateRepository;
            _lookupProvider = lookupProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( SaveReportTemplateRequest request, SaveReportTemplateResponse response )
        {
            var reportTemplateDto = request.ReportTemplate;
            if ( reportTemplateDto.Key == Guid.Empty )
            {
                var reportTemplateFactory = new ReportTemplateFactory ();
                var paramertersType = reportTemplateDto.Parameters.GetType ();
                var destinationType = Mapper.GetAllTypeMaps ().First ( typeMap => typeMap.SourceType == paramertersType ).DestinationType;
                var parameters = Mapper.Map ( reportTemplateDto.Parameters, paramertersType, destinationType ) as BaseReportParameters;
                if ( parameters == null )
                {
                    return;
                }
                parameters.ReportName = reportTemplateDto.Name;
                var reportTemplate = reportTemplateFactory.Create (
                    reportTemplateDto.SystemAccountKey,
                    reportTemplateDto.Name,
                    reportTemplateDto.ReportType,
                    parameters,
                    _lookupProvider.Find<ReportState> ( reportTemplateDto.ReportState.Code ) );
                if ( reportTemplate != null )
                {
                    var dto = Mapper.Map<ReportTemplate, ReportTemplateDto> ( reportTemplate );
                    response.ReportTemplate = dto;
                }
            }
            else
            {
                //update
                var reportTemplate = _reportTemplateRepository.GetByKey ( reportTemplateDto.Key );
                if ( reportTemplate != null )
                {
                    reportTemplate.ChangeName ( reportTemplateDto.Name );
                    reportTemplate.ChangeReportType ( reportTemplateDto.ReportType );
                    reportTemplate.ChangeParameters ( reportTemplateDto.Parameters );
                    reportTemplate.ChangeReportState ( _lookupProvider.Find<ReportState> ( reportTemplateDto.ReportState.Code ) );
                    var dto = Mapper.Map<ReportTemplate, ReportTemplateDto> ( reportTemplate );
                    response.ReportTemplate = dto;
                }
            }
        }

        #endregion
    }
}