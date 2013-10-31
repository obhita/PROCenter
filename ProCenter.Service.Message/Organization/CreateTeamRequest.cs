namespace ProCenter.Service.Message.Organization
{
    #region Using Statements

    using System;
    using Agatha.Common;

    #endregion

    /// <summary>
    ///     Request to create team.
    /// </summary>
    public class CreateTeamRequest : Request
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the organization key.
        /// </summary>
        /// <value>
        ///     The organization key.
        /// </value>
        public Guid OrganizationKey { get; set; }

        #endregion
    }
}