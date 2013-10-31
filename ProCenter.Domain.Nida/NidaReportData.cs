#region License Header
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
namespace ProCenter.Domain.Nida
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AssessmentModule;
    using Common.Report;

    #endregion

    /// <summary>
    ///     NIDA Report Data
    /// </summary>
    public class NidaReportData
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="NidaReportData" /> class.
        /// </summary>
        public NidaReportData ()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NidaReportData" /> class.
        /// </summary>
        /// <param name="reportModel">The report model.</param>
        public NidaReportData ( ReportModel reportModel )
        {
            SetValues ( reportModel.FindReportItem ( "PositiveFeedback" ), s => PositiveFeedback = s );
            SetValues ( reportModel.FindReportItem ( "SummaryHeader" ), s => SummaryHeader = s, SummaryItems = new List<ReportString> () );
            SetValues ( reportModel.FindReportItem ( "RecommendCessation" ), s => RecommendCessation = s );
            SetValues ( reportModel.FindReportItem ( "BenefitOfDrugAbstinentFeedback" ), s => BenefitOfDrugAbstinentFeedback = s );
            SetValues ( reportModel.FindReportItem ( "ReferralForFurtherAssessment" ), s => ReferralForFurtherAssessment = s );
            SetValues ( reportModel.FindReportItem ( "FollowUpHeader" ), s => FollowUpHeader = s, FollowUpItems = new List<ReportString> () );
            SetValues ( reportModel.FindReportItem ( "UseTreatmentHistoryHeader" ), s => UseTreatmentHistoryHeader = s, UseTreatmentHistoryItems = new List<ReportString> () );
            SetValues ( reportModel.FindReportItem ( "SupportGroups" ), s => SupportGroups = s );
            SetValues ( reportModel.FindReportItem ( "UrineDrugScreen" ), s => UrineDrugScreen = s );
            SetValues ( reportModel.FindReportItem ( "PatientResourceHeader" ), s => PatientResourceHeader = s, PatientResourceItems = new List<ReportString> () );
            SetValues ( reportModel.FindReportItem ( "OpioidDependenceOnSiteTreatment" ), s => OpioidDependenceOnSiteTreatment = s );
            SetValues ( reportModel.FindReportItem ( "OpioidDependenceOffSiteTreatment" ), s => OpioidDependenceOffSiteTreatment = s );
            SetValues ( reportModel.FindReportItem ( "AdditionalNotes" ), s => AdditionalNotes = s );
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the additional notes.
        /// </summary>
        /// <value>
        ///     The additional notes.
        /// </value>
        public string AdditionalNotes { get; set; }

        /// <summary>
        ///     Gets or sets the benefit of drug abstinent feedback.
        /// </summary>
        /// <value>
        ///     The benefit of drug abstinent feedback.
        /// </value>
        public string BenefitOfDrugAbstinentFeedback { get; set; }

        /// <summary>
        ///     Gets or sets the follow up header.
        /// </summary>
        /// <value>
        ///     The follow up header.
        /// </value>
        public string FollowUpHeader { get; set; }

        /// <summary>
        ///     Gets or sets the follow up items.
        /// </summary>
        /// <value>
        ///     The follow up items.
        /// </value>
        public List<ReportString> FollowUpItems { get; set; }

        /// <summary>
        ///     Gets or sets the opioid dependence off site treatment.
        /// </summary>
        /// <value>
        ///     The opioid dependence off site treatment.
        /// </value>
        public string OpioidDependenceOffSiteTreatment { get; set; }

        /// <summary>
        ///     Gets or sets the opioid dependence on site treatment.
        /// </summary>
        /// <value>
        ///     The opioid dependence on site treatment.
        /// </value>
        public string OpioidDependenceOnSiteTreatment { get; set; }

        /// <summary>
        ///     Gets or sets the patient resource header.
        /// </summary>
        /// <value>
        ///     The patient resource header.
        /// </value>
        public string PatientResourceHeader { get; set; }

        /// <summary>
        ///     Gets or sets the patient resource items.
        /// </summary>
        /// <value>
        ///     The patient resource items.
        /// </value>
        public List<ReportString> PatientResourceItems { get; set; }

        /// <summary>
        ///     Gets or sets the positive feedback.
        /// </summary>
        /// <value>
        ///     The positive feedback.
        /// </value>
        public string PositiveFeedback { get; set; }

        /// <summary>
        ///     Gets or sets the recommend cessation.
        /// </summary>
        /// <value>
        ///     The recommend cessation.
        /// </value>
        public string RecommendCessation { get; set; }

        /// <summary>
        ///     Gets or sets the referral for further assessment.
        /// </summary>
        /// <value>
        ///     The referral for further assessment.
        /// </value>
        public string ReferralForFurtherAssessment { get; set; }

        /// <summary>
        ///     Gets or sets the summary header.
        /// </summary>
        /// <value>
        ///     The summary header.
        /// </value>
        public string SummaryHeader { get; set; }

        /// <summary>
        ///     Gets or sets the summary items.
        /// </summary>
        /// <value>
        ///     The summary items.
        /// </value>
        public List<ReportString> SummaryItems { get; set; }

        /// <summary>
        ///     Gets or sets the support groups.
        /// </summary>
        /// <value>
        ///     The support groups.
        /// </value>
        public string SupportGroups { get; set; }

        /// <summary>
        ///     Gets or sets the urine drug screen.
        /// </summary>
        /// <value>
        ///     The urine drug screen.
        /// </value>
        public string UrineDrugScreen { get; set; }

        /// <summary>
        ///     Gets or sets the use treatment history header.
        /// </summary>
        /// <value>
        ///     The use treatment history header.
        /// </value>
        public string UseTreatmentHistoryHeader { get; set; }

        /// <summary>
        ///     Gets or sets the use treatment history items.
        /// </summary>
        /// <value>
        ///     The use treatment history items.
        /// </value>
        public List<ReportString> UseTreatmentHistoryItems { get; set; }

        #endregion

        #region Methods

        private void SetValues ( ReportItem reportItem, Action<string> setProperty, List<ReportString> items = null )
        {
            if ( reportItem == null || reportItem.ShouldShow == false )
            {
                return;
            }
            if ( reportItem.Text != null && ( items == null || ( reportItem.ReportItems != null && reportItem.ReportItems.Any ( ri => ri.ShouldShow != false) ) ) )
            {
                setProperty ( reportItem.Text );
            }
            if ( reportItem.ReportItems != null && reportItem.ReportItems.Any () && items != null )
            {
                items.AddRange ( reportItem.ReportItems.Where ( ri => ri.ShouldShow != false ).Select ( ri => (ReportString) ri.Text ) );
            }
        }

        #endregion
    }
}