namespace ProCenter.Service.Handler.Message
{
    #region Using Statements

    using Common;
    using Domain.CommonModule;
    using Domain.MessageModule;
    using Infrastructure.Domain;
    using Service.Message.Message;

    #endregion

    public class RejectWorkflowMessageRequestHandler :
        ServiceRequestHandler<RejectWorkflowMessageRequest, WorkflowMessageStatusChangedResponse>
    {
        private readonly IWorkflowMessageRepository _workflowMessageRepository;

        public RejectWorkflowMessageRequestHandler(IWorkflowMessageRepository workflowMessageRepository)
        {
            _workflowMessageRepository = workflowMessageRepository;
        }

        protected override void Handle(RejectWorkflowMessageRequest request,
                                       WorkflowMessageStatusChangedResponse response)
        {
            var workflowMessage = _workflowMessageRepository.GetByKey(request.WorkflowMessageKey);
            if (workflowMessage != null)
            {
                workflowMessage.Reject();
            }
        }
    }
}