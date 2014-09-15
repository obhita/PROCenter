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

namespace ProCenter.Service.Message.Message
{
    #region Using Statements

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using ProCenter.Domain.MessageModule;
    using ProCenter.Service.Message.Common;

    #endregion

    /// <summary>The assessment reminder dto class.</summary>
    public class AssessmentReminderDto : KeyedDataTransferObject, IMessageDto
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the alert sent date.
        /// </summary>
        /// <value>
        /// The alert sent date.
        /// </value>
        public DateTime? AlertSentDate { get; set; }

        /// <summary>
        /// Gets or sets the assessment code.
        /// </summary>
        /// <value>
        /// The assessment code.
        /// </value>
        [ScaffoldColumn ( false )]
        public string AssessmentCode { get; set; }

        /// <summary>
        /// Gets or sets the assessment definition key.
        /// </summary>
        /// <value>
        /// The assessment definition key.
        /// </value>
        [Required]
        public Guid? AssessmentDefinitionKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the assessment.
        /// </summary>
        /// <value>
        /// The name of the assessment.
        /// </value>
        [ScaffoldColumn ( false )]
        public string AssessmentName { get; set; }

        /// <summary>
        /// Gets or sets the created by staff key.
        /// </summary>
        /// <value>
        /// The created by staff key.
        /// </value>
        [HiddenInput ( DisplayValue = false )]
        public Guid? CreatedByStaffKey { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [ScaffoldColumn ( false )]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        [DisplayFormat ( DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true )]
        public DateTime? End { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [for self administration].
        /// </summary>
        /// <value>
        /// <c>true</c> if [for self administration]; otherwise, <c>false</c>.
        /// </value>
        [ScaffoldColumn ( false )]
        public bool ForSelfAdministration { get; set; }

        /// <summary>
        /// Gets or sets the organization key.
        /// </summary>
        /// <value>
        /// The organization key.
        /// </value>
        [ScaffoldColumn ( false )]
        public Guid? OrganizationKey { get; set; }

        /// <summary>
        /// Gets or sets the first name of the patient.
        /// </summary>
        /// <value>
        /// The first name of the patient.
        /// </value>
        [ScaffoldColumn ( false )]
        public string PatientFirstName { get; set; }

        /// <summary>
        /// Gets or sets the patient key.
        /// </summary>
        /// <value>
        /// The patient key.
        /// </value>
        [Required]
        public Guid? PatientKey { get; set; }

        /// <summary>
        /// Gets or sets the last name of the patient.
        /// </summary>
        /// <value>
        /// The last name of the patient.
        /// </value>
        [ScaffoldColumn ( false )]
        public string PatientLastName { get; set; }

        /// <summary>
        /// Gets or sets the reminder recurrence.
        /// </summary>
        /// <value>
        /// The reminder recurrence.
        /// </value>
        [Display ( Name = " " )]
        public AssessmentReminderRecurrence ReminderRecurrence { get; set; }

        /// <summary>
        /// Gets or sets the reminder time.
        /// </summary>
        /// <value>
        /// The reminder time.
        /// </value>
        [Required]
        public double ReminderTime { get; set; }

        /// <summary>
        /// Gets or sets the reminder unit.
        /// </summary>
        /// <value>
        /// The reminder unit.
        /// </value>
        [Required]
        public AssessmentReminderUnit ReminderUnit { get; set; }

        /// <summary>
        /// Gets or sets the send to email.
        /// </summary>
        /// <value>
        /// The send to email.
        /// </value>
        public string SendToEmail { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        [DisplayFormat ( DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true )]
        [Required]
        public DateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [ScaffoldColumn ( false )]
        public AssessmentReminderStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the assessment instance key.
        /// </summary>
        /// <value>
        /// The assessment instance key.
        /// </value>
        public Guid? AssessmentInstanceKey { get; set; }

        /// <summary>
        /// Gets or sets the system account key.
        /// </summary>
        /// <value>
        /// The system account key.
        /// </value>
        public Guid? SystemAccountKey { get; set; }

        /// <summary>
        /// Gets or sets the recurrence key.
        /// </summary>
        /// <value>
        /// The recurrence key.
        /// </value>
        public Guid? RecurrenceKey { get; set; }

        #endregion
    }
}