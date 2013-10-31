namespace ProCenter.Infrastructure.Domain
{
    using System.Collections.Generic;
    using ProCenter.Domain.MessageModule;

    public class MessageCollector : IMessageCollector
    {
        private readonly List<IMessage> _messages = new List<IMessage>();
        public IEnumerable<IMessage> Messages
        {
            get { return _messages; }
        }

        public void AddMessage(IMessage message)
        {
            _messages.Add(message);
        }
    }
}