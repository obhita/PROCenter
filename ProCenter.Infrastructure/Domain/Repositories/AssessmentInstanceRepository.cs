namespace ProCenter.Infrastructure.Domain.Repositories
{
    #region Using Statements

    using EventStore;
    using ProCenter.Domain.AssessmentModule;

    #endregion

    public class AssessmentInstanceRepository : RepositoryBase<AssessmentInstance>, IAssessmentInstanceRepository
    {
        #region Constructors and Destructors

        public AssessmentInstanceRepository ( IEventStoreRepository eventStoreRepository )
            : base ( eventStoreRepository )
        {
        }

        #endregion
    }
}