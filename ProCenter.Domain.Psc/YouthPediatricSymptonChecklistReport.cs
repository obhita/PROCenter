using DevExpress.XtraReports.UI;

namespace ProCenter.Domain.Psc
{
    using ProCenter.Domain.CommonModule;

    /// <summary>
    /// Class for the Youth Pediatric Sympton Checklist Report.
    /// </summary>
    public sealed partial class YouthPediatricSymptonChecklistReport : XtraReport, IReport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YouthPediatricSymptonChecklistReport"/> class.
        /// </summary>
        public YouthPediatricSymptonChecklistReport()
        {
            InitializeComponent();
        }
    }
}
