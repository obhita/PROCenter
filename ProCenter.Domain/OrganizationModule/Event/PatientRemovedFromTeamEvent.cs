namespace ProCenter.Domain.OrganizationModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     Event for removing patient from team.
    /// </summary>
    public class PatientRemovedFromTeamEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PatientRemovedFromTeamEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="patientKey">The patient key.</param>
        public PatientRemovedFromTeamEvent ( Guid key, int version, Guid patientKey )
            : base ( key, version )
        {
            PatientKey = patientKey;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the patient key.
        /// </summary>
        /// <value>
        ///     The patient key.
        /// </value>
        public Guid PatientKey { get; private set; }

        #endregion
    }
}