namespace ProCenter.Service.Message.Common
{
    #region Using Statements

    using System;
    using Agatha.Common;

    #endregion

    public class AddDtoResponse<TDto> : Response
    {
        #region Public Properties

        public Guid AggregateKey { get; set; }
        public TDto DataTransferObject { get; set; }

        #endregion
    }
}