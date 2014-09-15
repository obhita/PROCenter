using System.Drawing.Printing;
using DevExpress.XtraReports.UI;

namespace ProCenter.Domain.Nih
{
    using ProCenter.Domain.CommonModule;

    /// <summary>
    /// NihPatientSummaryReport class.
    /// </summary>
    public partial class NihHealthBehaviorsAssessmentPatientSummaryReport :XtraReport, IReport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NihHealthBehaviorsAssessmentPatientSummaryReport"/> class.
        /// </summary>
        public NihHealthBehaviorsAssessmentPatientSummaryReport()
        {
            InitializeComponent();   
        }
    }
}
