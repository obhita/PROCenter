namespace ProCenter.Service.Handler.Message
{
    using Common;
    using Domain.MessageModule;
    using Service.Message.Common;
    using Service.Message.Message;
    using global::AutoMapper;

    public class CancelAssessmentReminderRequestHandler : ServiceRequestHandler<CancelAssessmentReminderRequest, DtoResponse<AssessmentReminderDto>>
    {
        private readonly IAssessmentReminderRepository _assessmentReminderRepository;

        public CancelAssessmentReminderRequestHandler(IAssessmentReminderRepository assessmentReminderRepository)
        {
            _assessmentReminderRepository = assessmentReminderRepository;
        }

        protected override void Handle(CancelAssessmentReminderRequest request, DtoResponse<AssessmentReminderDto> response)
        {
            var assessmentReminder = _assessmentReminderRepository.GetByKey(request.AssessmentReminderKey);
            if (assessmentReminder != null)
            {
                assessmentReminder.Cancel ();
                var dto = Mapper.Map<AssessmentReminder, AssessmentReminderDto>(assessmentReminder);
                response.DataTransferObject = dto;
            }
        }
    }
}