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
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    using Pillar.Common.Utility;
    using Pillar.Domain.Primitives;

    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.MessageModule.Event;

    #endregion

    /// <summary>The assessment reminder class.</summary>
    public sealed class AssessmentReminder : AggregateRootBase, IMessage
    {
        #region Static Fields

        private static Dictionary<string, PropertyInfo> _propertyCache;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentReminder"/> class.
        /// </summary>
        public AssessmentReminder ()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentReminder" /> class.
        /// </summary>
        /// <param name="organizationKey">The organization key.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="createdByStaffKey">The created by staff key.</param>
        /// <param name="assessmentDefinitionKey">The assessment definition key.</param>
        /// <param name="title">The title.</param>
        /// <param name="start">The start.</param>
        /// <param name="description">The description.</param>
        /// <param name="reminderRecurrence">The reminder recurrence.</param>
        /// <param name="end">The end.</param>
        public AssessmentReminder (
            Guid organizationKey,
            Guid patientKey,
            Guid createdByStaffKey,
            Guid assessmentDefinitionKey,
            string title,
            DateTime start,
            string description,
            AssessmentReminderRecurrence reminderRecurrence,
            DateTime? end)
        {
            OrganizationKey = organizationKey;
            Key = CombGuid.NewCombGuid ();

            RaiseEvent (
                        new AssessmentReminderCreatedEvent (
                            Key,
                            MessageType,
                            organizationKey,
                            patientKey,
                            createdByStaffKey,
                            assessmentDefinitionKey,
                            title,
                            start,
                            end ?? start,
                            description,
                            AssessmentReminderStatus.Default,
                            reminderRecurrence ) );
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the alert sent date.
        /// </summary>
        /// <value>
        /// The alert sent date.
        /// </value>
        public DateTime? AlertSentDate { get; private set; }

        /// <summary>
        /// Gets the assessment definition key.
        /// </summary>
        /// <value>
        /// The assessment definition key.
        /// </value>
        public Guid AssessmentDefinitionKey { get; private set; }

        /// <summary>
        /// Gets the created by staff key.
        /// </summary>
        /// <value>
        /// The created by staff key.
        /// </value>
        public Guid CreatedByStaffKey { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; private set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        public DateTime? End { get; set; }

        /// <summary>
        /// Gets a value indicating whether [for self administration].
        /// </summary>
        /// <value>
        /// <c>true</c> if [for self administration]; otherwise, <c>false</c>.
        /// </value>
        public bool ForSelfAdministration { get; private set; }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        public MessageType MessageType
        {
            get { return MessageType.AssessmentReminder; }
        }

        /// <summary>
        /// Gets or sets the organization key.
        /// </summary>
        /// <value>
        /// The organization key.
        /// </value>
        public Guid OrganizationKey { get; set; }

        /// <summary>
        /// Gets the patient key.
        /// </summary>
        /// <value>
        /// The patient key.
        /// </value>
        public Guid PatientKey { get; private set; }

        /// <summary>
        /// Gets the reminder recurrence.
        /// </summary>
        /// <value>
        /// The reminder recurrence.
        /// </value>
        public AssessmentReminderRecurrence ReminderRecurrence { get; private set; }

        /// <summary>
        /// Gets the reminder time.
        /// </summary>
        /// <value>
        /// The reminder time.
        /// </value>
        public double ReminderTime { get; private set; }

        /// <summary>
        /// Gets the reminder unit.
        /// </summary>
        /// <value>
        /// The reminder unit.
        /// </value>
        public AssessmentReminderUnit ReminderUnit { get; private set; }

        /// <summary>
        /// Gets the send to email.
        /// </summary>
        /// <value>
        /// The send to email.
        /// </value>
        public Email SendToEmail { get; private set; }

        /// <summary>
        /// Gets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        public DateTime Start { get; private set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public AssessmentReminderStatus Status { get; private set; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the assessment instance key.
        /// </summary>
        /// <value>
        /// The assessment instance key.
        /// </value>
        public Guid? AssessmentInstanceKey { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Acknowledges this instance.
        /// </summary>
        /// <param name="assessmentReminderKey">The assessment reminder key.</param>
        public void Acknowledge ( Guid assessmentReminderKey)
        {
            ReviseStatus ( assessmentReminderKey, AssessmentReminderStatus.Acknowledge );
        }

        /// <summary>Allows for self administration.</summary>
        public void AllowSelfAdministration ()
        {
            RaiseEvent ( new MessageForSelfAdministrationEvent ( Key, MessageType ) );
        }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        /// <param name="assessmentReminderKey">The assessment reminder key.</param>
        public void Cancel ( Guid assessmentReminderKey )
        {
            ReviseStatus ( assessmentReminderKey, AssessmentReminderStatus.Cancelled );
        }

        /// <summary>Doesnt allow for self administration.</summary>
        public void DontAllowSelfAdministration ()
        {
            RaiseEvent ( new MessageNotForSelfAdministrationEvent ( Key, MessageType ) );
        }

        /// <summary>
        /// Revises the assessment instance key.
        /// </summary>
        /// <param name="assessmentInstanceKey">The assessment instance key.</param>
        /// <param name="assessmentReminderKey">The assessment reminder key.</param>
        public void ReviseAssessmentInstanceKey ( Guid assessmentInstanceKey, Guid assessmentReminderKey )
        {
            RaiseEvent(new AdministerAssessmentNowEvent(Key, Version, assessmentInstanceKey, assessmentReminderKey));
        }

        /// <summary>Revises the alert sent date.</summary>
        /// <param name="alertSentDate">The alert sent date.</param>
        public void ReviseAlertSentDate ( DateTime? alertSentDate )
        {
            RaiseEvent ( new AssessmentReminderUpdatedEvent ( Key, Version, a => a.AlertSentDate, alertSentDate ) );
        }

        /// <summary>Revises the assessment definition key.</summary>
        /// <param name="assessmentDefinitionKey">The assessment definition key.</param>
        public void ReviseAssessmentDefinitionKey ( Guid assessmentDefinitionKey )
        {
            RaiseEvent ( new AssessmentReminderUpdatedEvent ( Key, Version, a => a.AssessmentDefinitionKey, assessmentDefinitionKey ) );
        }

        /// <summary>Revises the createdby staff key.</summary>
        /// <param name="createdByStaffkey">The created by staffkey.</param>
        public void ReviseCreatedbyStaffKey ( Guid createdByStaffkey )
        {
            RaiseEvent ( new AssessmentReminderUpdatedEvent ( Key, Version, a => a.CreatedByStaffKey, createdByStaffkey ) );
        }

        /// <summary>Revises the description.</summary>
        /// <param name="description">The description.</param>
        public void ReviseDescription ( string description )
        {
            RaiseEvent ( new AssessmentReminderUpdatedEvent ( Key, Version, a => a.Description, description ) );
        }

        /// <summary>Revises the patient key.</summary>
        /// <param name="patientKey">The patient key.</param>
        public void RevisePatientKey ( Guid patientKey )
        {
            RaiseEvent ( new AssessmentReminderUpdatedEvent ( Key, Version, a => a.PatientKey, patientKey ) );
        }

        /// <summary>
        /// Revises the assessment instance key.
        /// </summary>
        /// <param name="assessmentInstanceKey">The assessment instance key.</param>
        public void ReviseAssessmentInstanceKey(Guid? assessmentInstanceKey)
        {
            RaiseEvent(new AssessmentReminderUpdatedEvent(Key, Version, a => a.AssessmentInstanceKey, assessmentInstanceKey));
        }

        /// <summary>Revises the recurrence.</summary>
        /// <param name="recurrence">The recurrence.</param>
        /// <param name="end">The end.</param>
        public void ReviseRecurrence ( AssessmentReminderRecurrence recurrence, DateTime? end )
        {
            RaiseEvent ( new AssessmentRecurrenceRevisedEvent ( Key, Version, recurrence, end ) );
        }

        /// <summary>Revises the reminder.</summary>
        /// <param name="reminderTime">The reminder time.</param>
        /// <param name="reminderUnit">The reminder unit.</param>
        /// <param name="sendToEmail">The send to email.</param>
        public void ReviseReminder ( double reminderTime, AssessmentReminderUnit reminderUnit, Email sendToEmail = null )
        {
            RaiseEvent ( new AssessmentReminderRevisedEvent ( Key, Version, reminderTime, reminderUnit, sendToEmail ) );
            if ( AlertSentDate.HasValue )
            {
                var thresholdDate = reminderUnit == AssessmentReminderUnit.Days
                    ? Start.AddDays (-Convert.ToInt32 ( reminderTime ) )
                    : Start.AddDays (-Convert.ToInt32 ( reminderTime * 7 ) );
                if ( DateTime.Now < thresholdDate )
                {
                    RaiseEvent ( new AssessmentReminderUpdatedEvent ( Key, Version, a => a.AlertSentDate, null ) );
                }
            }
        }

        /// <summary>Revises the start.</summary>
        /// <param name="start">The start.</param>
        public void ReviseStart ( DateTime start )
        {
            RaiseEvent ( new AssessmentReminderUpdatedEvent ( Key, Version, a => a.Start, start ) );
            if ( AlertSentDate.HasValue )
            {
                var thresholdDate = ReminderUnit == AssessmentReminderUnit.Days
                    ? start.AddDays (-Convert.ToInt32 ( ReminderTime ) )
                    : start.AddDays (-Convert.ToInt32 ( ReminderTime * 7 ) );
                if ( DateTime.Now < thresholdDate )
                {
                    RaiseEvent ( new AssessmentReminderUpdatedEvent ( Key, Version, a => a.AlertSentDate, null ) );
                }
            }
        }

        /// <summary>Revises the title.</summary>
        /// <param name="title">The title.</param>
        public void ReviseTitle ( string title )
        {
            RaiseEvent ( new AssessmentReminderUpdatedEvent ( Key, Version, a => a.Title, title ) );
        }

        #endregion

        #region Methods

        private static Type GetConvertToType ( Type propertyType )
        {
            var convertToType = propertyType;
            var underlyingType = Nullable.GetUnderlyingType ( convertToType );
            if ( underlyingType != null )
            {
                convertToType = underlyingType;
            }
            return convertToType;
        }

        private void Apply ( MessageForSelfAdministrationEvent messageForSelfAdministrationEvent )
        {
            ForSelfAdministration = true;
        }

        private void Apply ( MessageNotForSelfAdministrationEvent messageNotForSelfAdministrationEvent )
        {
            ForSelfAdministration = false;
        }

        private void Apply ( AssessmentReminderRevisedEvent assessmentReminderRevisedEvent )
        {
            ReminderTime = assessmentReminderRevisedEvent.Time;
            ReminderUnit = assessmentReminderRevisedEvent.Unit;
            SendToEmail = assessmentReminderRevisedEvent.SendToEmail;
        }

        private void Apply ( AssessmentRecurrenceRevisedEvent assessmentRecurrenceRevisedEvent )
        {
            ReminderRecurrence = assessmentRecurrenceRevisedEvent.Recurrence;
            End = assessmentRecurrenceRevisedEvent.End;
        }

        private void Apply(AdministerAssessmentNowEvent administerAssessmentNowEvent)
        {
            Status = AssessmentReminderStatus.Acknowledge;
        }

        private void Apply ( AssessmentReminderCreatedEvent assessmentReminderCreatedEvent )
        {
            OrganizationKey = assessmentReminderCreatedEvent.OrganizationKey;
            PatientKey = assessmentReminderCreatedEvent.PatientKey;
            CreatedByStaffKey = assessmentReminderCreatedEvent.CreatedByStaffKey;
            AssessmentDefinitionKey = assessmentReminderCreatedEvent.AssessmentDefinitionKey;
            Title = assessmentReminderCreatedEvent.Title;
            Start = assessmentReminderCreatedEvent.Start;
            End = assessmentReminderCreatedEvent.End;
            Description = assessmentReminderCreatedEvent.Description;
            Status = assessmentReminderCreatedEvent.Status;
            ReminderRecurrence = assessmentReminderCreatedEvent.Recurrence;
        }

        private void Apply ( AssessmentReminderUpdatedEvent assessmentReminderUpdatedEvent )
        {
            var propertyName = assessmentReminderUpdatedEvent.Property;
            var value = assessmentReminderUpdatedEvent.Value;

            if ( _propertyCache == null )
            {
                _propertyCache = GetType ().GetProperties ( BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy ).ToDictionary ( pi => pi.Name );
            }
            var property = _propertyCache.ContainsKey ( propertyName ) ? _propertyCache[propertyName] : null;
            if ( property == null )
            {
                throw new InvalidOperationException ( string.Format ( "Invalid property name {0}", propertyName ) );
            }
            if ( value != null && !property.PropertyType.IsInstanceOfType ( value ) )
            {
                var convertToType = GetConvertToType ( property.PropertyType );
                if ( value is string )
                {
                    value = TypeDescriptor.GetConverter ( convertToType ).ConvertFromInvariantString ( value.ToString () );
                }
                else
                {
                    value = Convert.ChangeType ( value, convertToType );
                }
            }
            property.SetValue ( this, value );
        }

        private void Apply(AssessmentReminderStatusChangedEvent @event)
        {
            Status = @event.Status;
        }

        private void ReviseStatus ( Guid assessmentReminderKey, AssessmentReminderStatus status )
        {
            RaiseEvent ( new AssessmentReminderStatusChangedEvent( Key, Version, assessmentReminderKey, status ) );
        }

        #endregion
    }
}