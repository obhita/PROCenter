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