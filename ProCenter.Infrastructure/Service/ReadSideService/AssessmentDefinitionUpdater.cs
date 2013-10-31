namespace ProCenter.Infrastructure.Service.ReadSideService
{
    #region Using Statements

    using System.Data;
    using Dapper;
    using ProCenter.Domain.AssessmentModule.Event;

    #endregion

    public class AssessmentDefinitionUpdater : IHandleMessages<AssessmentDefinitionCreatedEvent>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AssessmentDefinitionUpdater(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        #region Public Methods and Operators

        public void Handle(AssessmentDefinitionCreatedEvent assessmentDefinitionCreatedEvent)
        {
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                connection.Execute(
                    "insert AssessmentModule.AssessmentDefinition values(@AssessmentDefinitionKey, @AssessmentName, @AssessmentCode)",
                    new
                        {
                            AssessmentDefinitionKey = assessmentDefinitionCreatedEvent.Key,
                            AssessmentName = assessmentDefinitionCreatedEvent.CodedConcept.Name,
                            AssessmentCode = assessmentDefinitionCreatedEvent.CodedConcept.Code,
                        });
            }
        }

        #endregion
    }
}