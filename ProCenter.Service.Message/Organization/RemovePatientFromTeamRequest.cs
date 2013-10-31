namespace ProCenter.Service.Message.Organization
{
    #region Using Statements

    using System;
    using Agatha.Common;

    #endregion

    /// <summary>
    ///     Request to remove patient from team.
    /// </summary>
    public class RemovePatientFromTeamRequest : Request
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the patient key.
        /// </summary>
        /// <value>
        ///     The patient key.
        /// </value>
        public Guid PatientKey { get; set; }

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