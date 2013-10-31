using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCenter.Service.Handler.Message
{
    using Common;
    using Domain.MessageModule;
    using Service.Message.Common;
    using Service.Message.Message;
    using global::AutoMapper;

    public class AcknowledgeAssessmentReminderRequestHandler: ServiceRequestHandler<AcknowledgeAssessmentReminderRequest, DtoResponse<AssessmentReminderDto>>
    {
        private readonly IAssessmentReminderRepository _assessmentReminderRepository;

        public AcknowledgeAssessmentReminderRequestHandler(IAssessmentReminderRepository assessmentReminderRepository)
        {
            _assessmentReminderRepository = assessmentReminderRepository;
        }

        protected override void Handle(AcknowledgeAssessmentReminderRequest request, DtoResponse<AssessmentReminderDto> response)
        {
             var assessmentReminder = _assessmentReminderRepository.GetByKey(request.Key);
            if (assessmentReminder != null)
            {
                assessmentReminder.Acknowledge();
                var dto2 = Mapper.Map<AssessmentReminder, AssessmentReminderDto>(assessmentReminder);
                response.DataTransferObject = dto2;
            }
        }
    }
}