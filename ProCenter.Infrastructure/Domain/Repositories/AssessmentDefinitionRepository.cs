namespace ProCenter.Infrastructure.Domain.Repositories
{
    #region Using Statements

    using System;
    using System.Linq;
    using Dapper;
    using EventStore;
    using ProCenter.Domain.AssessmentModule;
    using Service.ReadSideService;

    #endregion

    public class AssessmentDefinitionRepository : RepositoryBase<AssessmentDefinition>, IAssessmentDefinitionRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AssessmentDefinitionRepository(IDbConnectionFactory connectionFactory, IEventStoreRepository eventStoreRepository)
            :base(eventStoreRepository)
        {
            _connectionFactory = connectionFactory;
        }

        public Guid GetKeyByCode(string assessmentDefinitionCode)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var key = connection.Query<Guid?>(
                    "Select AssessmentDefinitionKey from [AssessmentModule].[AssessmentDefinition] Where AssessmentCode = @code",
                    new {code = assessmentDefinitionCode}).SingleOrDefault();
                if (!key.HasValue)
                {
                    throw new ArgumentException(
                        string.Format("There is no assessment definition with the code {0}", assessmentDefinitionCode),
                        "assessmentDefinitionCode");
                }
                return key.Value;
            }
        }
    }
}