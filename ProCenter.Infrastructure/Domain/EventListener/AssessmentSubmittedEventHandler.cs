namespace ProCenter.Infrastructure.Domain.EventListener
{
    #region Using Statements

    using EventStore;
    using Pillar.Common.InversionOfControl;
    using Pillar.Domain.Event;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Event;
    using ProCenter.Domain.CommonModule;

    #endregion

    public class AssessmentSubmittedEventHandler : IDomainEventHandler<AssessmentSubmittedEvent>
    {
        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;

        public AssessmentSubmittedEventHandler(IAssessmentInstanceRepository assessmentInstanceRepository)
        {
            _assessmentInstanceRepository = assessmentInstanceRepository;
        }

        public void Handle(AssessmentSubmittedEvent args)
        {
            if (args.Submit)
            {
                var assessmentInstance = _assessmentInstanceRepository.GetByKey(args.Key);
                var scoringEngine = IoC.CurrentContainer.Resolve<IScoringEngine>(assessmentInstance.AssessmentName);
                scoringEngine.CalculateScore(assessmentInstance);
            }
        }
    }
}