namespace ProCenter.Service.Handler.Message
{
    #region Using Statements

    using Common;
    using Domain.CommonModule;
    using Domain.MessageModule;
    using Infrastructure.Domain;
    using Service.Message.Message;

    #endregion

    public class AdministerAssessmentWorkflowMessageRequestHandler :
        ServiceRequestHandler<AdministerAssessmentWorkflowMessageRequest, WorkflowMessageStatusChangedResponse>
    {
        private readonly IWorkflowMessageRepository _workflowMessageRepository;

        public AdministerAssessmentWorkflowMessageRequestHandler(IWorkflowMessageRepository workflowMessageRepository)
        {
            _workflowMessageRepository = workflowMessageRepository;
        }

        protected override void Handle(AdministerAssessmentWorkflowMessageRequest request,
                                       WorkflowMessageStatusChangedResponse response)
        {
            var workflowMessage = _workflowMessageRepository.GetByKey(request.WorkflowMessageKey);
            if (workflowMessage != null)
            {
                workflowMessage.AdministerAssessment();
            }
        }
    }
}