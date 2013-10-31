namespace ProCenter.Domain.MessageModule.Event
{
    using System;

    public class MessageForSelfAdministrationEvent : MessageEventBase
    {
        public MessageForSelfAdministrationEvent ( Guid key, MessageType messageType )
            : base ( key, messageType )
        {
        }
    }
}
