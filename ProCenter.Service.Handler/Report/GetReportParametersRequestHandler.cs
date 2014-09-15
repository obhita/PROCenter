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

    using Pillar.Common.InversionOfControl;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Service.Handler.Common;
    using ProCenter.Service.Message.Report;

    using global::AutoMapper;

    #endregion

    /// <summary>The get report parameters request handler class.</summary>
    public class GetReportParametersRequestHandler : ServiceRequestHandler<GetReportParametersRequest, ReportParametersResponse>
    {
        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( GetReportParametersRequest request, ReportParametersResponse response )
        {
            var reportEngine = IoC.CurrentContainer.Resolve<IReportEngine> ( request.ReportName );
            var parameters = reportEngine.GetCustomizationModel ( request.TemplateKey.HasValue ? request.TemplateKey.Value : Guid.Empty, request.ReportName, request.PatientKey );
            
            var paramertersType = parameters.GetType ();
            var destinationType = Mapper.GetAllTypeMaps ().First ( typeMap => typeMap.SourceType == paramertersType ).DestinationType;
            response.Parameters = Mapper.Map ( parameters, paramertersType, destinationType );
        }

        #endregion
    }
}