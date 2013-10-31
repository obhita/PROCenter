namespace ProCenter.Infrastructure.Domain.Repositories
{
    #region

    using EventStore;
    using ProCenter.Domain.MessageModule;

    #endregion

    public class AssessmentReminderRepository : RepositoryBase<AssessmentReminder>, IAssessmentReminderRepository
    {
        public AssessmentReminderRepository(IEventStoreRepository eventStoreRepository) : base(eventStoreRepository)
        {
        }
    }
}