namespace ProCenter.Domain.MessageModule
{
    public interface IMessage
    {
        MessageType MessageType { get; }
        bool ForSelfAdministration { get; }
    }
}