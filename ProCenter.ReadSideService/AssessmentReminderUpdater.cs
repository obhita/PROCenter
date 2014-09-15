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

using System.Collections.Generic;
using ProCenter.Domain.AssessmentModule;

namespace ProCenter.ReadSideService
{
    #region Using Statements

    using System;
    using System.Data.SqlClient;

    using Dapper;

    using Pillar.Common.Utility;

    using ProCenter.Common;
    using ProCenter.Domain.MessageModule;
    using ProCenter.Domain.MessageModule.Event;
    using ProCenter.Domain.PatientModule;
    using ProCenter.Domain.PatientModule.Event;
    using ProCenter.Primitive;

    #endregion

    /// <summary>The assessment message updater class.</summary>
    public class AssessmentReminderUpdater : IHandleMessages<AssessmentReminderCreatedEvent>,
        IHandleMessages<AssessmentReminderUpdatedEvent>,
        IHandleMessages<AssessmentReminderRevisedEvent>,
        IHandleMessages<PatientChangedEvent>,
        IHandleMessages<MessageForSelfAdministrationEvent>,
        IHandleMessages<MessageNotForSelfAdministrationEvent>,
        IHandleMessages<AssessmentRecurrenceRevisedEvent>,
        IHandleMessages<AssessmentReminderStatusChangedEvent>,
        IHandleMessages<AdministerAssessmentNowEvent>
    {
        #region Fields

        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IPatientRepository _patientRepository;
        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;
        private readonly IAssessmentReminderRepository _assessmentReminderRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentReminderUpdater" /> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="patientRepository">The patient repository.</param>
        /// <param name="assessmentDefinitionRepository">The assessment definition repository.</param>
        /// <param name="assessmentReminderRepository">The assessment message repository.</param>
        public AssessmentReminderUpdater ( 
            IDbConnectionFactory connectionFactory,
            IPatientRepository patientRepository,
            IAssessmentDefinitionRepository assessmentDefinitionRepository,
            IAssessmentReminderRepository assessmentReminderRepository)
        {
            _connectionFactory = connectionFactory;
            _patientRepository = patientRepository;
            _assessmentDefinitionRepository = assessmentDefinitionRepository;
            _assessmentReminderRepository = assessmentReminderRepository;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( AssessmentReminderCreatedEvent message )
        {
            CreateAssessmentReminders(message.Key);
        }

        private void CreateAssessmentReminders(Guid key)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var assessmentReminder = _assessmentReminderRepository.GetByKey(key);
                var patient = _patientRepository.GetByKey(assessmentReminder.PatientKey);
                var assessmentDefinition = _assessmentDefinitionRepository.GetByKey(assessmentReminder.AssessmentDefinitionKey);
                Func<DateTime, DateTime> getStartDate = (date) =>
                    {
                        switch (assessmentReminder.ReminderRecurrence)
                        {
                            case AssessmentReminderRecurrence.Weekly:
                                return date.AddDays(7);
                            case AssessmentReminderRecurrence.Monthly:
                                return date.AddMonths(1);
                            default:
                                return date.AddDays(1);
                        }
                    };
                
                var parameterList = new List<object>();
                if ( assessmentReminder.End == null )
                {
                    assessmentReminder.End = assessmentReminder.Start;
                }
                if ( assessmentReminder.Start > assessmentReminder.End )
                {
                    // add 1 minute to the end date to make it go at least once
                    assessmentReminder.End = assessmentReminder.Start.AddMinutes(1);
                }
                for (DateTime startDate = assessmentReminder.Start; startDate <= assessmentReminder.End; startDate = getStartDate(startDate))
                {
                    parameterList.Add(
                        new
                            {
                                AssessmentReminderKey = Guid.NewGuid(),
                                assessmentReminder.OrganizationKey,
                                assessmentReminder.PatientKey,
                                assessmentReminder.CreatedByStaffKey,
                                assessmentReminder.AssessmentDefinitionKey,
                                assessmentReminder.Title,
                                Start = startDate,
                                End = startDate.AddDays(1),
                                Status = assessmentReminder.Status.ToString(),
                                ReminderDays = 0.0,
                                Recurrence = assessmentReminder.ReminderRecurrence.ToString(),
                                PatientFirstName = patient.Name.FirstName,
                                PatientLastName = patient.Name.LastName,
                                AssessmentCode = assessmentDefinition.CodedConcept.Code,
                                AssessmentName = assessmentDefinition.CodedConcept.Name,
                                RecurrenceKey = assessmentReminder.Key,
                                SystemAccountKey = UserContext.Current.SystemAccountKey ?? assessmentReminder.CreatedByStaffKey
                            });
                }

                // todo: add assessmentinstancekey to the query
                connection.Execute("DELETE FROM [MessageModule].[AssessmentReminder] WHERE [RecurrenceKey] = @RecurrenceKey;", new { RecurrenceKey = key });
                connection.Execute(
                    @"INSERT INTO [MessageModule].[AssessmentReminder] ([AssessmentReminderKey], 
                                        [OrganizationKey], 
                                        [PatientKey] , 
                                        [PatientFirstname], 
                                        [PatientLastname], 
                                        [CreatedByStaffKey], 
                                        [AssessmentDefinitionKey], 
                                        [AssessmentName], 
                                        [AssessmentCode], 
                                        [Title], 
                                        [Start],
                                        [End],
                                        [Status],
                                        [ReminderDays],
                                        [Recurrence],
                                        [RecurrenceKey],
                                        [SystemAccountKey]
                                        ) 
                                    VALUES ( @AssessmentReminderKey,
                                        @OrganizationKey, 
                                        @PatientKey, 
                                        @PatientFirstName, 
                                        @PatientLastName, 
                                        @CreatedByStaffKey, 
                                        @AssessmentDefinitionKey, 
                                        @AssessmentName,
                                        @AssessmentCode, 
                                        @Title, 
                                        @Start,
                                        @End,
                                        @Status,
                                        @ReminderDays,
                                        @Recurrence,
                                        @RecurrenceKey,
                                        @SystemAccountKey)",
                    parameterList);
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( AssessmentReminderUpdatedEvent message )
        {
            if (message.Property == PropertyUtil.ExtractPropertyName<AssessmentReminder, Guid>(a => a.PatientKey))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(
                                        @"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [PatientKey] = @PatientKey, [PatientFirstName] = (SELECT FirstName FROM [PatientModule].[Patient] WHERE PatientKey = @PatientKey), 
                                        [PatientLastName] = (SELECT LastName FROM [PatientModule].[Patient] WHERE PatientKey = @PatientKey)
                                    WHERE [RecurrenceKey] = @AssessmentReminderKey",
                        new { AssessmentReminderKey = message.Key, PatientKey = message.Value.ToString() });
                }
            }

            if (message.Property == PropertyUtil.ExtractPropertyName<AssessmentReminder, Guid>(a => a.CreatedByStaffKey))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(
                                        @"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [CreatedByStaffKey] = @CreatedByStaffKey 
                                    WHERE [RecurrenceKey] = @AssessmentReminderKey",
                        new { AssessmentReminderKey = message.Key, CreatedByStaffKey = message.Value.ToString() });
                }
            }
            if (message.Property == PropertyUtil.ExtractPropertyName<AssessmentReminder, string>(a => a.Title))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(
                                        @"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [Title] = @Title 
                                    WHERE [RecurrenceKey] = @AssessmentReminderKey",
                        new { AssessmentReminderKey = message.Key, Title = message.Value.ToString() });
                }
            }

            if (message.Property == PropertyUtil.ExtractPropertyName<AssessmentReminder, DateTime>(a => a.Start))
            {
                CreateAssessmentReminders(message.Key);
            }

            if (message.Property == PropertyUtil.ExtractPropertyName<AssessmentReminder, string>(a => a.Description))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(
                                        @"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [Description] = @Description 
                                    WHERE [RecurrenceKey] = @AssessmentReminderKey",
                        new { AssessmentReminderKey = message.Key, Description = message.Value.ToString() });
                }
            }
            
            if (message.Property == PropertyUtil.ExtractPropertyName<AssessmentReminder, Guid>(a => a.AssessmentDefinitionKey))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                var assessmentDefinition = _assessmentDefinitionRepository.GetByKey(Guid.Parse(message.Value.ToString ()));
                    if ( assessmentDefinition != null )
                    {
                        connection.Execute(
                                @"UPDATE [MessageModule].[AssessmentReminder] 
                                SET [AssessmentDefinitionKey] = @AssessmentDefinitionKey, 
                                    [AssessmentName] = @AssessmentName,
                                    [AssessmentCode] = @AssessmentCode 
                                WHERE [RecurrenceKey] = @AssessmentReminderKey",
                            new
                            {
                                AssessmentReminderKey = message.Key,
                                AssessmentDefinitionKey = message.Value.ToString(),
                                AssessmentName = assessmentDefinition.CodedConcept.Name,
                                AssessmentCode = assessmentDefinition.CodedConcept.Code
                            });
                    }
                }
            }

            // todo: need to check whether the records should be updated based on the recurrencekey
            if ( message.Property == PropertyUtil.ExtractPropertyName<AssessmentReminder, DateTime?> ( a => a.AlertSentDate ) )
            {
                using ( var connection = _connectionFactory.CreateConnection () )
                {
                    var alertSentDate = (DateTime?)( message.Value );
                    connection.Execute (
                                        @"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [AlertSentDate] = @AlertSentDate 
                                    WHERE [AssessmentReminderKey] = @AssessmentReminderKey",
                        new { AssessmentReminderKey = message.Key, AlertSentDate = alertSentDate.HasValue ? alertSentDate.Value.ToString () : "NULL" } );
                }
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( PatientChangedEvent message )
        {
            if ( message.Property == PropertyUtil.ExtractPropertyName<Patient, PersonName> ( p => p.Name ) )
            {
                var name = message.Value as PersonName;
                using ( var connection = _connectionFactory.CreateConnection () )
                {
                    connection.Execute (
                                        @"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [PatientFirstName] = @PatientFirstName, 
                                        [PatientLastName] = @PatientLastName
                                    WHERE [PatientKey] = @PatientKey",
                        new { PatientFirstName = name.FirstName, PatientLastName = name.LastName, PatientKey = message.Key } );
                }
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( MessageForSelfAdministrationEvent message )
        {
            if ( message.MessageType == MessageType.AssessmentReminder )
            {
                using ( var connection = _connectionFactory.CreateConnection () )
                {
                    connection.Execute (
                                        @"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [ForSelfAdministration] = @ForSelfAdministration
                                    WHERE [RecurrenceKey] = @AssessmentReminderKey",
                        new { AssessmentReminderKey = message.Key, ForSelfAdministration = true } );
                }
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( AssessmentReminderRevisedEvent message )
        {
            // todo: need to check whether this should be updated based on the recurrencekey
            using ( var connection = _connectionFactory.CreateConnection () )
            {
                if ( message.SendToEmail == null )
                {
                    connection.Execute (
                                        @"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [ReminderDays] = @ReminderDays
                                    WHERE [RecurrenceKey] = @AssessmentReminderKey",
                        new
                        {
                            AssessmentReminderKey = message.Key,
                            ReminderDays = message.Unit == AssessmentReminderUnit.Days ? message.Time : 7 * message.Time,
                        } );
                }
                else
                {
                    connection.Execute (
                                        @"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [ReminderDays] = @ReminderDays, [SendToEmail] = @SendToEmail
                                    WHERE [RecurrenceKey] = @AssessmentReminderKey",
                        new
                        {
                            AssessmentReminderKey = message.Key,
                            ReminderDays = message.Unit == AssessmentReminderUnit.Days ? message.Time : 7 * message.Time,
                            SendToEmail = message.SendToEmail.Address
                        } );
                }
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( AssessmentRecurrenceRevisedEvent message )
        {
            if ( message.Recurrence != AssessmentReminderRecurrence.OneTime && ( message.End == null || message.End == DateTime.MinValue ) )
            {
                return;
            }

            CreateAssessmentReminders(message.Key);
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(AdministerAssessmentNowEvent message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                                @"UPDATE [MessageModule].[AssessmentReminder] 
                                SET [AssessmentInstanceKey] = @AssessmentInstanceKey
                                WHERE [AssessmentReminderKey] = @AssessmentReminderKey
                                AND [RecurrenceKey] = @RecurrenceKey",
                    new
                    {
                        message.AssessmentReminderKey,
                        message.AssessmentInstanceKey,
                        RecurrenceKey = message.Key
                    });
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(AssessmentReminderStatusChangedEvent message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                                @"UPDATE [MessageModule].[AssessmentReminder] 
                                SET [Status] = @Status
                                WHERE [AssessmentReminderKey] = @AssessmentReminderKey
                                AND [RecurrenceKey] = @RecurrenceKey",
                    new
                    {
                        message.AssessmentReminderKey,
                        Status = message.Status.ToString(),
                        RecurrenceKey = message.Key
                    });
            }
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle ( MessageNotForSelfAdministrationEvent message )
        {
            if ( message.MessageType == MessageType.AssessmentReminder )
            {
                using ( var connection = _connectionFactory.CreateConnection () )
                {
                    connection.Execute (
                                        @"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [ForSelfAdministration] = @ForSelfAdministration
                                    WHERE [RecurrenceKey] = @AssessmentReminderKey",
                        new { AssessmentReminderKey = message.Key, ForSelfAdministration = false } );
                }
            }
        }

        #endregion
    }
}