namespace ProCenter.Service.Message.Common
{
    public interface IHaveDataTransferObject : IDtoResponse
    {
        object Dto { get; }
    }
}