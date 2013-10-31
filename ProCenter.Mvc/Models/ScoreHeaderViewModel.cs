namespace ProCenter.Mvc.Models
{
    #region Using Statements

    using System.Collections.Generic;
    using Service.Message.Assessment;
    using Service.Message.Message;

    #endregion

    /// <summary>
    ///     View model for score header parital view.
    /// </summary>
    public class ScoreHeaderViewModel
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the messages.
        /// </summary>
        /// <value>
        ///     The messages.
        /// </value>
        public IEnumerable<IMessageDto> Messages { get; set; }

        /// <summary>
        ///     Gets or sets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public ScoreDto Score { get; set; }

        #endregion
    }
}