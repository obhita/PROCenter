namespace ProCenter.Service.Message.Organization
{
    #region Using Statements

    using Common;
    using Primitive;

    #endregion

    /// <summary>
    ///     Data transfer object for Patient in context of a Team
    /// </summary>
    public class TeamPatientDto : KeyedDataTransferObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public PersonName Name { get; set; }

        #endregion
    }
}