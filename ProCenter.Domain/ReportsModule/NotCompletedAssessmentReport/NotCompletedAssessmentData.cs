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

namespace ProCenter.Domain.ReportsModule.NotCompletedAssessmentReport
{
    #region Using Statements

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// The NotCompletedAssessmentData class.
    /// </summary>
    public class NotCompletedAssessmentData
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public List<NotCompletedAssessmentDataObject> Data { get; set; } 

        /// <summary>
        /// Gets or sets the name of the report.
        /// </summary>
        /// <value>
        /// The name of the report.
        /// </value>
        public string ReportName { get; set; }

        /// <summary>
        /// Gets or sets the assessment parameter.
        /// </summary>
        /// <value>
        /// The assessment parameter.
        /// </value>
        public string AssessmentParameter { get; set; }

        /// <summary>
        /// Gets or sets the date range parameter.
        /// </summary>
        /// <value>
        /// The date range parameter.
        /// </value>
        public string DateRangeParameter { get; set; }

        /// <summary>
        /// Gets or sets the age group parameter.
        /// </summary>
        /// <value>
        /// The age group parameter.
        /// </value>
        public string AgeGroupParameter { get; set; }

        /// <summary>
        /// Gets or sets the genter parameter.
        /// </summary>
        /// <value>
        /// The genter parameter.
        /// </value>
        public string GenderParameter { get; set; }

        /// <summary>
        /// Gets or sets the name of the header.
        /// </summary>
        /// <value>
        /// The name of the header.
        /// </value>
        public string HeaderName { get; set; }

        /// <summary>
        /// Gets or sets the header age.
        /// </summary>
        /// <value>
        /// The header age.
        /// </value>
        public string HeaderAge { get; set; }

        /// <summary>
        /// Gets or sets the header gender.
        /// </summary>
        /// <value>
        /// The header gender.
        /// </value>
        public string HeaderGender { get; set; }

        /// <summary>
        /// Gets or sets the name of the header assessment.
        /// </summary>
        /// <value>
        /// The name of the header assessment.
        /// </value>
        public string HeaderAssessmentName { get; set; }

        /// <summary>
        /// Gets or sets the header assessment status.
        /// </summary>
        /// <value>
        /// The header assessment status.
        /// </value>
        public string HeaderAssessmentStatus { get; set; }

        /// <summary>
        /// Gets or sets the header view assessment.
        /// </summary>
        /// <value>
        /// The header view assessment.
        /// </value>
        public string HeaderViewAssessment { get; set; }

    #endregion
    }
}