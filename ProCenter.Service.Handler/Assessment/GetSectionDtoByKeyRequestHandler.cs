namespace ProCenter.Service.Handler.Assessment
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.AssessmentModule;
    using Domain.AssessmentModule.Lookups;
    using Domain.CommonModule;
    using Domain.CommonModule.Lookups;
    using Domain.MessageModule;
    using Infrastructure.Domain;
    using Infrastructure.Service.Completeness;
    using Service.Message.Assessment;
    using Service.Message.Common.Lookups;
    using Service.Message.Message;
    using Service.Message.Metadata;
    using global::AutoMapper;

    #endregion

    /// <summary>
    /// Gets a section dto.
    /// </summary>
    public class GetSectionDtoByKeyRequestHandler :
        ServiceRequestHandler<GetSectionDtoByKeyRequest, GetSectionDtoByKeyResponse>
    {
        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;
        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;
        private readonly IWorkflowMessageRepository _workflowMessageRepository;
        private readonly IMessageCollector _messageCollector;

        public GetSectionDtoByKeyRequestHandler(IAssessmentDefinitionRepository assessmentDefinitionRepository,
                                                IAssessmentInstanceRepository assessmentInstanceRepository,
                                                IWorkflowMessageRepository workflowMessageRepository,
                                                IMessageCollector messageCollector)
        {
            _assessmentDefinitionRepository = assessmentDefinitionRepository;
            _assessmentInstanceRepository = assessmentInstanceRepository;
            _workflowMessageRepository = workflowMessageRepository;
            _messageCollector = messageCollector;
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle(GetSectionDtoByKeyRequest request, GetSectionDtoByKeyResponse response)
        {
            var assessmentInstance = _assessmentInstanceRepository.GetByKey(request.Key);
            var assessmentDefinition = _assessmentDefinitionRepository.GetByKey(assessmentInstance.AssessmentDefinitionKey);

            var sectionDto = new SectionDto
                {
                    Key = assessmentInstance.Key,
                    AssessmentKey = assessmentInstance.Key,
                    PatientKey = assessmentInstance.PatientKey,
                    AssessmentDefinitionKey = assessmentDefinition.Key,
                    AssessmentDefinitionCode = assessmentDefinition.CodedConcept.Code,
                    AssessmentName = assessmentDefinition.CodedConcept.Name,
                    IsSubmitted = assessmentInstance.IsSubmitted,
                    IsComplete = Math.Abs(assessmentInstance.PercentComplete - 1.0d) <= double.Epsilon,
                    Score = assessmentInstance.Score == null ? null : Mapper.Map<Score, ScoreDto>(assessmentInstance.Score),
                    Items = new List<ItemDto>()
                };

            MapItems(sectionDto, assessmentDefinition, assessmentInstance);

            response.DataTransferObject = sectionDto;
            var messages = new List<IMessage>(_messageCollector.Messages);
            if (assessmentInstance.WorkflowKey.HasValue)
            {
                var message = _workflowMessageRepository.GetByKey(assessmentInstance.WorkflowKey.Value);
                if (message.Status == WorkflowMessageStatus.WaitingForResponse)
                {
                    messages.Add(message);
                }
            }
            response.Messages =
                Mapper.Map<IEnumerable<IMessage>, IEnumerable<IMessageDto>>(messages);
        }

        private static void MapItems(IContainItems itemContainer, IContainItemDefinitions itemDefinitionContainer, AssessmentInstance assessmentInstance)
        {
            foreach (var itemDefinition in itemDefinitionContainer.ItemDefinitions)
            {
                if (itemDefinition.ItemType == ItemType.Question)
                {
                    itemContainer.Items.Add(CreateQuestion(itemDefinition, assessmentInstance));
                }
                else if (itemDefinition.ItemType == ItemType.Group)
                {
                    var groupItemDto = new ItemDto
                        {
                            Metadata = itemDefinition.ItemMetadata,
                            Key = assessmentInstance.Key,
                            ItemDefinitionCode = itemDefinition.CodedConcept.Code,
                            ItemDefinitionName = itemDefinition.CodedConcept.Name,
                            Items = new List<ItemDto>(),
                            ItemType = itemDefinition.ItemType.CodedConcept.Code
                        };

                    MapItems(groupItemDto, itemDefinition, assessmentInstance);

                    itemContainer.Items.Add(groupItemDto);
                }
            }
        }

        private static ItemDto CreateQuestion(ItemDefinition itemDefinition, AssessmentInstance assessmentInstance)
        {
            var itemInstance =
                assessmentInstance.ItemInstances.FirstOrDefault(
                    i => i.ItemDefinitionCode == itemDefinition.CodedConcept.Code);
            return new ItemDto
                {
                    Metadata = itemDefinition.ItemMetadata,
                    Key = assessmentInstance.Key,
                    ItemDefinitionCode = itemDefinition.CodedConcept.Code,
                    ItemDefinitionName = itemDefinition.CodedConcept.Name,
                    Options = Mapper.Map<IEnumerable<Lookup>, IEnumerable<LookupDto>>(itemDefinition.Options),
                    Value = itemInstance == null ? null : itemInstance.Value is Lookup ? Mapper.Map<Lookup, LookupDto>(itemInstance.Value as Lookup) : itemInstance.Value,
                    ItemType = itemDefinition.ItemType.CodedConcept.Code
                };
        }
    }
}