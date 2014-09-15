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

    #region Using Statements

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Primitive;

#endregion

namespace ProCenter.Domain.ReportsModule.PatientScoreRangeReport
{
    using System;

    /// <summary>The assessment score over time parameters class.</summary>
    public class PatientScoreRangeParameters : BaseReportParameters, IReportModel
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the assessment definition code.
        /// </summary>
        /// <value>
        ///     The assessment definition code.
        /// </value>
        public string AssessmentDefinitionCode { get; set; }

        /// <summary>
        ///     Gets or sets the name of the patient.
        /// </summary>
        /// <value>
        ///     The name of the patient.
        /// </value>
        public PersonName PatientName { get; set; }

        /// <summary>
        /// Gets or sets the age range low.
        /// </summary>
        /// <value>
        /// The age range low.
        /// </value>
        public int? AgeRangeLow { get; set; }

        /// <summary>
        /// Gets or sets the age range high.
        /// </summary>
        /// <value>
        /// The age range high.
        /// </value>
        public int? AgeRangeHigh { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the type of the score.
        /// </summary>
        /// <value>
        /// The type of the score.
        /// </value>
        public IScoreType ScoreType { get; set; }

        #endregion
    }
}