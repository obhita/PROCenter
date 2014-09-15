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
    using Domain.AssessmentModule;
    using Domain.MessageModule;
    using Domain.PatientModule;
    using global::AutoMapper;
    using ProCenter.Common;
    using Service.Message.Common;
    using Service.Message.Message;

    #endregion

    /// <summary>The get assessment reminder by key request handler class.</summary>
    public class GetAssessmentReminderByKeyRequestHandler : ServiceRequestHandler<GetAssessmentReminderByKeyRequest, DtoResponse<AssessmentReminderDto>>
    {
        #region Fields

        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;
        private readonly IAssessmentReminderRepository _assessmentReminderRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IResourcesManager _resourcesManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAssessmentReminderByKeyRequestHandler"/> class.
        /// </summary>
        /// <param name="assessmentReminderRepository">The assessment reminder repository.</param>
        /// <param name="assessmentDefinitionRepository">The assessment definition repository.</param>
        /// <param name="patientRepository">The patient repository.</param>
        /// <param name="resourcesManager">The resources manager.</param>
        public GetAssessmentReminderByKeyRequestHandler (
            IAssessmentReminderRepository assessmentReminderRepository,
            IAssessmentDefinitionRepository assessmentDefinitionRepository,
            IPatientRepository patientRepository,
            IResourcesManager resourcesManager )
        {
            _assessmentReminderRepository = assessmentReminderRepository;
            _assessmentDefinitionRepository = assessmentDefinitionRepository;
            _patientRepository = patientRepository;
            _resourcesManager = resourcesManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( GetAssessmentReminderByKeyRequest request, DtoResponse<AssessmentReminderDto> response )
        {
            var assessmentReminder = _assessmentReminderRepository.GetByKey ( request.AssessmentReminderKey );
            if ( assessmentReminder != null )
            {
                var patient = _patientRepository.GetByKey ( assessmentReminder.PatientKey );
                var assessmentDefinition = _assessmentDefinitionRepository.GetByKey ( assessmentReminder.AssessmentDefinitionKey );
                var assessmentReminderDto = Mapper.Map<AssessmentReminder, AssessmentReminderDto> ( assessmentReminder );
                assessmentReminderDto.AssessmentName =
                    _resourcesManager.GetResourceManagerByName ( assessmentDefinition.CodedConcept.Name ).GetString ( "_" + assessmentDefinition.CodedConcept.Code );
                assessmentReminderDto.PatientFirstName = patient.Name.FirstName;
                assessmentReminderDto.PatientLastName = patient.Name.LastName;
                response.DataTransferObject = assessmentReminderDto;
            }
        }

        #endregion
    }
}