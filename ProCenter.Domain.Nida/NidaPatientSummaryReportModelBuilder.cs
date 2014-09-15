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
    using System.Resources;

    using Pillar.Common.Metadata;
    using Pillar.Common.Utility;

    using ProCenter.Common;
    using ProCenter.Common.Report;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>
    ///     Builder for NIDA Patient Summary report model types.
    /// </summary>
    public class NidaPatientSummaryReportModelBuilder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Fills the defaults.
        /// </summary>
        /// <param name="reportModel">The report model.</param>
        public static void FillDefaults ( ReportModel reportModel )
        {
            reportModel.RecurseReportItems (
                                            reportItem =>
                                            {
                                                if ( reportItem.Text == null )
                                                {
                                                    reportItem.Update (
                                                                       reportItem.ShouldShow,
                                                        reportItem.FormatParameters.Any ()
                                                            ? string.Format (
                                                                             NidaWorkflowPatientSummaryReport.ResourceManager.GetString ( reportItem.Name ) ?? string.Empty,
                                                                reportItem.FormatParameters.ToArray () )
                                                            : NidaWorkflowPatientSummaryReport.ResourceManager.GetString ( reportItem.Name ) );
                                                }
                                                return true;
                                            } );
        }

        /// <summary>
        ///     Gets the amber report model.
        /// </summary>
        /// <param name="resourcesManager">The resources manager.</param>
        /// <param name="dastInstance">The dast instance.</param>
        /// <param name="nidaAssessFurtherInstance">The nida assess further instance.</param>
        /// <returns>A <see cref="ReportModel" />.</returns>
        public static ReportModel GetAmberReportModel ( IResourcesManager resourcesManager, AssessmentInstance dastInstance, AssessmentInstance nidaAssessFurtherInstance )
        {
            var reportModel = CreateSummarySection ( resourcesManager, dastInstance, nidaAssessFurtherInstance );
            reportModel.ReportSeverity = ReportSeverity.Low;
            reportModel.ReportStatus = NidaWorkflowPatientSummaryReport.Amber;
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> (
                                                                                                     r =>
                                                                                                         r.BenefitOfDrugAbstinentFeedback ),
                                           true ) );
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.FollowUpHeader ),
                                           null,
                                           null,
                                           new ReportItem (
                                               "FollowUpVisit",
                                               true,
                                               new object[]
                                               {
                                                   DateTime.Now.AddMonths ( 6 )
                                                       .ToShortDateString ()
                                               } ) ) );
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> (
                                                                                                     r =>
                                                                                                         r
                                                                                                             .PatientResourceHeader ),
                                           null,
                                           null,
                                           new ReportItem (
                                               "UsDepartmentHealthAndHumanServices",
                                               true ) ) );
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.AdditionalNotes ) ) );

            reportModel.IsPatientViewable = true;
            return reportModel;
        }

        /// <summary>
        ///     Gets the green report model.
        /// </summary>
        /// <returns>A <see cref="ReportModel" />.</returns>
        public static ReportModel GetGreenReportModel ()
        {
            var reportModel = new ReportModel
                              {
                                  Name = ReportNames.NidaPatientSummaryReport,
                                  IsCustomizable = true,
                                  ReportSeverity = ReportSeverity.Good,
                                  ReportStatus = NidaWorkflowPatientSummaryReport.Green
                              };
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.PositiveFeedback ),
                                           true ) );
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.FollowUpHeader ),
                                           null,
                                           null,
                                           new ReportItem (
                                               "FollowUpVisit",
                                               true,
                                               new object[] { DateTime.Now.AddYears ( 1 ).ToShortDateString () } ) ) );
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.AdditionalNotes ) ) );

            reportModel.IsPatientViewable = true;
            return reportModel;
        }

        /// <summary>
        ///     Gets the red report model.
        /// </summary>
        /// <param name="resourcesManager">The resources manager.</param>
        /// <param name="dastInstance">The dast instance.</param>
        /// <param name="nidaAssessFurtherInstance">The nida assess further instance.</param>
        /// <returns>A <see cref="ReportModel" />.</returns>
        public static ReportModel GetRedReportModel ( IResourcesManager resourcesManager, AssessmentInstance dastInstance, AssessmentInstance nidaAssessFurtherInstance )
        {
            var reportModel = CreateSummarySection ( resourcesManager, dastInstance, nidaAssessFurtherInstance );
            reportModel.ReportSeverity = ReportSeverity.High;
            reportModel.ReportStatus = NidaWorkflowPatientSummaryReport.High;
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.RecommendCessation ),
                                           true ) );
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.ReferralForFurtherAssessment ),
                                           true ) );
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.FollowUpHeader ),
                                           null,
                                           null,
                                           new ReportItem ( "AssessReadinessToChange", true ),
                                           new ReportItem ( "ReviewCurrentMedication", true ),
                                           new ReportItem ( "MentalHealthCondition", true ),
                                           new ReportItem ( "PreventiveScreenings", true ),
                                           new ReportItem (
                                               "FollowUpVisit",
                                               true,
                                               new object[]
                                               {
                                                   DateTime.Now.AddMonths ( 6 )
                                                       .ToShortDateString ()
                                               } ) ) );
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.UseTreatmentHistoryHeader ),
                                           null,
                                           null,
                                           new ReportItem ( "ObtainDrugTreatmentHistory", true ),
                                           new ReportItem ( "ObtainTobaccoAlcoholHistory", true )
                                           )
                                       {
                                           ItemMetadata = new ItemMetadata ()
                                       } );
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.SupportGroups ),
                                           true ) );
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.UrineDrugScreen ),
                                           true ) );
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> (
                                                                                                     r => r.PatientResourceHeader ),
                                           null,
                                           null,
                                           new ReportItem ( "UsDepartmentHealthAndHumanServices", true ) )
                );
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.OpioidDependenceOnSiteTreatment ),
                                           true ) );
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.OpioidDependenceOffSiteTreatment ),
                                           true ) );
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.AdditionalNotes ) ) );

            reportModel.IsPatientViewable = true;
            return reportModel;
        }

        #endregion

        #region Methods

        private static void CheckAddDrugFrequency(List<ReportItem> reportItems, ResourceManager nidaAssessFurtherResources, Lookup item, string displayName)
        {
            if ( item != null )
            {
                reportItems.Add (
                                 new ReportItem (
                                     PropertyUtil.ExtractPropertyName<NidaReportData, List<ReportString>> ( r => r.SummaryItems ),
                                     null,
                                     new object[]
                                     {
                                         displayName,
                                         item.DisplayName
                                     }
                                     )
                                 {
                                     ItemMetadata = new ItemMetadata { MetadataItems = new List<IMetadataItem> { new ReadonlyMetadataItem { IsReadonly = true } } }
                                 } );
            }
        }

        private static ReportModel CreateSummarySection ( IResourcesManager resourcesManager, AssessmentInstance dastInstance, AssessmentInstance nidaAssessFurtherInstance )
        {
            var reportModel = new ReportModel
                              {
                                  Name = ReportNames.NidaPatientSummaryReport,
                                  IsCustomizable = true
                              };
            var reportItems = new List<ReportItem> ();
            var nidaAssessFurtherResources = resourcesManager == null
                ? new ResourceManager ( typeof(NidaAssessFurther) )
                : resourcesManager.GetResourceManagerByName ( NidaAssessFurther.AssessmentCodedConcept.Code );

            var nidaAssessFurther = new NidaAssessFurther ( nidaAssessFurtherInstance );
            var displayName = nidaAssessFurtherResources.GetString (
                "_" + AssessmentPart.GetPropertyCodedConcept<NidaAssessFurther, Lookup>(
                a => a.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceOpioidPersonalMedicalHistoryFrequency).Code);
            CheckAddDrugFrequency (
                                    reportItems,
                                    nidaAssessFurtherResources,
                                    nidaAssessFurther.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceOpioidPersonalMedicalHistoryFrequency,
                                    displayName);
            displayName = nidaAssessFurtherResources.GetString(
                "_" + AssessmentPart.GetPropertyCodedConcept<NidaAssessFurther, Lookup>(
                a => a.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceCocainePersonalMedicalHistoryFrequency).Code);
            CheckAddDrugFrequency(
                                    reportItems,
                                    nidaAssessFurtherResources,
                                    nidaAssessFurther.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceCocainePersonalMedicalHistoryFrequency,
                                    displayName);
            displayName = nidaAssessFurtherResources.GetString(
                "_" + AssessmentPart.GetPropertyCodedConcept<NidaAssessFurther, Lookup>(
                a => a.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceMarijuanaPersonalMedicalHistoryFrequency).Code);
            CheckAddDrugFrequency(
                                    reportItems,
                                    nidaAssessFurtherResources,
                                    nidaAssessFurther.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceMarijuanaPersonalMedicalHistoryFrequency,
                                    displayName);
            displayName = nidaAssessFurtherResources.GetString(
                "_" + AssessmentPart.GetPropertyCodedConcept<NidaAssessFurther, Lookup>(
                a => a.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceSedativePersonalMedicalHistoryFrequency).Code);
            CheckAddDrugFrequency(
                                    reportItems,
                                    nidaAssessFurtherResources,
                                    nidaAssessFurther.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceSedativePersonalMedicalHistoryFrequency,
                                    displayName);
            displayName = nidaAssessFurtherResources.GetString(
                "_" + AssessmentPart.GetPropertyCodedConcept<NidaAssessFurther, Lookup>(
                a => a.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceStimulantPersonalMedicalHistoryFrequency).Code);
            CheckAddDrugFrequency(
                                    reportItems,
                                    nidaAssessFurtherResources,
                                    nidaAssessFurther.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceStimulantPersonalMedicalHistoryFrequency,
                                    displayName);
            displayName = nidaAssessFurther.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceOtherSubstanceofAbusePersonalMedicalHistorySpecify;
            CheckAddDrugFrequency(
                                    reportItems,
                                    nidaAssessFurtherResources,
                                    nidaAssessFurther.DrugUseFrequencyGroup.SubstanceAbuseIllicitSubstanceOtherSubstanceofAbusePersonalMedicalHistoryFrequency,
                                    displayName);
            reportModel.AddReportItem (
                                       new ReportItem (
                                           PropertyUtil.ExtractPropertyName<NidaReportData, string> ( r => r.SummaryHeader ),
                                           null,
                                           new[]
                                           {
                                               dastInstance.Score.Value
                                           },
                                           reportItems.ToArray ()
                                           )
                                       {
                                           ItemMetadata = new ItemMetadata { MetadataItems = new List<IMetadataItem> { new ReadonlyMetadataItem { IsReadonly = true } } }
                                       } );
            return reportModel;
        }

        #endregion
    }
}