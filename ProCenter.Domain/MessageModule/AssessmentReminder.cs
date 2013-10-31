#region Licence Header
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
    #region

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using CommonModule;
    using Event;
    using Pillar.Common.Utility;
    using Pillar.Domain.Primitives;

    #endregion

    public class AssessmentReminder : AggregateRootBase, IMessage
    {
        public Guid OrganizationKey { get; set; }
        private static Dictionary<string, PropertyInfo> _propertyCache;

        public AssessmentReminder()
        {
        }

        public AssessmentReminder(Guid organizationKey,
                                         Guid patientKey,
                                         Guid createdByStaffKey,
                                         Guid assessmentDefinitionKey,
                                         string title,
                                         DateTime start,
                                         string description)
        {
            OrganizationKey = organizationKey;
            Key = CombGuid.NewCombGuid();

            RaiseEvent(new AssessmentReminderCreatedEvent(
                           Key,
                           MessageType,
                           organizationKey,
                           patientKey,
                           createdByStaffKey,
                           assessmentDefinitionKey,
                           title,
                           start,
                           description,
                           AssessmentReminderStatus.Default));
        }

        public Guid PatientKey { get; private set; }
        public Guid CreatedByStaffKey { get; private set; }
        public Guid AssessmentDefinitionKey { get; private set; }
        public string Title { get; private set; }
        public DateTime Start { get; private set; }
        public string Description { get; private set; }
        public AssessmentReminderStatus Status { get; private set; }

        public double ReminderTime { get; private set; }
        public AssessmentReminderUnit ReminderUnit { get; private set; }
        public Email SendToEmail { get; private set; }
        public DateTime? AlertSentDate { get; private set; }

        public bool ForSelfAdministration { get; private set; }

        public MessageType MessageType
        {
            get { return MessageType.AssessmentReminder; }
        }

        public void RevisePatientKey(Guid patientKey)
        {
            RaiseEvent(new AssessmentReminderUpdatedEvent(Key, Version, a => a.PatientKey, patientKey));
        }

        public void ReviseCreatedbyStaffKey(Guid createdByStaffkey)
        {
            RaiseEvent(new AssessmentReminderUpdatedEvent(Key, Version, a => a.CreatedByStaffKey, createdByStaffkey));
        }
        
        public void ReviseTitle(string title)
        {
            RaiseEvent(new AssessmentReminderUpdatedEvent(Key, Version, a => a.Title, title));
        }

        public void ReviseStart(DateTime start)
        {
            RaiseEvent(new AssessmentReminderUpdatedEvent(Key, Version, a => a.Start, start));
            if (AlertSentDate.HasValue) //mark for re-send
            {
                var thresholdDate = ReminderUnit == AssessmentReminderUnit.Days ? start.AddDays(-Convert.ToInt32(ReminderTime)) : start.AddDays(-Convert.ToInt32(ReminderTime*7));
                if (DateTime.Now < thresholdDate)
                {
                    RaiseEvent(new AssessmentReminderUpdatedEvent(Key, Version, a => a.AlertSentDate, null));
                }
            }
        }

        public void ReviseDescription(string description)
        {
            RaiseEvent(new AssessmentReminderUpdatedEvent(Key, Version, a => a.Description, description));
        }

        public void Acknowledge ()
        {
            ReviseStatus ( AssessmentReminderStatus.Acknowledge );
        }

        public void Cancel()
        {
            ReviseStatus(AssessmentReminderStatus.Cancelled);
        }

        private void ReviseStatus(AssessmentReminderStatus status)
        {
            RaiseEvent(new AssessmentReminderUpdatedEvent(Key, Version, a => a.Status, status));
        }

        public void ReviseAssessmentDefinitionKey(Guid assessmentDefinitionKey)
        {
            RaiseEvent(new AssessmentReminderUpdatedEvent(Key, Version, a => a.AssessmentDefinitionKey, assessmentDefinitionKey));
        }

        public void ReviseReminder(double reminderTime, AssessmentReminderUnit reminderUnit, Email sendToEmail = null)
        {
            RaiseEvent(new AssessmentReminderRevisedEvent(Key, Version, reminderTime, reminderUnit, sendToEmail));
            if (AlertSentDate.HasValue) //mark for re-send
            {
                var thresholdDate = reminderUnit == AssessmentReminderUnit.Days ? Start.AddDays(-Convert.ToInt32(reminderTime)) : Start.AddDays(-Convert.ToInt32(reminderTime*7));
                if (DateTime.Now < thresholdDate)
                {
                    RaiseEvent(new AssessmentReminderUpdatedEvent(Key, Version, a => a.AlertSentDate, null));
                }
            }
        }

        public void ReviseAlertSentDate(DateTime? alertSentDate)
        {
            RaiseEvent(new AssessmentReminderUpdatedEvent(Key, Version, a => a.AlertSentDate, alertSentDate));
        }

        public void AllowSelfAdministration()
        {
            RaiseEvent(new MessageForSelfAdministrationEvent(Key, MessageType));
        }

        private void Apply(MessageForSelfAdministrationEvent messageForSelfAdministrationEvent)
        {
            ForSelfAdministration = true;
        }

        private void Apply ( AssessmentReminderRevisedEvent assessmentReminderRevisedEvent )
        {
            ReminderTime = assessmentReminderRevisedEvent.Time;
            ReminderUnit = assessmentReminderRevisedEvent.Unit;
            SendToEmail = assessmentReminderRevisedEvent.SendToEmail;
        }

        private void Apply(AssessmentReminderCreatedEvent assessmentReminderCreatedEvent)
        {
            OrganizationKey = assessmentReminderCreatedEvent.OrganizationKey;
            PatientKey = assessmentReminderCreatedEvent.PatientKey;
            CreatedByStaffKey = assessmentReminderCreatedEvent.CreatedByStaffKey;
            AssessmentDefinitionKey = assessmentReminderCreatedEvent.AssessmentDefinitionKey;
            Title = assessmentReminderCreatedEvent.Title;
            Start = assessmentReminderCreatedEvent.Start;
            Description = assessmentReminderCreatedEvent.Description;
            Status = assessmentReminderCreatedEvent.Status;
        }

        private static Type GetConvertToType(Type propertyType)
        {
            var convertToType = propertyType;
            var underlyingType = Nullable.GetUnderlyingType(convertToType);
            if (underlyingType != null)
            {
                convertToType = underlyingType;
            }
            return convertToType;
        }

        private void Apply(AssessmentReminderUpdatedEvent assessmentReminderUpdatedEvent)
        {
            var propertyName = assessmentReminderUpdatedEvent.Property;
            var value = assessmentReminderUpdatedEvent.Value;

            if (_propertyCache == null)
            {
                _propertyCache = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy).ToDictionary(pi => pi.Name);
            }
            var property = _propertyCache.ContainsKey(propertyName) ? _propertyCache[propertyName] : null;
            if (property == null)
            {
                throw new InvalidOperationException(string.Format("Invalid property name {0}", propertyName));
            }
            if (value != null && !property.PropertyType.IsInstanceOfType(value))
            {
                var convertToType = GetConvertToType(property.PropertyType);
                if (value is string)
                {
                    value = TypeDescriptor.GetConverter(convertToType).ConvertFromInvariantString(value.ToString());
                }
                else
                {
                    value = Convert.ChangeType(value, convertToType);
                }
            }
            property.SetValue(this, value);
        }
    }
}