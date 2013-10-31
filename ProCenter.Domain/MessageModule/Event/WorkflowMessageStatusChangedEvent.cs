namespace ProCenter.Domain.MessageModule.Event
{
    #region Using Statements

    using System;

    #endregion

    public class WorkflowMessageStatusChangedEvent : MessageEventBase
    {
        public WorkflowMessageStatusChangedEvent(Guid key, MessageType messageType, WorkflowMessageStatus status)
            : base(key, messageType)
        {
            Status = status;
        }

        public WorkflowMessageStatus Status { get; private set; }
    }
}