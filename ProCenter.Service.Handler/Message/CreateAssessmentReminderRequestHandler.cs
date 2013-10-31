namespace ProCenter.Service.Handler.Message
{
    #region

    using Common;
    using Domain.MessageModule;
    using Pillar.Domain.Primitives;
    using Service.Message.Common;
    using Service.Message.Message;
    using global::AutoMapper;

    #endregion

    public class CreateAssessmentReminderRequestHandler : ServiceRequestHandler<AddDtoRequest<AssessmentReminderDto>, DtoResponse<AssessmentReminderDto>>
    {
        private readonly IAssessmentReminderFactory _assessmentReminderFactory;

        public CreateAssessmentReminderRequestHandler(IAssessmentReminderFactory assessmentReminderFactory)
        {
            _assessmentReminderFactory = assessmentReminderFactory;
        }

        protected override void Handle(AddDtoRequest<AssessmentReminderDto> request, DtoResponse<AssessmentReminderDto> response)
        {
            var dto = request.DataTransferObject;
            var assessmentReminder = _assessmentReminderFactory.Create(dto.OrganizationKey.Value, dto.PatientKey.Value, dto.CreatedByStaffKey.Value, dto.AssessmentDefinitionKey.Value,
                                                                                     dto.Title, dto.Start, dto.Description);
            assessmentReminder.ReviseReminder ( dto.ReminderTime, dto.ReminderUnit, string.IsNullOrWhiteSpace ( dto.SendToEmail ) ? null : new Email ( dto.SendToEmail ) );
            if ( request.DataTransferObject.ForSelfAdministration )
            {
                assessmentReminder.AllowSelfAdministration ();
            }

            response.DataTransferObject = Mapper.Map<AssessmentReminder, AssessmentReminderDto>(assessmentReminder);
        }
    }
}