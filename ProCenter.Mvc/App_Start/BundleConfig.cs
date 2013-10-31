#region Using Statements

#endregion

namespace ProCenter.Mvc.App_Start
{
    #region

    using System.Web.Optimization;

    #endregion

    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap-transition.js",
                "~/Scripts/bootstrap-alert.js",
                "~/Scripts/bootstrap-button.js",
                "~/Scripts/bootstrap-dropdown.js",
                "~/Scripts/bootstrap-modal.js",
                "~/Scripts/bootstrap-tooltip.js",
                "~/Scripts/bootstrap-popover.js",
                "~/Scripts/DataTables-1.9.4/media/js/jquery.dataTables.js",
                "~/Scripts/DataTables-1.9.4/media/js/jquery.dataTables.*",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery.procenter*",
                "~/Scripts/modernizr-*",
                "~/Scripts/fullcalendar.js"//"~/Scripts/gcal.js"
                ));

            bundles.Add(new ScriptBundle("~/ClientBusinessRulesEngine").Include("~/Scripts/ClientBusinessRulesEngine.js"));

            bundles.Add(new ScriptBundle("~/ClientBusinessRules/NidaAssessFurther").Include("~/Scripts/ClientBusinessRules-NidaAssessFurther.js"));

            bundles.Add(new ScriptBundle("~/bundles/organization").Include(
                "~/Views/Staff/Edit.js",
                "~/Views/Team/Edit.js",
                "~/Views/Role/Edit.js",
                "~/Views/Organization/ActivateAssessments.js"));

            bundles.Add(new ScriptBundle("~/bundles/organizationedit").Include("~/Views/Organization/Edit.js"));

            bundles.Add ( new ScriptBundle ( "~/bundles/home" ).Include ("~/Views/Widgets/AssessmentReminderPartial.js"));

            bundles.Add(new ScriptBundle("~/bundles/patient").Include(
                                                                    "~/Views/Widgets/AssessmentReminderPartial.js",
                                                                    "~/Views/Widgets/PatientRecentAssessments.js",
                                                                    "~/Views/Widgets/PatientFeed.js",
                                                                    "~/Views/Widgets/PatientReports.js",
                                                                    "~/Views/Patient/Edit.js"));

            bundles.Add(new StyleBundle("~/Content/procenter").Include("~/Content/less/procenter-style.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css")
                            .Include("~/Content/themes/base/jquery.ui.core.css",
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
                                     "~/Content/themes/base/jquery.ui.theme.css")
                            .IncludeDirectory("~/Content/modules/DataTables-1.9.4/media/css", "*.css", true)
                            .IncludeDirectory("~/Content/modules/calendar", "*.css", true)
                            );
        }
    }
}