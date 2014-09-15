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
    #region Using Statements

    using Common;
    using Domain.MessageModule;
    using global::AutoMapper;
    using Pillar.Domain.Primitives;
    using Service.Message.Common;
    using Service.Message.Message;

    #endregion

    /// <summary>The create assessment reminder request handler class.</summary>
    public class CreateAssessmentReminderRequestHandler : ServiceRequestHandler<AddDtoRequest<AssessmentReminderDto>, DtoResponse<AssessmentReminderDto>>
    {
        #region Fields

        private readonly IAssessmentReminderFactory _assessmentReminderFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAssessmentReminderRequestHandler"/> class.
        /// </summary>
        /// <param name="assessmentReminderFactory">The assessment reminder factory.</param>
        public CreateAssessmentReminderRequestHandler ( IAssessmentReminderFactory assessmentReminderFactory )
        {
            _assessmentReminderFactory = assessmentReminderFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( AddDtoRequest<AssessmentReminderDto> request, DtoResponse<AssessmentReminderDto> response )
        {
            var dto = request.DataTransferObject;
            var assessmentReminder = _assessmentReminderFactory.Create ( 
                dto.OrganizationKey.Value,
                dto.PatientKey.Value,
                dto.CreatedByStaffKey.Value,
                dto.AssessmentDefinitionKey.Value,
                dto.Title,
                dto.Start,
                dto.Description,
                dto.ReminderRecurrence,
                dto.End);
            assessmentReminder.ReviseReminder ( dto.ReminderTime, dto.ReminderUnit, string.IsNullOrWhiteSpace ( dto.SendToEmail ) ? null : new Email ( dto.SendToEmail ) );
            if ( request.DataTransferObject.ForSelfAdministration )
            {
                assessmentReminder.AllowSelfAdministration ();
            }

            response.DataTransferObject = Mapper.Map<AssessmentReminder, AssessmentReminderDto> ( assessmentReminder );
        }

        #endregion
    }
}