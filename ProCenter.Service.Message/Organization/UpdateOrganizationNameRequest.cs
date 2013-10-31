namespace ProCenter.Service.Message.Organization
{
    #region Using Statements

    using System;
    using Agatha.Common;

    #endregion

    /// <summary>
    ///     Request to update organization name;
    /// </summary>
    public class UpdateOrganizationNameRequest : Request
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the key.
        /// </summary>
        /// <value>
        ///     The key.
        /// </value>
        public Guid Key { get; set; }

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