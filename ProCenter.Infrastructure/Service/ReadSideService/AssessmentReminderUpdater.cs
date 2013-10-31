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
namespace ProCenter.Infrastructure.Service.ReadSideService
{
    #region

    using System;
    using Dapper;
    using Pillar.Common.Utility;
    using Primitive;
    using ProCenter.Domain.MessageModule;
    using ProCenter.Domain.MessageModule.Event;
    using ProCenter.Domain.PatientModule;
    using ProCenter.Domain.PatientModule.Event;

    #endregion

    public class AssessmentReminderUpdater : IHandleMessages<AssessmentReminderCreatedEvent>,
                                             IHandleMessages<AssessmentReminderUpdatedEvent>,
                                             IHandleMessages<AssessmentReminderRevisedEvent>,
                                             IHandleMessages<PatientChangedEvent>,
                                             IHandleMessages<MessageForSelfAdministrationEvent>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AssessmentReminderUpdater(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Handle(AssessmentReminderCreatedEvent message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(@"INSERT INTO [MessageModule].[AssessmentReminder] ([AssessmentReminderKey], 
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
                                        [Status],
                                        [ReminderDays]) 
                                    SELECT @AssessmentReminderKey,
                                        @OrganizationKey, 
                                        @PatientKey, 
                                        FirstName, 
                                        LastName, 
                                        @CreatedByStaffKey, 
                                        @AssessmentDefinitionKey, 
                                        (SELECT AssessmentName FROM [AssessmentModule].[AssessmentDefinition] WHERE [AssessmentDefinitionKey] = @AssessmentDefinitionKey),
                                        (SELECT AssessmentCode FROM [AssessmentModule].[AssessmentDefinition] WHERE [AssessmentDefinitionKey] = @AssessmentDefinitionKey), 
                                        @Title, 
                                        @Start,
                                        @Status,
                                        @ReminderDays
                                    FROM [PatientModule].[Patient] 
                                    WHERE PatientKey = @PatientKey",
                                   new
                                       {
                                           AssessmentReminderKey = message.Key,
                                           message.OrganizationKey,
                                           message.PatientKey,
                                           message.CreatedByStaffKey,
                                           message.AssessmentDefinitionKey,
                                           message.Title,
                                           message.Start,
                                           Status = message.Status.ToString(),
                                           ReminderDays = 0.0
                                       });
            }
        }


        public void Handle(AssessmentReminderUpdatedEvent message)
        {
            if (message.Property == PropertyUtil.ExtractPropertyName<AssessmentReminder, Guid>(a => a.PatientKey))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(@"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [PatientKey] = @PatientKey, [PatientFirstName] = (SELECT FirstName FROM [PatientModule].[Patient] WHERE PatientKey = @PatientKey), 
                                        [PatientLastName] = (SELECT LastName FROM [PatientModule].[Patient] WHERE PatientKey = @PatientKey)
                                    WHERE [AssessmentReminderKey] = @AssessmentReminderKey",
                                       new {AssessmentReminderKey = message.Key, PatientKey = message.Value.ToString()});
                }
            }

            if (message.Property == PropertyUtil.ExtractPropertyName<AssessmentReminder, Guid>(a => a.CreatedByStaffKey))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(@"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [CreatedByStaffKey] = @CreatedByStaffKey 
                                    WHERE [AssessmentReminderKey] = @AssessmentReminderKey",
                                       new {AssessmentReminderKey = message.Key, CreatedByStaffKey = message.Value.ToString()});
                }
            }
            if (message.Property == PropertyUtil.ExtractPropertyName<AssessmentReminder, string>(a => a.Title))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(@"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [Title] = @Title 
                                    WHERE [AssessmentReminderKey] = @AssessmentReminderKey",
                                       new {AssessmentReminderKey = message.Key, Title = message.Value.ToString()});
                }
            }

            if (message.Property == PropertyUtil.ExtractPropertyName<AssessmentReminder, DateTime>(a => a.Start))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(@"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [Start] = @Start
                                    WHERE [AssessmentReminderKey] = @AssessmentReminderKey",
                                       new {AssessmentReminderKey = message.Key, Start = ((DateTime) (message.Value)).ToString()});
                }
            }

            if (message.Property == PropertyUtil.ExtractPropertyName<AssessmentReminder, string>(a => a.Description))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(@"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [Description] = @Description 
                                    WHERE [AssessmentReminderKey] = @AssessmentReminderKey",
                                       new {AssessmentReminderKey = message.Key, Description = message.Value.ToString()});
                }
            }

            if (message.Property == PropertyUtil.ExtractPropertyName<AssessmentReminder, AssessmentReminderStatus>(a => a.Status))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(@"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [Status] = @Status 
                                    WHERE [AssessmentReminderKey] = @AssessmentReminderKey",
                                       new {AssessmentReminderKey = message.Key, Status = ((AssessmentReminderStatus) (message.Value)).ToString()});
                }
            }

            if (message.Property == PropertyUtil.ExtractPropertyName<AssessmentReminder, Guid>(a => a.AssessmentDefinitionKey))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(@"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [AssessmentDefinitionKey] = @AssessmentDefinitionKey, 
                                        [AssessmentName] = (SELECT AssessmentName FROM [AssessmentModule].[AssessmentDefinition] WHERE [AssessmentDefinitionKey] = @AssessmentDefinitionKey),
                                        [AssessmentCode] = (SELECT AssessmentCode FROM [AssessmentModule].[AssessmentDefinition] WHERE [AssessmentDefinitionKey] = @AssessmentDefinitionKey) 
                                    WHERE [AssessmentReminderKey] = @AssessmentReminderKey",
                                       new {AssessmentReminderKey = message.Key, AssessmentDefinitionKey = message.Value.ToString()});
                }
            }

            if (message.Property == PropertyUtil.ExtractPropertyName<AssessmentReminder, DateTime?>(a => a.AlertSentDate))
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    var alertSentDate = (DateTime?) (message.Value);
                    connection.Execute(@"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [AlertSentDate] = @AlertSentDate 
                                    WHERE [AssessmentReminderKey] = @AssessmentReminderKey",
                                       new {AssessmentReminderKey = message.Key, AlertSentDate = alertSentDate.HasValue ? alertSentDate.Value.ToString() : "NULL"});
                }
            }
        }

        public void Handle(PatientChangedEvent message)
        {
            if (message.Property == PropertyUtil.ExtractPropertyName<Patient, PersonName>(p => p.Name))
            {
                var name = message.Value as PersonName;
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(@"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [PatientFirstName] = @PatientFirstName, 
                                        [PatientLastName] = @PatientLastName
                                    WHERE [PatientKey] = @PatientKey", new {PatientFirstName = name.FirstName, PatientLastName = name.LastName, PatientKey = message.Key});
                }
            }
        }

        public void Handle(MessageForSelfAdministrationEvent message)
        {
            if (message.MessageType == MessageType.AssessmentReminder)
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(@"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [ForSelfAdministration] = @ForSelfAdministration
                                    WHERE [AssessmentReminderKey] = @AssessmentReminderKey",
                                            new { AssessmentReminderKey = message.Key, ForSelfAdministration = true });
                }
            }
        }

        public void Handle ( AssessmentReminderRevisedEvent message )
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                if ( message.SendToEmail == null )
                {
                    connection.Execute (@"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [ReminderDays] = @ReminderDays
                                    WHERE [AssessmentReminderKey] = @AssessmentReminderKey",
                                         new
                                             {
                                                 AssessmentReminderKey = message.Key,
                                                 ReminderDays = message.Unit == AssessmentReminderUnit.Days ? message.Time : 7 * message.Time,
                                             } );
                }
                else
                {
                    connection.Execute(@"UPDATE [MessageModule].[AssessmentReminder] 
                                    SET [ReminderDays] = @ReminderDays, [SendToEmail] = @SendToEmail
                                    WHERE [AssessmentReminderKey] = @AssessmentReminderKey",
                                         new
                                             {
                                                 AssessmentReminderKey = message.Key,
                                                 ReminderDays = message.Unit == AssessmentReminderUnit.Days ? message.Time : 7 * message.Time,
                                                 SendToEmail = message.SendToEmail.Address
                                             } );
                }
            }
        }
    }
}