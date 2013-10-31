namespace ProCenter.Domain.PatientModule
{
    using System;

    /// <summary>
    /// This class provides the algorithm to Generate a unique identifier for a patient.
    /// </summary>
    public class PatientUniqueIdentifierGenerator : IPatientUniqueIdentifierGenerator
    {

        /// <summary>
        /// Generates the unique identifier.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>The generated unique identifier.</returns>
        public string GenerateUniqueIdentifier(Guid key, string lastName, Gender gender, DateTime dateOfBirth)
        {
            var identifier = key.ToString();

            if (gender != null && dateOfBirth != default(DateTime))
            {
                identifier = GenerateUniqueUniqueIdentifier(lastName, gender.CodedConcept.Name, dateOfBirth);
            }

            return identifier;
        }

        private string GenerateUniqueUniqueIdentifier(string lastName, string gender, DateTime birthDate)
        {
            var firstDigitLastName = lastName.Substring(0, 1);
            var firstDigitGender = gender.Substring(0, 1);
            var birthDateAsString = birthDate.ToString("MMddyyyy");

            var identifier = firstDigitGender + birthDateAsString + firstDigitLastName;

            var oddChars = string.Empty;
            var evenChars = string.Empty;
            for (var chIdx = 0; chIdx < identifier.Length; chIdx++)
            {
                var ch = identifier[chIdx];
                if (chIdx % 2 == 0)
                {
                    evenChars += ch;
                }
                else
                {
                    oddChars = ch + oddChars;
                }
            }

            identifier = evenChars + oddChars;

            return identifier;
        }
    }
}
