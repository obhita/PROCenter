namespace ProCenter.Service.Message.Assessment
{
    #region Using Statements

    using System.Collections.Generic;
    using Agatha.Common;
    using Message;

    #endregion

    public class SubmitAssessmentResponse : Response
    {
        public ScoreDto ScoreDto { get; set; }
        public IEnumerable<IMessageDto> Messages { get; set; }
    }
}