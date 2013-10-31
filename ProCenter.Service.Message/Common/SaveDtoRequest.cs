namespace ProCenter.Service.Message.Common
{
    #region Using Statements

    using System;
    using Pillar.Agatha.Message;

    #endregion

    public class SaveDtoRequest<TDto> : SaveDtoRequest<TDto, Guid>, IHaveDataTransferObject
        where TDto : KeyedDataTransferObject
    {
        public SaveDtoRequest()
        {
        }

        public SaveDtoRequest(TDto dto)
        {
            DataTransferObject = dto;
        }

        public object Dto
        {
            get { return DataTransferObject; }
        }

        public KeyedDataTransferObject GetDto ()
        {
            return DataTransferObject;
        }
    }
}