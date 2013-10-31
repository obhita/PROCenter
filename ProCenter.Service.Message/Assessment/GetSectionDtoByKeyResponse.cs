namespace ProCenter.Service.Message.Assessment
{
    #region Using Statements

    using System.Collections.Generic;
    using Common;
    using Message;

    #endregion

    public class GetSectionDtoByKeyResponse : GetDtoByKeyResponse<SectionDto>
    {
        public IEnumerable<IMessageDto> Messages { get; set; }
    }
}