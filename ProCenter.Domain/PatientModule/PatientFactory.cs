namespace ProCenter.Domain.PatientModule
{
    #region Using Statements

    using System;
    using Primitive;

    #endregion

    /// <summary>
    ///     Class for creating patients.
    /// </summary>
    public class PatientFactory : IPatientFactory
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
        public Patient Create (Guid organizationKey, PersonName personName, DateTime? dateOfBirth, Gender gender )
        {
            var patient = new Patient (organizationKey, personName, dateOfBirth, gender );
            return patient;
        }

        #endregion
    }
} 