namespace ProCenter.Infrastructure.Service.ReadSideService
{
    #region Using Statements

    using System;
    using System.Data;
    using Dapper;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Event;

    #endregion

    public class AssessmentInstanceUpdater : IHandleMessages<AssessmentCreatedEvent>,
                                             IHandleMessages<ItemUpdatedEvent>,
                                             IHandleMessages<AssessmentSubmittedEvent>,
                                             IHandleMessages<PercentCompleteUpdatedEvent>,
                                             IHandleMessages<AssessmentCanBeSelfAdministeredEvent>
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;

        public AssessmentInstanceUpdater(IDbConnectionFactory connectionFactory, IAssessmentInstanceRepository assessmentInstanceRepository)
        {
            _connectionFactory = connectionFactory;
            _assessmentInstanceRepository = assessmentInstanceRepository;
        }

        public void Handle(AssessmentCreatedEvent message)
        {
            var lastModified = _assessmentInstanceRepository.GetLastModifiedDate ( message.Key );
            var createTime = DateTime.Now;
            const string cmd =
                @"INSERT INTO AssessmentModule.AssessmentInstance (AssessmentInstanceKey, AssessmentName, AssessmentCode, OrganizationKey, PatientKey, PercentComplete, CreatedTime, LastModifiedTime, IsSubmitted) 
                SELECT @AssessmentInstanceKey, AssessmentName, AssessmentCode, (SELECT OrganizationKey FROM PatientModule.Patient WHERE PatientKey = @PatientKey), 
                    @PatientKey, @PercentComplete, @CreatedTime, @LastModifiedTime, @IsSubmitted 
                FROM AssessmentModule.AssessmentDefinition WHERE AssessmentDefinitionKey =  @AssessmentDefinitionKey";
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(cmd, new
                    {
                        AssessmentInstanceKey = message.Key,
                        message.PatientKey,
                        PercentComplete = 0,
                        CreatedTime = createTime,
                        LastModifiedTime = lastModified ?? createTime,
                        IsSubmitted = false,
                        message.AssessmentDefinitionKey
                    });
            }
        }

        public void Handle(AssessmentSubmittedEvent message)
        {
            const string cmd =
                @"UPDATE AssessmentModule.AssessmentInstance SET IsSubmitted = @IsSubmitted
                where AssessmentInstanceKey = @AssessmentInstanceKey";
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(cmd, new
                    {
                        AssessmentInstanceKey = message.Key,
                        IsSubmitted = message.Submit,
                    });
            }
        }

        public void Handle(AssessmentCanBeSelfAdministeredEvent message)
        {
            const string cmd =
                @"UPDATE AssessmentModule.AssessmentInstance SET CanSelfAdminister = @CanSelfAdminister
                where AssessmentInstanceKey = @AssessmentInstanceKey";
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(cmd, new
                {
                    AssessmentInstanceKey = message.Key,
                    CanSelfAdminister = true
                });
            }
        }

        public void Handle(ItemUpdatedEvent message)
        {
            var lastModified = _assessmentInstanceRepository.GetLastModifiedDate(message.Key);
            const string cmd =
                @"UPDATE AssessmentModule.AssessmentInstance SET LastModifiedTime = @LastModifiedTime
                where AssessmentInstanceKey = @AssessmentInstanceKey";
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(cmd, new
                {
                    AssessmentInstanceKey = message.Key,
                    LastModifiedTime = lastModified,
                });
            }
        }

        public void Handle(PercentCompleteUpdatedEvent message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute("UPDATE AssessmentModule.AssessmentInstance SET PercentComplete = @PercentComplete WHERE AssessmentInstanceKey = @AssessmentInstanceKey",
                                   new {PercentComplete = message.PercentComplete, AssessmentInstanceKey = message.Key});
            }
        }
    }
}