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

namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using System.ComponentModel.DataAnnotations;

    using Pillar.Common.InversionOfControl;

    using ProCenter.Common;
    using ProCenter.Service.Message.Attribute;
    using ProCenter.Service.Message.Common.Lookups;

    #endregion

    /// <summary>
    ///     The PatientScoreRangeParametersDto class.
    /// </summary>
    public class PatientScoreRangeParametersDto : BaseParametersDto
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the assessment definition code.
        /// </summary>
        /// <value>
        ///     The assessment definition code.
        /// </value>
        [Required]
        [Display ( ResourceType = typeof(Report), Name = "Assessment" )]
        public string AssessmentDefinitionCode { get; set; }

        /// <summary>
        ///     Gets or sets the name of the assessment.
        /// </summary>
        /// <value>
        ///     The name of the assessment.
        /// </value>
        [Required]
        public string AssessmentName { get; set; }

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
        [LookupCategory("Gender")]
        public LookupDto Gender { get; set; }

        /// <summary>
        /// Gets the parameter string.
        /// </summary>
        /// <returns>Returns all the parameters as a string.</returns>
        public override string ToString()
        {
            var and = string.Empty;
            var assessment = string.Empty;
            if ( AssessmentName != null )
            {
                assessment = AssessmentName;
            }
            var returnString = " " + Report.Of + assessment;
            if ( TimePeriod != null && !string.IsNullOrEmpty ( TimePeriod.Code ) )
            {
                var resources = IoC.CurrentContainer.Resolve<IResourcesManager> ();
                returnString += " " + resources.GetResourceManagerByName("ReportTimePeriod").GetString(TimePeriod.Code);
            }
            else
            {
                returnString += " " + StartDate.GetValueOrDefault().ToShortDateString () + " " + Report.ToLowerCase + " " + EndDate.GetValueOrDefault().ToShortDateString ();
            }
            if ( AgeRangeLow != null && AgeRangeHigh != null )
            {
                returnString += " " + Report.AgeRange + " " + AgeRangeLow + " " + Report.To + " " + AgeRangeHigh;
                and = Report.And; 
            }
            if ( Gender != null && Gender.Code != null )
            {
                returnString += " " + and + " " + Report.Gender + " " + Gender.Code;
                and = Report.And;
            }
            if ( ScoreType != null )
            {
                if (ScoreType.GetType() == typeof(ScoreTypeIntDto) && (((ScoreTypeIntDto)ScoreType).RiskStart > 0 || ((ScoreTypeIntDto)ScoreType).RiskEnd > 0))
                {
                    returnString += " " + 
                        and + " " + 
                        Report.ScoreRange + " " + 
                        ((ScoreTypeIntDto)ScoreType).RiskStart + 
                        " " + Report.To + 
                        " " + ((ScoreTypeIntDto)ScoreType).RiskEnd;
                }
                else if (ScoreType.GetType() == typeof(ScoreTypeBooleanDto))
                {
                    returnString += " " + and + " " + Report.IsAtRisk + " " + ((ScoreTypeBooleanDto)ScoreType).IsAtRisk;
                }
            }
            return returnString;
        }

        #endregion
    }
}