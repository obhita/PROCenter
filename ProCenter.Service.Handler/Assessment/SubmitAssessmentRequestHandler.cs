namespace ProCenter.Service.Handler.Assessment
{
    #region Using Statements

    using System.Collections.Generic;
    using Common;
    using Domain.AssessmentModule;
    using Domain.CommonModule;
    using Domain.MessageModule;
    using Infrastructure.Domain;
    using Service.Message.Assessment;
    using Service.Message.Message;
    using global::AutoMapper;

    #endregion

    public class SubmitAssessmentRequestHandler :
        ServiceRequestHandler<SubmitAssessmentRequest, SubmitAssessmentResponse>
    {
        private readonly IMessageCollector _messageCollector;
        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;

        public SubmitAssessmentRequestHandler(IAssessmentInstanceRepository assessmentInstanceRepository, IMessageCollector messageCollector)
        {
            _assessmentInstanceRepository = assessmentInstanceRepository;
            _messageCollector = messageCollector;
        }

        protected override void Handle(SubmitAssessmentRequest request, SubmitAssessmentResponse response)
        {
            var assessmentInstance = _assessmentInstanceRepository.GetByKey(request.AssessmentKey);

            if (request.Submit)
            {
                assessmentInstance.Submit();
            }
            else
            {
                assessmentInstance.Unsubmit();
            }
            response.ScoreDto = Mapper.Map<Score, ScoreDto> ( assessmentInstance.Score );
            response.Messages = Mapper.Map<IEnumerable<IMessage>, IEnumerable<IMessageDto>>(_messageCollector.Messages);
        }
    }
}