#region License Header

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

#endregion

namespace ProCenter.Domain.Nida
{
    #region Using Statements

    using System;
    using System.Linq;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.MessageModule;

    #endregion

    /// <summary>The nida report engine class.</summary>
    [ReportEngine ( ReportNames.NidaPatientSummaryReport )]
    public class NidaReportEngine : IReportEngine
    {
        #region Fields

        private readonly IWorkflowMessageRepository _workflowMessageRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NidaReportEngine"/> class.
        /// </summary>
        /// <param name="workflowMessageRepository">The workflow message repository.</param>
        public NidaReportEngine ( IWorkflowMessageRepository workflowMessageRepository )
        {
            _workflowMessageRepository = workflowMessageRepository;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Generates the specified workflow key.</summary>
        /// <param name="workflowKey">The workflow key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="IReport"/>.</returns>
        public IReport Generate ( Guid workflowKey, string reportName, object parameters = null )
        {
            var workflow = _workflowMessageRepository.GetByKey ( workflowKey );
            var reportModel = workflow.WorkflowReports.FirstOrDefault ( r => r.Name == reportName );
            if ( reportModel != null )
            {
                NidaPatientSummaryReportModelBuilder.FillDefaults ( reportModel );
            }

            //todo: get report data from event store
            var nidaReportDataCollection = new NidaReportDataCollection
                                           {
                                               reportModel == null ? new NidaReportData () : new NidaReportData ( reportModel )
                                           };
            var report = new NidaReport
                         {
                             DataSource = nidaReportDataCollection,
                         };
            return report;
        }

        /// <summary>
        /// Gets the customization model.
        /// </summary>
        /// <param name="workflowKey">The workflow key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <returns>
        /// A <see cref="IReportModel" />.
        /// </returns>
        public IReportModel GetCustomizationModel(Guid workflowKey, string reportName, Guid? patientKey = null)
        {
            var reportModel = _workflowMessageRepository.GetByKey ( workflowKey ).WorkflowReports.FirstOrDefault ( r => r.Name == reportName );
            if ( reportModel != null )
            {
                NidaPatientSummaryReportModelBuilder.FillDefaults ( reportModel );
            }
            return reportModel;
        }

        /// <summary>
        /// Updates the customization model.
        /// </summary>
        /// <param name="workflowKey">The workflow key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="name">The name.</param>
        /// <param name="shouldShow">The should show.</param>
        /// <param name="text">The text.</param>
        public void UpdateCustomizationModel ( Guid workflowKey, string reportName, string name, bool? shouldShow, string text )
        {
            var workflowMessage = _workflowMessageRepository.GetByKey ( workflowKey );
            var reportItem = workflowMessage.WorkflowReports.First ( r => r.Name == reportName ).FindReportItem ( name );
            var defaultText = ( reportItem.FormatParameters.Any ()
                ? string.Format (
                                 NidaWorkflowPatientSummaryReport.ResourceManager.GetString ( reportItem.Name ) ?? string.Empty,
                    reportItem.FormatParameters.ToArray () )
                : NidaWorkflowPatientSummaryReport.ResourceManager.GetString ( reportItem.Name ) ) ?? string.Empty;
            if ( defaultText.Equals ( text ) )
            {
                text = null;
            }
            workflowMessage.UpdateReportItem ( reportName, name, shouldShow, text );
        }
        
        #endregion
    }
}