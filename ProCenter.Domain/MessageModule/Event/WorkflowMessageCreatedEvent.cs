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

namespace ProCenter.Domain.MessageModule.Event
{
    #region Using Statements

    using System;

    using ProCenter.Domain.AssessmentModule;

    #endregion

    /// <summary>The workflow message created event class.</summary>
    public class WorkflowMessageCreatedEvent : MessageEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowMessageCreatedEvent"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="workflowMessageStatus">The workflow message status.</param>
        /// <param name="initiatingAssessmentKey">The initiating assessment key.</param>
        /// <param name="initiatingAssessmentCode">The initiating assessment code.</param>
        /// <param name="recommendedAssessmentDefinitionKey">The recommended assessment definition key.</param>
        /// <param name="recommendedAssessmentDefinitionCode">The recommended assessment definition code.</param>
        /// <param name="initiatingAssessmentScore">The initiating assessment score.</param>
        public WorkflowMessageCreatedEvent (
            Guid key,
            MessageType messageType,
            Guid patientKey,
            WorkflowMessageStatus workflowMessageStatus,
            Guid initiatingAssessmentKey,
            string initiatingAssessmentCode,
            Guid recommendedAssessmentDefinitionKey,
            string recommendedAssessmentDefinitionCode,
            Score initiatingAssessmentScore )
            : base ( key, messageType )
        {
            PatientKey = patientKey;
            WorkflowMessageStatus = workflowMessageStatus;
            InitiatingAssessmentKey = initiatingAssessmentKey;
            InitiatingAssessmentCode = initiatingAssessmentCode;
            RecommendedAssessmentDefinitionKey = recommendedAssessmentDefinitionKey;
            RecommendedAssessmentDefinitionCode = recommendedAssessmentDefinitionCode;
            InitiatingAssessmentScore = initiatingAssessmentScore;
        }

        #endregion

        #region Public Properties

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
        /// Gets the workflow message status.
        /// </summary>
        /// <value>
        /// The workflow message status.
        /// </value>
        public WorkflowMessageStatus WorkflowMessageStatus { get; private set; }

        #endregion
    }
}