namespace ProCenter.Service.Message.Common
{
    #region Using Statements

    using System;
    using Pillar.Common.DataTransferObject;

    #endregion

    public interface IKeyedDataTransferObject : IKeyedDataTransferObject<Guid>, IDataTransferObject
    {
    }
}