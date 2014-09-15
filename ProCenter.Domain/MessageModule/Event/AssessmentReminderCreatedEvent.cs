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

    #endregion

    /// <summary>The assessment reminder created event class.</summary>
    public class AssessmentReminderCreatedEvent : MessageEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentReminderCreatedEvent"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="organizationKey">The organization key.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="createdByStaffKey">The created by staff key.</param>
        /// <param name="assessmentDefinitionKey">The assessment definition key.</param>
        /// <param name="title">The title.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="description">The description.</param>
        /// <param name="status">The status.</param>
        /// <param name="recurrence">The recurrence.</param>
        public AssessmentReminderCreatedEvent (
            Guid key,
            MessageType messageType,
            Guid organizationKey,
            Guid patientKey,
            Guid createdByStaffKey,
            Guid assessmentDefinitionKey,
            string title,
            DateTime start,
            DateTime? end,
            string description,
            AssessmentReminderStatus status,
            AssessmentReminderRecurrence recurrence
            )
            : base ( key, messageType )
        {
            OrganizationKey = organizationKey;
            PatientKey = patientKey;
            CreatedByStaffKey = createdByStaffKey;
            AssessmentDefinitionKey = assessmentDefinitionKey;
            Title = title;
            Start = start;
            End = end;
            Description = description;
            Status = status;
            Recurrence = recurrence;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the assessment definition key.
        /// </summary>
        /// <value>
        /// The assessment definition key.
        /// </value>
        public Guid AssessmentDefinitionKey { get; set; }

        /// <summary>
        /// Gets or sets the created by staff key.
        /// </summary>
        /// <value>
        /// The created by staff key.
        /// </value>
        public Guid CreatedByStaffKey { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        public DateTime? End { get; set; }

        /// <summary>
        /// Gets or sets the organization key.
        /// </summary>
        /// <value>
        /// The organization key.
        /// </value>
        public new Guid OrganizationKey { get; set; }

        /// <summary>
        /// Gets or sets the patient key.
        /// </summary>
        /// <value>
        /// The patient key.
        /// </value>
        public Guid PatientKey { get; set; }

        /// <summary>
        /// Gets or sets the recurrence.
        /// </summary>
        /// <value>
        /// The recurrence.
        /// </value>
        public AssessmentReminderRecurrence Recurrence { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        public DateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public AssessmentReminderStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        #endregion
    }
}