namespace ProCenter.Domain.PatientModule
{
    #region Using Statements

    using System;
    using Primitive;

    #endregion

    /// <summary>
    ///     Interface for creating patient aggregates.
    /// </summary>
    public interface IPatientFactory
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Creates the specified person name.
        /// </summary>
        /// <param name="organizationKey">Organization Key.</param>
        /// <param name="personName">Name of the person.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="gender">The gender.</param>
        /// <returns>
        ///     A <see cref="Patient" />.
        /// </returns>
        Patient Create ( Guid organizationKey, PersonName personName, DateTime? dateOfBirth, Gender gender );

        #endregion
    }
}