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

namespace ProCenter.Service.Handler.Assessment
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::AutoMapper;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Lookups;
    using ProCenter.Domain.MessageModule;
    using ProCenter.Infrastructure.Service.Completeness;
    using ProCenter.Service.Handler.Common;
    using ProCenter.Service.Message.Assessment;
    using ProCenter.Service.Message.Common;
    using ProCenter.Service.Message.Message;

    #endregion

    /// <summary>The get assessment section summary dto by key request handler.</summary>
    public class GetAssessmentSectionSummaryDtoByKeyRequestHandler :
        ServiceRequestHandler<GetDtoByKeyRequest<AssessmentSectionSummaryDto>, DtoResponse<AssessmentSectionSummaryDto>>
    {
        #region Fields

        private readonly IAssessmentCompletenessManager _assessmentCompletenessManager;

        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;

        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;

        private readonly IMessageCollector _messageCollector;

        private readonly IWorkflowMessageRepository _workflowMessageRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="GetAssessmentSectionSummaryDtoByKeyRequestHandler" /> class.</summary>
        /// <param name="assessmentInstanceRepository">The assessment instance repository.</param>
        /// <param name="assessmentDefinitionRepository">The assessment definition repository.</param>
        /// <param name="workflowMessageRepository">The workflow message repository.</param>
        /// <param name="messageCollector">The message collector.</param>
        /// <param name="assessmentCompletenessManager">The assessment completeness manager.</param>
        public GetAssessmentSectionSummaryDtoByKeyRequestHandler (
            IAssessmentInstanceRepository assessmentInstanceRepository,
            IAssessmentDefinitionRepository assessmentDefinitionRepository,
            IWorkflowMessageRepository workflowMessageRepository,
            IMessageCollector messageCollector,
            IAssessmentCompletenessManager assessmentCompletenessManager )
        {
            _assessmentInstanceRepository = assessmentInstanceRepository;
            _assessmentDefinitionRepository = assessmentDefinitionRepository;
            _workflowMessageRepository = workflowMessageRepository;
            _messageCollector = messageCollector;
            _assessmentCompletenessManager = assessmentCompletenessManager;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( GetDtoByKeyRequest<AssessmentSectionSummaryDto> request, DtoResponse<AssessmentSectionSummaryDto> response )
        {
            var assessmentInstance = _assessmentInstanceRepository.GetByKey ( request.Key );
            var assessmentDefinition = _assessmentDefinitionRepository.GetByKey ( assessmentInstance.AssessmentDefinitionKey );

            var assessmentSectionSummaryDto = new AssessmentSectionSummaryDto ()
                                              {
                                                  Key = assessmentInstance.Key,
                                                  PatientKey = assessmentInstance.PatientKey,
                                                  AssessmentDefinitionKey = assessmentDefinition.Key,
                                                  AssessmentDefinitionCode = assessmentDefinition.CodedConcept.Code,
                                                  AssessmentName = assessmentDefinition.CodedConcept.Name,
                                                  IsSubmitted = assessmentInstance.IsSubmitted,
                                                  IsComplete = Math.Abs ( assessmentInstance.CalculateCompleteness ().PercentComplete - 1.0d ) <= double.Epsilon,
                                                  Score = assessmentInstance.Score == null ? null : Mapper.Map<Score, ScoreDto> ( assessmentInstance.Score ),
                                                  Sections = new List<SectionSummaryDto> (),
                                                  PercentComplete = assessmentInstance.CalculateCompleteness().PercentComplete * 100,
                                              };

            MapItems ( assessmentSectionSummaryDto, assessmentDefinition, assessmentInstance );

            response.DataTransferObject = assessmentSectionSummaryDto;
            var messages = new List<IMessage> ( _messageCollector.Messages );
            if ( assessmentInstance.WorkflowKey.HasValue )
            {
                var message = _workflowMessageRepository.GetByKey ( assessmentInstance.WorkflowKey.Value );
                if ( message.Status == WorkflowMessageStatus.WaitingForResponse )
                {
                    messages.Add ( message );
                }
            }
            response.DataTransferObject.Messages =
                Mapper.Map<IEnumerable<IMessage>, IEnumerable<IMessageDto>> ( messages );
        }

        private SectionSummaryDto CreateSection ( AssessmentInstance assessmentInstance, ItemDefinition sectionItemDefinition )
        {
            var sectionSummaryDto = new SectionSummaryDto
                                    {
                                        ItemDefinitionCode = sectionItemDefinition.CodedConcept.Code,
                                        PercentComplete =
                                            _assessmentCompletenessManager.CalculateCompleteness ( assessmentInstance, sectionItemDefinition ).PercentComplete
                                    };
            return sectionSummaryDto;
        }

        private void MapItems ( AssessmentSectionSummaryDto assessmentSectionSummaryDto, AssessmentDefinition assessmentDefinition, AssessmentInstance assessmentInstance )
        {
            foreach ( var section in assessmentDefinition.ItemDefinitions.Where ( i => i.ItemType == ItemType.Section ) )
            {
                var subSections = section.ItemDefinitions.Where ( i => i.ItemType == ItemType.Section );
                var sectionSummaryDto = CreateSection ( assessmentInstance, section );
                foreach ( var subSection in subSections )
                {
                    sectionSummaryDto.SubSections.Add ( CreateSection ( assessmentInstance, subSection ) );
                }
                assessmentSectionSummaryDto.Sections.Add ( sectionSummaryDto );
            }
        }

        #endregion
    }
}