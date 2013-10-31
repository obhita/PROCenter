namespace ProCenter.Domain.Nida
{
    #region

    using System.Linq;
    using CommonModule;
    using DevExpress.XtraReports.UI;

    #endregion

    public partial class NidaReport : XtraReport, IReport
    {
        public NidaReport()
        {
            InitializeComponent();
        }

        private void NidaReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = ((this.DataSource as NidaReportDataCollection)[0] as NidaReportData);
            if (data.FollowUpItems == null || !data.FollowUpItems.Any())
            {
                xrLabel17.Text = null;
                xrLabel17.Visible = false;
            }
            if (data.UseTreatmentHistoryItems == null || !data.UseTreatmentHistoryItems.Any())
            {
                xrLabel18.Text = null;
                xrLabel18.Visible = false;
            }
            if (data.PatientResourceItems == null || !data.PatientResourceItems.Any())
            {
                xrLabel19.Text = null;
                xrLabel19.Visible = false;
            }
            if (data.SummaryItems == null || !data.SummaryItems.Any())
            {
                xrLabel16.Text = null;
                xrLabel16.Visible = false;
            }
        }
    }
}