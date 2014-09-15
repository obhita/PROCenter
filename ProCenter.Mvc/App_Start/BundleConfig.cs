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

namespace ProCenter.Mvc.App_Start
{
    #region Using Statements

    using System.Web.Optimization;

    #endregion

    /// <summary>The bundle configuration class.</summary>
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725

        #region Public Methods and Operators

        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles ( BundleCollection bundles )
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add ( new ScriptBundle ( "~/bundles/vendor" ).Include (
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap-collapse.js",
                "~/Scripts/bootstrap-transition.js",
                "~/Scripts/bootstrap-alert.js",
                "~/Scripts/bootstrap-button.js",
                "~/Scripts/bootstrap-dropdown.js",
                "~/Scripts/bootstrap-modal.js",
                "~/Scripts/bootstrap-tooltip.js",
                "~/Scripts/bootstrap-popover.js",
                "~/Scripts/bootstrap-multiselect.js",
                "~/Scripts/DataTables-1.9.4/media/js/jquery.dataTables.js",
                "~/Scripts/DataTables-1.9.4/media/js/jquery.dataTables.*",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery.procenter*",
                "~/Scripts/modernizr-*",
                "~/Scripts/fullcalendar.js",
                "~/Scripts/jquery.formatCurrency*",
                "~/Scripts/jquery.numeric.js",
                "~/Scripts/jquery-typing.js",
                "~/Scripts/knockout-3.1.0.js"
                ));

            bundles.Add ( new ScriptBundle ( "~/ClientBusinessRulesEngine" ).Include ( "~/Scripts/ClientBusinessRulesEngine.js" ) );

            bundles.Add(new ScriptBundle("~/ClientBusinessRules/NidaAssessFurther").Include (
                "~/Scripts/ClientBusinessRules-NidaAssessFurther.js"));
            bundles.Add(new ScriptBundle("~/ClientBusinessRules/GainShortScreener").Include(
                "~/Scripts/ClientBusinessRules-GainShortScreener.js"));
            bundles.Add(new ScriptBundle("~/ClientBusinessRules/PediatricSymptonChecklist").Include(
                "~/Scripts/ClientBusinessRules-PediatricSymptonChecklist.js"));
            bundles.Add(new ScriptBundle("~/ClientBusinessRules/NihHealthBehaviorsAssessment").Include(
                "~/Scripts/ClientBusinessRules-NihHealthBehaviorsAssessment.js"));

            bundles.Add ( new ScriptBundle ( "~/ClientBusinessRules/GpraInterview" ).Include (
                "~/Scripts/Gpra/ClientBusinessRules-InterviewInformationSection.js",
                "~/Scripts/Gpra/ClientBusinessRules-CriminalJusticeSection.js",
                "~/Scripts/Gpra/ClientBusinessRules-DrugAlcoholUseSection.js",
                "~/Scripts/Gpra/ClientBusinessRules-PlannedServicesSection.js",
                "~/Scripts/Gpra/ClientBusinessRules-DemographicsSection.js",
                "~/Scripts/Gpra/ClientBusinessRules-FamilyLivingSituationSection.js",
                "~/Scripts/Gpra/ClientBusinessRules-ProfessionalInformationSection.js",
                "~/Scripts/Gpra/ClientBusinessRules-TreatmentRecoverySection.js",
                "~/Scripts/Gpra/ClientBusinessRules-SocialConnectednessSection.js",
                "~/Scripts/Gpra/ClientBusinessRules-FollowUpSection.js",
                "~/Scripts/Gpra/ClientBusinessRules-DischargeSection.js" ) );

            bundles.Add ( new ScriptBundle ( "~/bundles/organization" ).Include (
                "~/Views/Staff/Edit.js",
                "~/Views/Team/Edit.js",
                "~/Views/Role/Edit.js",
                "~/Views/Organization/ActivateAssessments.js" ) );

            bundles.Add ( new ScriptBundle ( "~/bundles/organizationedit" ).Include ( "~/Views/Organization/Edit.js" ) );

            bundles.Add ( new ScriptBundle ("~/bundles/assessmentReminder").Include("~/Views/Widgets/AssessmentReminderPartial.js" ) );

            bundles.Add(new ScriptBundle("~/bundles/reportsCenter").Include("~/Views/ReportsCenter/*.js",
                                                                            "~/Views/Widgets/ReportBuilder.js"));

            bundles.Add ( new ScriptBundle ( "~/bundles/patient" ).Include (
                                                                    "~/Views/Widgets/AssessmentReminderPartial.js",
                                                                    "~/Views/Widgets/PatientRecentAssessments.js",
                                                                    "~/Views/Widgets/PatientFeed.js",
                                                                    "~/Views/Widgets/PatientReports.js",
                                                                    "~/Views/Widgets/ReportBuilder.js",
                                                                    "~/Views/Patient/Edit.js"));

            bundles.Add(new ScriptBundle("~/bundles/patientPortal").Include("~/Views/Portal/patient-portal-alerts.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/miscellaneous").Include("~/Scripts/SessionManager.js"));

            bundles.Add ( new StyleBundle ( "~/Content/procenter" ).Include ( "~/Content/less/procenter-style.css" ) );

            bundles.Add ( new StyleBundle ( "~/Content/themes/base/css" )
                .Include ( "~/Content/themes/base/jquery.ui.core.css",
                                     "~/Content/themes/base/jquery.ui.resizable.css",
                                     "~/Content/themes/base/jquery.ui.selectable.css",
                                     "~/Content/themes/base/jquery.ui.accordion.css",
                                     "~/Content/themes/base/jquery.ui.autocomplete.css",
                                     "~/Content/themes/base/jquery.ui.button.css",
                                     "~/Content/themes/base/jquery.ui.dialog.css",
                                     "~/Content/themes/base/jquery.ui.slider.css",
                                     "~/Content/themes/base/jquery.ui.tabs.css",
                                     "~/Content/themes/base/jquery.ui.datepicker.css",
                                     "~/Content/themes/base/jquery.ui.progressbar.css",
                    "~/Content/themes/base/jquery.ui.theme.css",
                    "~/Content/css/jquery.procenter-joyride.css")
                .IncludeDirectory ( "~/Content/modules/DataTables-1.9.4/media/css", "jquery.dataTables*", true )
                .IncludeDirectory ( "~/Content/modules/calendar", "*.css", true )
                            );
        }

        #endregion
    }
}