namespace ProCenter.Service.Handler.Message
{
    #region

    using System;
    using Common;
    using Domain.MessageModule;
    using Pillar.Domain.Primitives;
    using Service.Message.Common;
    using Service.Message.Message;
    using global::AutoMapper;

    #endregion

    public class UpdateAssessmentReminderRequestHandler : ServiceRequestHandler<UpdateAssessmentReminderRequest, DtoResponse<AssessmentReminderDto>>
    {
        private readonly IAssessmentReminderRepository _assessmentReminderRepository;

        public UpdateAssessmentReminderRequestHandler(IAssessmentReminderRepository assessmentReminderRepository)
        {
            _assessmentReminderRepository = assessmentReminderRepository;
        }

        protected override void Handle(UpdateAssessmentReminderRequest request, DtoResponse<AssessmentReminderDto> response)
        {
            if (request.AssessmentReminderKey == Guid.Empty)
            {
                var dto = request.AssessmentReminderDto;
                var assessmentReminder = _assessmentReminderRepository.GetByKey(dto.Key);
                if (assessmentReminder != null)
                {
                    if (dto.PatientKey.HasValue && assessmentReminder.PatientKey != dto.PatientKey.Value)
                    {
                        assessmentReminder.RevisePatientKey(dto.PatientKey.Value);
                    }
                    if (dto.CreatedByStaffKey.HasValue && assessmentReminder.CreatedByStaffKey != dto.CreatedByStaffKey.Value)
                    {
                        assessmentReminder.ReviseCreatedbyStaffKey(dto.CreatedByStaffKey.Value);
                    }
                    if (assessmentReminder.Title != dto.Title)
                    {
                        assessmentReminder.ReviseTitle(dto.Title);
                    }
                    if (assessmentReminder.Start.Date != dto.Start.Date)
                    {
                        assessmentReminder.ReviseStart(dto.Start);
                    }
                    if (dto.AssessmentDefinitionKey.HasValue && assessmentReminder.AssessmentDefinitionKey != dto.AssessmentDefinitionKey.Value)
                    {
                        assessmentReminder.ReviseAssessmentDefinitionKey(dto.AssessmentDefinitionKey.Value);
                    }
                    if ( dto.ReminderTime > 0 )
                    {
                        assessmentReminder.ReviseReminder ( dto.ReminderTime, dto.ReminderUnit, string.IsNullOrWhiteSpace ( dto.SendToEmail ) ? null : new Email ( dto.SendToEmail ) );
                    }
                    if ( dto.ForSelfAdministration )
                    {
                        assessmentReminder.AllowSelfAdministration ();
                    }
                    var dto2 = Mapper.Map<AssessmentReminder, AssessmentReminderDto>(assessmentReminder);
                    response.DataTransferObject = dto2;
                }
            }
            else
            {
                var assessmentReminder = _assessmentReminderRepository.GetByKey(request.AssessmentReminderKey);
                if (assessmentReminder != null)
                {
                    assessmentReminder.ReviseStart(assessmentReminder.Start.AddDays(request.DayDelta));
                    var dto2 = Mapper.Map<AssessmentReminder, AssessmentReminderDto>(assessmentReminder);
                    response.DataTransferObject = dto2;
                }
            }
        }
    }
}