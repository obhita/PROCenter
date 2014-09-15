#region License Header
// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/
#endregion
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

    /// <summary>
    /// UpdateAssessmentReminderRequestHandler class.
    /// </summary>
    public class UpdateAssessmentReminderRequestHandler : ServiceRequestHandler<UpdateAssessmentReminderRequest, DtoResponse<AssessmentReminderDto>>
    {
        /// <summary>
        /// The _assessment reminder repository.
        /// </summary>
        private readonly IAssessmentReminderRepository _assessmentReminderRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAssessmentReminderRequestHandler"/> class.
        /// </summary>
        /// <param name="assessmentReminderRepository">The assessment reminder repository.</param>
        public UpdateAssessmentReminderRequestHandler(IAssessmentReminderRepository assessmentReminderRepository)
        {
            _assessmentReminderRepository = assessmentReminderRepository;
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
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
                    if ( dto.AssessmentInstanceKey.HasValue )
                    {
                        assessmentReminder.ReviseAssessmentInstanceKey ( dto.AssessmentInstanceKey.GetValueOrDefault() );
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
                    if (dto.ReminderTime > 0)
                    {
                        assessmentReminder.ReviseReminder(
                            dto.ReminderTime,
                            dto.ReminderUnit,
                            string.IsNullOrWhiteSpace(dto.SendToEmail) ? null : new Email(dto.SendToEmail));
                    }
                    if (dto.ForSelfAdministration)
                    {
                        assessmentReminder.AllowSelfAdministration();
                    }
                    else
                    {
                        assessmentReminder.DontAllowSelfAdministration();
                    }
                    if (dto.ReminderRecurrence != assessmentReminder.ReminderRecurrence || dto.End != assessmentReminder.End)
                    {
                        assessmentReminder.ReviseRecurrence(dto.ReminderRecurrence, dto.End);
                    }
                    var dto2 = Mapper.Map<AssessmentReminder, AssessmentReminderDto>(assessmentReminder);
                    response.DataTransferObject = dto2;
                }
            }
            else
            {
                var dto = request.AssessmentReminderDto;
                var assessmentReminder = _assessmentReminderRepository.GetByKey(dto.RecurrenceKey.Value);
                if (assessmentReminder != null)
                {
                    // todo: need to check whether the startdate should be updated
                    //assessmentReminder.ReviseStart(assessmentReminder.Start.AddDays(request.DayDelta));
                    var dto2 = Mapper.Map<AssessmentReminder, AssessmentReminderDto>(assessmentReminder);
                    response.DataTransferObject = dto2;

                    if (dto.AssessmentInstanceKey.HasValue)
                    {
                        assessmentReminder.ReviseAssessmentInstanceKey(dto.AssessmentInstanceKey.GetValueOrDefault(), request.AssessmentReminderKey);
                    }
                }
            }
        }
    }
}