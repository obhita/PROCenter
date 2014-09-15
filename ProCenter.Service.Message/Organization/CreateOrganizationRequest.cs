namespace ProCenter.Service.Message.Organization
{
    #region Using Statements

    using Agatha.Common;

    #endregion

    /// <summary>The create organization request class.</summary>
    public class CreateOrganizationRequest : Request
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        #endregion
    }
}