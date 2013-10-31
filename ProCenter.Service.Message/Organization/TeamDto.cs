namespace ProCenter.Service.Message.Organization
{
    #region Using Statements

    using System.Collections.Generic;
    using Common;

    #endregion

    /// <summary>
    ///     Data transfer object for team.
    /// </summary>
    public class TeamDto : KeyedDataTransferObject
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
        ///     Gets or sets the patients.
        /// </summary>
        /// <value>
        ///     The patients.
        /// </value>
        public IList<TeamPatientDto> Patients { get; set; }

        /// <summary>
        ///     Gets or sets the staff.
        /// </summary>
        /// <value>
        ///     The staff.
        /// </value>
        public IList<TeamStaffDto> Staff { get; set; }

        #endregion
    }
}