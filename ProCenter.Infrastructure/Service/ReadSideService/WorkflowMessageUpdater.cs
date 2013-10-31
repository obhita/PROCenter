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
namespace ProCenter.Infrastructure.Service.ReadSideService
{
    #region

    using System;
    using Dapper;
    using ProCenter.Domain.MessageModule;
    using ProCenter.Domain.MessageModule.Event;

    #endregion

    public class WorkflowMessageUpdater : IHandleMessages<WorkflowMessageCreatedEvent>, IHandleMessages<WorkflowMessageAdvancedEvent>,
                                          IHandleMessages<WorkflowMessageStatusChangedEvent>, IHandleMessages<MessageForSelfAdministrationEvent>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public WorkflowMessageUpdater(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Handle(WorkflowMessageCreatedEvent message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    @"INSERT INTO [MessageModule].[WorkflowMessage] (WorkflowMessageKey, MessageType, PatientKey, WorkflowMessageStatus, InitiatingAssessmentDefinitionCode, InitiatingAssessmentDefinitionKey, 
                        RecommendedAssessmentDefinitionCode, RecommendedAssessmentDefinitionKey, RecommendedAssessmentDefinitionName, InitiatingAssessmentScore, CreatedDate, OrganizationKey) 
                    SELECT @WorkflowMessageKey, @MessageType,  @PatientKey, @WorkflowMessageStatus, @InitiatingAssessmentDefinitionCode, @InitiatingAssessmentDefinitionKey, 
                        @RecommendedAssessmentDefinitionCode, @RecommendedAssessmentDefinitionKey, AssessmentName, @InitiatingAssessmentScore, @CreatedDate, @OrganizationKey
                    FROM AssessmentModule.AssessmentDefinition 
                    WHERE AssessmentDefinitionKey = @RecommendedAssessmentDefinitionKey",
                    new
                        {
                            WorkflowMessageKey = message.Key,
                            MessageType = message.MessageType.ToString(),
                            message.PatientKey,
                            WorkflowMessageStatus = message.WorkflowMessageStatus.ToString(),
                            InitiatingAssessmentDefinitionCode = message.InitiatingAssessmentCode,
                            InitiatingAssessmentDefinitionKey = message.InitiatingAssessmentKey,
                            message.RecommendedAssessmentDefinitionCode,
                            message.RecommendedAssessmentDefinitionKey,
                            InitiatingAssessmentScore = message.InitiatingAssessmentScore == null ? (string) null : message.InitiatingAssessmentScore.Value.ToString(),
                            CreatedDate = DateTime.Now,
                            message.OrganizationKey
                        });
            }
        }

        public void Handle(WorkflowMessageAdvancedEvent message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    @"UPDATE [MessageModule].[WorkflowMessage] SET InitiatingAssessmentDefinitionCode = @InitiatingAssessmentDefinitionCode,
                        InitiatingAssessmentDefinitionKey = @InitiatingAssessmentDefinitionKey,
                        RecommendedAssessmentDefinitionCode = @RecommendedAssessmentDefinitionCode,
                        RecommendedAssessmentDefinitionKey = @RecommendedAssessmentDefinitionKey,
                        RecommendedAssessmentDefinitionName = a.AssessmentName,
                        InitiatingAssessmentScore = @InitiatingAssessmentScore
                    FROM [MessageModule].[WorkflowMessage] w 
                    JOIN [AssessmentModule].[AssessmentDefinition] a 
                        ON w.WorkflowMessageKey = @WorkflowMessageKey AND w.RecommendedAssessmentDefinitionKey = a.AssessmentDefinitionKey",
                    new
                        {
                            WorkflowMessageKey = message.Key,
                            InitiatingAssessmentDefinitionCode = message.InitiatingAssessmentCode,
                            InitiatingAssessmentDefinitionKey = message.InitiatingAssessmentKey,
                            message.RecommendedAssessmentDefinitionCode,
                            message.RecommendedAssessmentDefinitionKey,
                            InitiatingAssessmentScore = message.InitiatingAssessmentScore == null ? (string) null : message.InitiatingAssessmentScore.Value.ToString(),
                        });
            }
        }

        public void Handle(WorkflowMessageStatusChangedEvent message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    @"UPDATE [MessageModule].[WorkflowMessage] 
                    SET WorkflowMessageStatus = @WorkflowMessageStatus 
                    WHERE WorkflowMessageKey = @WorkflowMessageKey",
                    new
                        {
                            WorkflowMessageKey = message.Key,
                            WorkflowMessageStatus = message.Status.ToString(),
                        });
            }
        }

        public void Handle(MessageForSelfAdministrationEvent message)
        {
            if ( message.MessageType == MessageType.RecommendAssessment )
            {
                using ( var connection = _connectionFactory.CreateConnection () )
                {
                    connection.Execute (
                                        @"UPDATE [MessageModule].[WorkflowMessage] 
                                        SET ForSelfAdministration = @ForSelfAdministration 
                                        WHERE WorkflowMessageKey = @WorkflowMessageKey",
                                        new
                                            {
                                                WorkflowMessageKey = message.Key,
                                                ForSelfAdministration = true,
                                            } );
                }
            }
        }
    }
}