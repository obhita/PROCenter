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

namespace ProCenter.Domain.AssessmentModule.Event
{
    #region Using Statements

    using System;

    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>Event when assessment is created.</summary>
    public class AssessmentCreatedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentCreatedEvent" /> class.
        /// </summary>
        /// <param name="assessmentInstanceKey">The assessment instance key.</param>
        /// <param name="version">The version.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="staffKey">The staff key.</param>
        /// <param name="assessmentDefinitionKey">The assessment definition key.</param>
        /// <param name="assessmentName">The assessment name.</param>
        /// <param name="requiredQuestionTotal">The question total.</param>
        /// <param name="createdDate">The created date.</param>
        /// <param name="canSelfAdminister">If set to <c>true</c> [can self administer].</param>
        public AssessmentCreatedEvent (
            Guid assessmentInstanceKey,
            int version,
            Guid patientKey,
            Guid? staffKey,
            Guid assessmentDefinitionKey,
            string assessmentName,
            int requiredQuestionTotal,
            DateTime createdDate,
            bool canSelfAdminister)
            : base ( assessmentInstanceKey, version )
        {
            PatientKey = patientKey;
            StaffKey = staffKey;
            AssessmentDefinitionKey = assessmentDefinitionKey;
            AssessmentName = assessmentName;
            RequiredQuestionTotal = requiredQuestionTotal;
            CreatedDate = createdDate;
            CanSelfAdminister = canSelfAdminister;
        }

        #endregion

        #region Public Properties

        /// <summary>Gets the assessment definition key.</summary>
        /// <value>The assessment definition key.</value>
        public Guid AssessmentDefinitionKey { get; private set; }

        /// <summary>Gets the name of the assessment.</summary>
        /// <value>The name of the assessment.</value>
        public string AssessmentName { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance can self administer.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can self administer; otherwise, <c>false</c>.
        /// </value>
        public bool CanSelfAdminister { get; private set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime CreatedDate { get; set; }

        /// <summary>Gets the required question total.</summary>
        /// <value>The required question total.</value>
        public int RequiredQuestionTotal { get; private set; }

        /// <summary>Gets the patient key.</summary>
        /// <value>The patient key.</value>
        public Guid PatientKey { get; private set; }

        /// <summary>
        /// Gets the staff key.
        /// </summary>
        /// <value>
        /// The staff key.
        /// </value>
        public Guid? StaffKey { get; private set; }

        #endregion
    }
}