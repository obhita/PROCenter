using System;
using System.Collections.Generic;

namespace ProCenter.Domain.Psc
{
    using ProCenter.Domain.CommonModule.Lookups;
    using ProCenter.Domain.PatientModule;

    /// <summary>
    /// This class has all common functions for all reports in PSC Project.
    /// </summary>
    public class Common
    {
        /// <summary>
        /// Gets the answer value.
        /// </summary>
        /// <param name="tf">The tf.</param>
        /// <returns>Returns the string with the Value and Name together.</returns>
        public static string GetAnswerValue(Lookup tf)
        {
            return tf.Value + " - " + tf.CodedConcept.Name;
        }

        /// <summary>
        /// Adjusts the score based on age.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="symptons">The symptons.</param>
        /// <returns>Returns an integer of any score sums to subtract from total by the caller.</returns>
        public static int AdjustScoreBasedOnAge(Patient patient, List<TimeFrequency> symptons)
        {
            var patientAge = GetAge(patient);
            var subtractTotal = 0;
            if (patientAge >= 4 && patientAge <= 5)
            {
                subtractTotal = GetScores(symptons);
            }
            return subtractTotal;
        }

        /// <summary>
        /// Gets the scores.
        /// </summary>
        /// <param name="timeFrequencyList">The time frequency list.</param>
        /// <returns>Returns the correct sum based on the list values passed in.</returns>
        public static int GetScores(IEnumerable<TimeFrequency> timeFrequencyList)
        {
            var returnScore = 0;
            foreach (var item in timeFrequencyList)
            {
                if (item == TimeFrequency.Often)
                {
                    returnScore += 2;
                }
                else if (item == TimeFrequency.Sometimes)
                {
                    returnScore += 1;
                }
            }
            return returnScore;
        }

        /// <summary>
        /// Gets the age.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>The age of the patient baased on their birth date.</returns>
        public static int GetAge(Patient patient)
        {
            if (patient.DateOfBirth == null || patient.DateOfBirth.Value == null)
            {
                return 0;
            }
            var now = DateTime.Today;
            var age = now.Year - patient.DateOfBirth.Value.Year;
            if (now < patient.DateOfBirth.Value.AddYears(age))
            {
                age--;
            }
            return age;
        }
    }
}
