namespace ProCenter.Service.Message.Organization
{
    #region Using Statements

    using System;
    using Agatha.Common;

    #endregion

    /// <summary>
    ///     Request to remove staff from team.
    /// </summary>
    public class RemoveStaffFromTeamRequest : Request
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the staff key.
        /// </summary>
        /// <value>
        ///     The staff key.
        /// </value>
        public Guid StaffKey { get; set; }

        /// <summary>
        ///     Gets or sets the team key.
        /// </summary>
        /// <value>
        ///     The team key.
        /// </value>
        public Guid TeamKey { get; set; }

        #endregion
    }
}