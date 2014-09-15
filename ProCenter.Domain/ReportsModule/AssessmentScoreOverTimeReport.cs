namespace ProCenter.Domain.ReportsModule
{
    #region Using Statements

    using System.Drawing.Printing;

    using DevExpress.XtraCharts;
    using DevExpress.XtraReports.UI;

    #endregion

    /// <summary>
    /// Assessment Score over time report.
    /// </summary>
    public partial class AssessmentScoreOverTimeReport : XtraReport
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentScoreOverTimeReport"/> class.
        /// </summary>
        public AssessmentScoreOverTimeReport ()
        {
            InitializeComponent ();
            BeforePrint += OnBeforePrint;
        }

        #endregion

        #region Methods

        private void OnBeforePrint ( object sender, PrintEventArgs printEventArgs )
        {
            var source = ( DataSource as AssessmentScoreOverTimeDataCollection )[0] as AssessmentScoreOverTimeData;
            if ( source.Scores != null )
            {
                foreach ( var scoreData in source.Scores )
                {
                    xrChart1.Series[0].Points.Add ( new SeriesPoint ( scoreData.Date, scoreData.Score ) );
                }
            }
        }

        #endregion
    }
}