namespace ProCenter.Service.Message.Common
{
    #region Using Statements

    using System;
    using Pillar.Agatha.Message;

    #endregion

    public class GetDtoByKeyRequest<TDto> : GetDtoByKeyRequest<TDto, Guid>
        where TDto : IDataTransferObject
    {
        public GetDtoByKeyRequest()
        {
        }

        public GetDtoByKeyRequest(Guid key)
        {
            Key = key;
        }
    }
}