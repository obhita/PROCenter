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

namespace ProCenter.Domain.MessageModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Pillar.Common.Utility;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.MessageModule.Event;

    #endregion

    /// <summary>The workflow message class.</summary>
    public class WorkflowMessage : AggregateRootBase, IMessage
    {
        #region Fields

        private readonly Dictionary<string, Guid> _workflowAssessments = new Dictionary<string, Guid> ();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowMessage"/> class.
        /// </summary>
        public WorkflowMessage ()
        {
            WorkflowReports = new List<ReportModel> ();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowMessage"/> class.
        /// </summary>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="initiatingAssessmentKey">The initiating assessment key.</param>
        /// <param name="initiatingAssessmentCode">The initiating assessment code.</param>
        /// <param name="recommendedAssessmentDefinitionKey">The recommended assessment definition key.</param>
        /// <param name="recommendedAssessmentDefinitionCode">The recommended assessment definition code.</param>
        /// <param name="initiatingAssessmentScore">The initiating assessment score.</param>
        public WorkflowMessage (
            Guid patientKey,
            Guid initiatingAssessmentKey,
            string initiatingAssessmentCode,
            Guid recommendedAssessmentDefinitionKey,
            string recommendedAssessmentDefinitionCode,
            Score initiatingAssessmentScore )
        {
            Key = CombGuid.NewCombGuid ();
            RaiseEvent (
                        new WorkflowMessageCreatedEvent (
                            Key,
                            MessageType,
                            patientKey,
                            WorkflowMessageStatus.WaitingForResponse,
                            initiatingAssessmentKey,
                            initiatingAssessmentCode,
                            recommendedAssessmentDefinitionKey,
                            recommendedAssessmentDefinitionCode,
                            initiatingAssessmentScore ) );
            WorkflowReports = new List<ReportModel> ();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether [for self administration].
        /// </summary>
        /// <value>
        /// <c>true</c> if [for self administration]; otherwise, <c>false</c>.
        /// </value>
        public bool ForSelfAdministration { get; private set; }

        /// <summary>
        /// Gets the initiating assessment code.
        /// </summary>
        /// <value>
        /// The initiating assessment code.
        /// </value>
        public string InitiatingAssessmentCode { get; private set; }

        /// <summary>
        /// Gets the initiating assessment key.
        /// </summary>
        /// <value>
        /// The initiating assessment key.
        /// </value>
        public Guid InitiatingAssessmentKey { get; private set; }

        /// <summary>
        /// Gets the initiating assessment score.
        /// </summary>
        /// <value>
        /// The initiating assessment score.
        /// </value>
        public Score InitiatingAssessmentScore { get; private set; }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        public MessageType MessageType
        {
            get { return MessageType.RecommendAssessment; }
        }

        /// <summary>
        /// Gets the patient key.
        /// </summary>
        /// <value>
        /// The patient key.
        /// </value>
        public Guid PatientKey { get; private set; }

        /// <summary>
        /// Gets the recommended assessment definition code.
        /// </summary>
        /// <value>
        /// The recommended assessment definition code.
        /// </value>
        public string RecommendedAssessmentDefinitionCode { get; private set; }

        /// <summary>
        /// Gets the recommended assessment definition key.
        /// </summary>
        /// <value>
        /// The recommended assessment definition key.
        /// </value>
        public Guid RecommendedAssessmentDefinitionKey { get; private set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public WorkflowMessageStatus Status { get; private set; }

        /// <summary>
        /// Gets the workflow reports.
        /// </summary>
        /// <value>
        /// The workflow reports.
        /// </value>
        public IEnumerable<ReportModel> WorkflowReports { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>Administers the assessment.</summary>
        public void AdministerAssessment ()
        {
            if ( Status == WorkflowMessageStatus.WaitingForResponse )
            {
                RaiseEvent ( new WorkflowMessageStatusChangedEvent ( Key, MessageType, WorkflowMessageStatus.InProgress ) );
            }
            else
            {
                //TODO: throw error
            }
        }

        /// <summary>Advances the specified initiating assessment key.</summary>
        /// <param name="initiatingAssessmentKey">The initiating assessment key.</param>
        /// <param name="initiatingAssessmentCode">The initiating assessment code.</param>
        /// <param name="recommendedAssessmentDefinitionKey">The recommended assessment definition key.</param>
        /// <param name="recommendedAssessmentDefinitionCode">The recommended assessment definition code.</param>
        /// <param name="initiatingAssessmentScore">The initiating assessment score.</param>
        public void Advance (
            Guid initiatingAssessmentKey,
            string initiatingAssessmentCode,
            Guid recommendedAssessmentDefinitionKey,
            string recommendedAssessmentDefinitionCode,
            Score initiatingAssessmentScore )
        {
            if ( Status == WorkflowMessageStatus.InProgress )
            {
                RaiseEvent (
                            new WorkflowMessageAdvancedEvent (
                                Key,
                                MessageType,
                                initiatingAssessmentKey,
                                initiatingAssessmentCode,
                                recommendedAssessmentDefinitionKey,
                                recommendedAssessmentDefinitionCode,
                                initiatingAssessmentScore ) );

                RaiseEvent ( new WorkflowMessageStatusChangedEvent ( Key, MessageType, WorkflowMessageStatus.WaitingForResponse ) );
            }
        }

        /// <summary>Allows for self administration.</summary>
        public void AllowSelfAdministration ()
        {
            RaiseEvent ( new MessageForSelfAdministrationEvent ( Key, MessageType ) );
        }

        /// <summary>Completes the specified workflow reports.</summary>
        /// <param name="workflowReports">The workflow reports.</param>
        public void Complete ( params ReportModel[] workflowReports )
        {
            if ( Status == WorkflowMessageStatus.InProgress )
            {
                RaiseEvent ( new WorkflowMessageStatusChangedEvent ( Key, MessageType, WorkflowMessageStatus.Complete ) );
                if ( workflowReports != null )
                {
                    foreach ( var workflowReport in workflowReports )
                    {
                        RaiseEvent ( new WorkflowMessageReportReadyEvent ( Key, MessageType, workflowReport, PatientKey ) );
                    }
                }
            }
        }

        /// <summary>Gets the assessment keyfor code in workflow.</summary>
        /// <param name="assessmentCode">The assessment code.</param>
        /// <returns>A <see cref="Nullable{Guid}"/>.</returns>
        public Guid? GetAssessmentKeyforCodeInWorkflow ( string assessmentCode )
        {
            if ( _workflowAssessments.ContainsKey ( assessmentCode ) )
            {
                return _workflowAssessments[assessmentCode];
            }
            return null;
        }

        /// <summary>Rejects this instance.</summary>
        public void Reject ()
        {
            if ( Status == WorkflowMessageStatus.WaitingForResponse )
            {
                RaiseEvent ( new WorkflowMessageStatusChangedEvent ( Key, MessageType, WorkflowMessageStatus.Rejected ) );
            }
            else
            {
                //TODO: throw error
            }
        }

        /// <summary>Updates the report item.</summary>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="name">The name.</param>
        /// <param name="shouldShow">The should show.</param>
        /// <param name="text">The text.</param>
        public void UpdateReportItem ( string reportName, string name, bool? shouldShow, string text )
        {
            RaiseEvent ( new WorkflowMessageReportItemUpdatedEvent ( Key, MessageType, reportName, name, shouldShow, text ) );
        }

        #endregion

        #region Methods

        private void Apply ( MessageForSelfAdministrationEvent messageForSelfAdministrationEvent )
        {
            ForSelfAdministration = true;
        }

        private void Apply ( WorkflowMessageReportItemUpdatedEvent workflowMessageReportItemUpdatedEvent )
        {
            var report = WorkflowReports.FirstOrDefault ( r => r.Name == workflowMessageReportItemUpdatedEvent.ReportName );
            if ( report != null )
            {
                report.UpdateReportItem ( 
                    workflowMessageReportItemUpdatedEvent.Name, 
                    workflowMessageReportItemUpdatedEvent.ShouldShow, 
                    workflowMessageReportItemUpdatedEvent.Text );
            }
        }

        private void Apply ( WorkflowMessageReportReadyEvent workflowMessageReportReadyEvent )
        {
            ( WorkflowReports as IList<ReportModel> ).Add ( workflowMessageReportReadyEvent.WorkflowReport );
        }

        private void Apply ( WorkflowMessageStatusChangedEvent workflowMessageStatusChangedEvent )
        {
            Status = workflowMessageStatusChangedEvent.Status;
        }

        private void Apply ( WorkflowMessageCreatedEvent workflowMessageCreatedEvent )
        {
            PatientKey = workflowMessageCreatedEvent.PatientKey;
            InitiatingAssessmentKey = workflowMessageCreatedEvent.InitiatingAssessmentKey;
            InitiatingAssessmentCode = workflowMessageCreatedEvent.InitiatingAssessmentCode;
            RecommendedAssessmentDefinitionKey = workflowMessageCreatedEvent.RecommendedAssessmentDefinitionKey;
            RecommendedAssessmentDefinitionCode = workflowMessageCreatedEvent.RecommendedAssessmentDefinitionCode;
            InitiatingAssessmentScore = workflowMessageCreatedEvent.InitiatingAssessmentScore;
            _workflowAssessments.Add ( InitiatingAssessmentCode, InitiatingAssessmentKey );
            if ( RecommendedAssessmentDefinitionCode == null )
            {
                Status = WorkflowMessageStatus.InProgress;
            }
        }

        private void Apply ( WorkflowMessageAdvancedEvent workflowMessageAdvancedEvent )
        {
            InitiatingAssessmentKey = workflowMessageAdvancedEvent.InitiatingAssessmentKey;
            InitiatingAssessmentCode = workflowMessageAdvancedEvent.InitiatingAssessmentCode;
            RecommendedAssessmentDefinitionKey = workflowMessageAdvancedEvent.RecommendedAssessmentDefinitionKey;
            RecommendedAssessmentDefinitionCode = workflowMessageAdvancedEvent.RecommendedAssessmentDefinitionCode;
            InitiatingAssessmentScore = workflowMessageAdvancedEvent.InitiatingAssessmentScore;
            _workflowAssessments.Add ( InitiatingAssessmentCode, InitiatingAssessmentKey );
        }

        #endregion
    }
}