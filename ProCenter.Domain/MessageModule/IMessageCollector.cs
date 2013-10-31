namespace ProCenter.Domain.MessageModule
{
    #region Using Statements

    using System.Collections.Generic;

    #endregion

    public interface IMessageCollector
    {
        IEnumerable<IMessage> Messages { get; }
        void AddMessage(IMessage message);
    }
}