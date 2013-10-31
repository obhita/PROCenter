namespace ProCenter.Service.Message.Assessment
{
    #region Using Statements

    using Agatha.Common;

    #endregion

    /// <summary>
    /// Response to <see cref="SaveAssessmentItemRequest"/>.
    /// </summary>
    public class SaveAssessmentItemResponse : Response
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance can submit.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can submit; otherwise, <c>false</c>.
        /// </value>
        public bool CanSubmit { get; set; }

        #endregion
    }
}