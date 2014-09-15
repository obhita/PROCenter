using DevExpress.XtraReports.UI;

namespace ProCenter.Domain.Psc
{
    using System.Drawing.Printing;

    using ProCenter.Domain.CommonModule;

    /// <summary>
    /// Class for the Pediatric Sympton Checklist Report.
    /// </summary>
    public sealed partial class PediatricSymptonChecklistReport : XtraReport, IReport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PediatricSymptonChecklistReport"/> class.
        /// </summary>
        public PediatricSymptonChecklistReport()
        {
            InitializeComponent();
            BeforePrint += OnBeforePrint;
        }

        private void OnBeforePrint ( object sender, PrintEventArgs printEventArgs )
        {
            var collection = DataSource as PediatricSymptonChecklistReportDataCollection;
            if ( collection == null )
            {
                return;
            }
            var source = collection[0] as PediatricSymptonChecklistReportData;
            var yesNo = "No";
            if ( source != null && source.PediatricSymptonChecklist.DoesYourChildHaveAnyEmotionalOrBehavioralProblemsTheyNeedHelp )
            {
                yesNo = "Yes";
            }
            xrlLabelDoesYourChildHaveAnyEmotionalProblems.Text = yesNo;
            yesNo = "No";
            if ( source != null && source.PediatricSymptonChecklist.AreThereAnyServicesThatYouWouldLikeYourChildToReceiveForTheseProblems )
            {
                yesNo = "Yes";
            }
            xrLabelAnyServices.Text = yesNo;
        }
    }
}
