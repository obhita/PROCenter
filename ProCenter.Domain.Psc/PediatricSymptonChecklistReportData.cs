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

namespace ProCenter.Domain.Psc
{
    #region Using Statements

    using ProCenter.Domain.AssessmentModule;

    #endregion

    /// <summary>
    /// Pediatric Sympton Check list ReportData.
    /// </summary>
    public class PediatricSymptonChecklistReportData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PediatricSymptonChecklistReportData"/> class.
        /// </summary>
        /// <param name="pediatricSymptonChecklist">The pediatric sympton checklist.</param>
        public PediatricSymptonChecklistReportData (PediatricSymptonChecklist pediatricSymptonChecklist)
        {
            PediatricSymptonChecklist = pediatricSymptonChecklist;
            AssessmentInstance = pediatricSymptonChecklist.AssessmentInstance;
        }

        #region Public Properties

        /// <summary>
        ///     Gets or sets the age.
        /// </summary>
        /// <value>
        ///     The age.
        /// </value>
        public int Age { get; set; }

        /// <summary>
        ///     Gets or sets the answer1.
        /// </summary>
        /// <value>
        ///     The answer1.
        /// </value>
        public string Answer1 { get; set; }

        /// <summary>
        ///     Gets or sets the answer10.
        /// </summary>
        /// <value>
        ///     The answer10.
        /// </value>
        public string Answer10 { get; set; }

        /// <summary>
        ///     Gets or sets the answer11.
        /// </summary>
        /// <value>
        ///     The answer11.
        /// </value>
        public string Answer11 { get; set; }

        /// <summary>
        ///     Gets or sets the answer12.
        /// </summary>
        /// <value>
        ///     The answer12.
        /// </value>
        public string Answer12 { get; set; }

        /// <summary>
        ///     Gets or sets the answer13.
        /// </summary>
        /// <value>
        ///     The answer13.
        /// </value>
        public string Answer13 { get; set; }

        /// <summary>
        ///     Gets or sets the answer14.
        /// </summary>
        /// <value>
        ///     The answer14.
        /// </value>
        public string Answer14 { get; set; }

        /// <summary>
        ///     Gets or sets the answer15.
        /// </summary>
        /// <value>
        ///     The answer15.
        /// </value>
        public string Answer15 { get; set; }

        /// <summary>
        ///     Gets or sets the answer16.
        /// </summary>
        /// <value>
        ///     The answer16.
        /// </value>
        public string Answer16 { get; set; }

        /// <summary>
        ///     Gets or sets the answer17.
        /// </summary>
        /// <value>
        ///     The answer17.
        /// </value>
        public string Answer17 { get; set; }

        /// <summary>
        ///     Gets or sets the answer18.
        /// </summary>
        /// <value>
        ///     The answer18.
        /// </value>
        public string Answer18 { get; set; }

        /// <summary>
        ///     Gets or sets the answer19.
        /// </summary>
        /// <value>
        ///     The answer19.
        /// </value>
        public string Answer19 { get; set; }

        /// <summary>
        ///     Gets or sets the answer2.
        /// </summary>
        /// <value>
        ///     The answer2.
        /// </value>
        public string Answer2 { get; set; }

        /// <summary>
        ///     Gets or sets the answer20.
        /// </summary>
        /// <value>
        ///     The answer20.
        /// </value>
        public string Answer20 { get; set; }

        /// <summary>
        ///     Gets or sets the answer21.
        /// </summary>
        /// <value>
        ///     The answer21.
        /// </value>
        public string Answer21 { get; set; }

        /// <summary>
        ///     Gets or sets the answer22.
        /// </summary>
        /// <value>
        ///     The answer22.
        /// </value>
        public string Answer22 { get; set; }

        /// <summary>
        ///     Gets or sets the answer23.
        /// </summary>
        /// <value>
        ///     The answer23.
        /// </value>
        public string Answer23 { get; set; }

        /// <summary>
        ///     Gets or sets the answer24.
        /// </summary>
        /// <value>
        ///     The answer24.
        /// </value>
        public string Answer24 { get; set; }

        /// <summary>
        ///     Gets or sets the answer25.
        /// </summary>
        /// <value>
        ///     The answer25.
        /// </value>
        public string Answer25 { get; set; }

        /// <summary>
        ///     Gets or sets the answer26.
        /// </summary>
        /// <value>
        ///     The answer26.
        /// </value>
        public string Answer26 { get; set; }

        /// <summary>
        ///     Gets or sets the answer27.
        /// </summary>
        /// <value>
        ///     The answer27.
        /// </value>
        public string Answer27 { get; set; }

        /// <summary>
        ///     Gets or sets the answer28.
        /// </summary>
        /// <value>
        ///     The answer28.
        /// </value>
        public string Answer28 { get; set; }

        /// <summary>
        ///     Gets or sets the answer29.
        /// </summary>
        /// <value>
        ///     The answer29.
        /// </value>
        public string Answer29 { get; set; }

        /// <summary>
        ///     Gets or sets the answer3.
        /// </summary>
        /// <value>
        ///     The answer3.
        /// </value>
        public string Answer3 { get; set; }

        /// <summary>
        ///     Gets or sets the answer30.
        /// </summary>
        /// <value>
        ///     The answer30.
        /// </value>
        public string Answer30 { get; set; }

        /// <summary>
        ///     Gets or sets the answer31.
        /// </summary>
        /// <value>
        ///     The answer31.
        /// </value>
        public string Answer31 { get; set; }

        /// <summary>
        ///     Gets or sets the answer32.
        /// </summary>
        /// <value>
        ///     The answer32.
        /// </value>
        public string Answer32 { get; set; }

        /// <summary>
        ///     Gets or sets the answer33.
        /// </summary>
        /// <value>
        ///     The answer33.
        /// </value>
        public string Answer33 { get; set; }

        /// <summary>
        ///     Gets or sets the answer34.
        /// </summary>
        /// <value>
        ///     The answer34.
        /// </value>
        public string Answer34 { get; set; }

        /// <summary>
        ///     Gets or sets the answer35.
        /// </summary>
        /// <value>
        ///     The answer35.
        /// </value>
        public string Answer35 { get; set; }

        /// <summary>
        ///     Gets or sets the answer4.
        /// </summary>
        /// <value>
        ///     The answer4.
        /// </value>
        public string Answer4 { get; set; }

        /// <summary>
        ///     Gets or sets the answer5.
        /// </summary>
        /// <value>
        ///     The answer5.
        /// </value>
        public string Answer5 { get; set; }

        /// <summary>
        ///     Gets or sets the answer6.
        /// </summary>
        /// <value>
        ///     The answer6.
        /// </value>
        public string Answer6 { get; set; }

        /// <summary>
        ///     Gets or sets the answer7.
        /// </summary>
        /// <value>
        ///     The answer7.
        /// </value>
        public string Answer7 { get; set; }

        /// <summary>
        ///     Gets or sets the answer8.
        /// </summary>
        /// <value>
        ///     The answer8.
        /// </value>
        public string Answer8 { get; set; }

        /// <summary>
        ///     Gets or sets the answer9.
        /// </summary>
        /// <value>
        ///     The answer9.
        /// </value>
        public string Answer9 { get; set; }

        /// <summary>
        ///     Gets or sets the anxiety depression subscale total.
        /// </summary>
        /// <value>
        ///     The anxiety depression subscale total.
        /// </value>
        public int AnxietyDepressionSubscaleTotal { get; set; }

        /// <summary>
        ///     Gets or sets the assessment instance.
        /// </summary>
        /// <value>
        ///     The assessment instance.
        /// </value>
        public AssessmentInstance AssessmentInstance { get; set; }

        /// <summary>
        ///     Gets or sets the attention problem subscale total.
        /// </summary>
        /// <value>
        ///     The attention problem subscale total.
        /// </value>
        public int AttentionProblemSubscaleTotal { get; set; }

        /// <summary>
        ///     Gets or sets the conduct problem subscale total.
        /// </summary>
        /// <value>
        ///     The conduct problem subscale total.
        /// </value>
        public int ConductProblemSubscaleTotal { get; set; }

        /// <summary>
        ///     Gets or sets the pediatric sympton checklist.
        /// </summary>
        /// <value>
        ///     The pediatric sympton checklist.
        /// </value>
        public PediatricSymptonChecklist PediatricSymptonChecklist { get; set; }

        /// <summary>
        ///     Gets or sets the screening date.
        /// </summary>
        /// <value>
        ///     The screening date.
        /// </value>
        public string ScreeningDate { get; set; }

        /// <summary>
        ///     Gets or sets the name of the staff.
        /// </summary>
        /// <value>
        ///     The name of the staff.
        /// </value>
        public string StaffName { get; set; }

        /// <summary>
        ///     Gets or sets the total score.
        /// </summary>
        /// <value>
        ///     The total score.
        /// </value>
        public int TotalScore { get; set; }

        #endregion
    }
}