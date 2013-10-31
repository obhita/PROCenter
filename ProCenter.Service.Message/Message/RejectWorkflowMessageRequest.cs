namespace ProCenter.Service.Message.Message
{
    #region Using Statements

    using System;
    using Agatha.Common;

    #endregion

    public class RejectWorkflowMessageRequest : Request
    {
        public Guid WorkflowMessageKey { get; set; }
    }
}