#region Licence Header
// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/
#endregion
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
