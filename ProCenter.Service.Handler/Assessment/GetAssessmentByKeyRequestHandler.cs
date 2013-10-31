namespace ProCenter.Service.Handler.Assessment
{
    using Common;
    using global::AutoMapper;

    public class GetAssessmentByKeyRequestHandler : ServiceRequestHandler<GetAssessmentByKeyRequest, GetAssessmentResponse>
    {
        private readonly IAssessmentRepository _assessmentRepository;

        public GetAssessmentByKeyRequestHandler(IAssessmentRepository assessmentRepository)
        {
            _assessmentRepository = assessmentRepository;
        }

        protected override void Handle(GetAssessmentByKeyRequest request, GetAssessmentResponse response)
        {
            var assessment = _assessmentRepository.GetByKey ( request.AssessmentKey );
            if (assessment != null)
            {
                response.DataTransferObject = Mapper.Map<Domain.AssessmentModule.Assessment, AssessmentDto>(assessment);
            }
        }
    }
}
