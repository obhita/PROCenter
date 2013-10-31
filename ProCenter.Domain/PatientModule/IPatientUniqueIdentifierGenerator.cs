namespace ProCenter.Domain.PatientModule
{
    using System;

    /// <summary>
    /// Interface to generating patient unique identifier
    /// </summary>
    public interface IPatientUniqueIdentifierGenerator
    {
        /// <summary>
        /// Generates the unique identifier.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns></returns>
        string GenerateUniqueIdentifier ( Guid key, string lastName, Gender gender, DateTime dateOfBirth );
    }
}