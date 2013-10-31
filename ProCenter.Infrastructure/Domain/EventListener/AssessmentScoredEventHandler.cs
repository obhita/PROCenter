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

    public class AssessmentScoredEventHandler : IDomainEventHandler<AssessmentScoredEvent>
    {
        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;

        public AssessmentScoredEventHandler(IAssessmentInstanceRepository assessmentInstanceRepository)
        {
            _assessmentInstanceRepository = assessmentInstanceRepository;
        }

        public void Handle(AssessmentScoredEvent args)
        {
            var assessmentInstance = _assessmentInstanceRepository.GetByKey(args.Key);

            var workflowEngine = IoC.CurrentContainer.Resolve<IWorkflowEngine>(assessmentInstance.AssessmentName);
            workflowEngine.Run(assessmentInstance);
        }
    }
}