namespace ProCenter.Infrastructure.Service.ReadSideService
{
    #region

    using System.Data;
    using Dapper;
    using ProCenter.Domain.OrganizationModule.Event;

    #endregion

    public class OrganizationUpdater : IHandleMessages<AssessmentDefinitionAddedEvent>, 
        IHandleMessages<AssessmentDefinitionRemovedEvent>, 
        IHandleMessages<OrganizationCreatedEvent>,
        IHandleMessages<OrganizationNameRevisedEvent>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public OrganizationUpdater(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Handle(AssessmentDefinitionAddedEvent message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    @"INSERT INTO [OrganizationModule].[OrganizationAssessmentDefinition] ([OrganizationKey], [AssessmentDefinitionKey], [AssessmentName], [AssessmentCode]) 
                    SELECT @OrganizationKey, @AssessmentDefinitionKey, a.AssessmentName, a.AssessmentCode
                    FROM AssessmentModule.AssessmentDefinition a 
                    WHERE AssessmentDefinitionKey = @AssessmentDefinitionKey", new
                    {
                        OrganizationKey = message.Key,
                        message.AssessmentDefinitionKey
                    });
            }
        }

        public void Handle(AssessmentDefinitionRemovedEvent message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    "DELETE FROM OrganizationModule.OrganizationAssessmentDefinition WHERE OrganizationKey = @OrganizationKey AND AssessmentDefinitionKey = @AssessmentDefinitionKey",
                    new
                        {
                            OrganizationKey = message.Key,
                            message.AssessmentDefinitionKey
                        });
            }
        }

        public void Handle ( OrganizationCreatedEvent message )
        {
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    "INSERT INTO OrganizationModule.Organization(OrganizationKey, Name) VALUES(@OrganizationKey, @Name)",
                    new
                    {
                        OrganizationKey = message.Key,
                        message.Name
                    });
            }
        }

        public void Handle ( OrganizationNameRevisedEvent message )
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    "UPDATE OrganizationModule.Organization Set Name = @Name WHERE OrganizationKey = @OrganizationKey",
                    new
                    {
                        OrganizationKey = message.Key,
                        message.Name,
                    });
            }
        }
    }
}