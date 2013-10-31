namespace ProCenter.Mvc.Infrastructure.Permission
{
    using Pillar.Security.AccessControl;

    public class AssessmentPermission
    {
        #region Static Fields

        /// <summary>
        ///     The assessment edit permission
        /// </summary>
        public static Permission AssessmentEditPermission = new Permission {Name = "assessmentmodule/assessmentedit"};

        /// <summary>
        ///     The assessment view permission
        /// </summary>
        public static Permission AssessmentViewPermission = new Permission { Name = "assessmentmodule/assessmentview" };

        /// <summary>
        ///     The assessment edit permission
        /// </summary>
        public static Permission AssessmentReminderEditPermission = new Permission { Name = "assessmentmodule/assessmentreminderedit" };

        /// <summary>
        ///     The assessment view permission
        /// </summary>
        public static Permission AssessmentReminderViewPermission = new Permission { Name = "assessmentmodule/assessmentreminderview" };

        /// <summary>
        ///     The report edit permission
        /// </summary>
        public static Permission ReportEditPermission = new Permission { Name = "assessmentmodule/reportedit" };

        /// <summary>
        ///     The report view permission
        /// </summary>
        public static Permission ReportViewPermission = new Permission { Name = "assessmentmodule/reportview" };

        #endregion
    }
}