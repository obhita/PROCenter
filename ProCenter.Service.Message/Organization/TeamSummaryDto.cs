namespace ProCenter.Service.Message.Organization
{
    #region Using Statements

    using Common;

    #endregion

    /// <summary>
    ///     Data transfer object for team summary.
    /// </summary>
    public class TeamSummaryDto : KeyedDataTransferObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        #endregion
    }
}