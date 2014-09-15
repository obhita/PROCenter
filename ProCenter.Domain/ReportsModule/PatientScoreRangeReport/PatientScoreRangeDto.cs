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

namespace ProCenter.Domain.ReportsModule.PatientScoreRangeReport
{
    #region Using Statements

    using System;

    #endregion

    /// <summary>
    ///     The PatientScoreRangeDto class.
    /// </summary>
    public class PatientScoreRangeDto
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the assessment instance key.
        /// </summary>
        /// <value>
        ///     The assessment instance key.
        /// </value>
        public Guid AssessmentInstanceKey { get; set; }

        /// <summary>
        ///     Gets or sets the name of the assessment.
        /// </summary>
        /// <value>
        ///     The name of the assessment.
        /// </value>
        public string AssessmentName { get; set; }

        /// <summary>
        ///     Gets or sets the assessment score.
        /// </summary>
        /// <value>
        ///     The assessment score.
        /// </value>
        public string AssessmentScore { get; set; }

        /// <summary>
        ///     Gets or sets the patient age.
        /// </summary>
        /// <value>
        ///     The patient age.
        /// </value>
        public int PatientAge { get; set; }

        /// <summary>
        ///     Gets or sets the patient birth date.
        /// </summary>
        /// <value>
        ///     The patient birth date.
        /// </value>
        public DateTime PatientBirthDate { get; set; }

        /// <summary>
        ///     Gets or sets the first name of the patient.
        /// </summary>
        /// <value>
        ///     The first name of the patient.
        /// </value>
        public string PatientFirstName { get; set; }

        /// <summary>
        ///     Gets or sets the patient gender.
        /// </summary>
        /// <value>
        ///     The patient gender.
        /// </value>
        public string PatientGender { get; set; }

        /// <summary>
        ///     Gets or sets the patient key.
        /// </summary>
        /// <value>
        ///     The patient key.
        /// </value>
        public Guid PatientKey { get; set; }

        /// <summary>
        ///     Gets or sets the last name of the patient.
        /// </summary>
        /// <value>
        ///     The last name of the patient.
        /// </value>
        public string PatientLastName { get; set; }

        /// <summary>
        ///     Gets or sets the score change.
        /// </summary>
        /// <value>
        ///     The score change.
        /// </value>
        public string ScoreChange { get; set; }

        /// <summary>
        ///     Gets or sets the score date.
        /// </summary>
        /// <value>
        ///     The score date.
        /// </value>
        public DateTime ScoreDate { get; set; }

        #endregion
    }
}