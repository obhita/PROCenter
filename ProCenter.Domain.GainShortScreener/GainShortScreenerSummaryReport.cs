using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace ProCenter.Domain.GainShortScreener
{
    using System.Drawing.Printing;

    using DevExpress.XtraCharts;

    using ProCenter.Domain.CommonModule;

    /// <summary>
    /// The gain short screener summary report class.
    /// </summary>
    public partial class GainShortScreenerSummaryReport : XtraReport, IReport
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GainShortScreenerSummaryReport"/> class.
        /// </summary>
        public GainShortScreenerSummaryReport()
        {
            InitializeComponent();
            BeforePrint += OnBeforePrint;
        }

        private void OnBeforePrint(object sender, PrintEventArgs printEventArgs)
        {
            var source = (DataSource as GainShortScreenerReportDataCollection)[0] as GainShortScreenerReportData;
            xrTotalChart.Series[0].Points.Add(new SeriesPoint("A", source.TotalPastMonth[0]));
            xrTotalChart.Series[1].Points.Add(new SeriesPoint("A", source.TotalTwoToThreeMonths[0]));
            xrTotalChart.Series[2].Points.Add(new SeriesPoint("A", source.TotalFourToTwelveMonths[0]));
            xrTotalChart.Series[3].Points.Add(new SeriesPoint("A", source.TotalMoreThenOneYear[0]));
            xrTotalChart.Series[4].Points.Add(new SeriesPoint("A", source.TotalLifetime[0]));

            xrGroupChart.Series[0].Points.Add(new SeriesPoint("A", source.PastMonth[0]));
            xrGroupChart.Series[0].Points.Add(new SeriesPoint("B", source.PastMonth[1]));
            xrGroupChart.Series[0].Points.Add(new SeriesPoint("C", source.PastMonth[2]));
            xrGroupChart.Series[0].Points.Add(new SeriesPoint("D", source.PastMonth[3]));
            xrGroupChart.Series[1].Points.Add(new SeriesPoint("A", source.TwoToThreeMonths[0]));
            xrGroupChart.Series[1].Points.Add(new SeriesPoint("B", source.TwoToThreeMonths[1]));
            xrGroupChart.Series[1].Points.Add(new SeriesPoint("C", source.TwoToThreeMonths[2]));
            xrGroupChart.Series[1].Points.Add(new SeriesPoint("D", source.TwoToThreeMonths[3]));
            xrGroupChart.Series[2].Points.Add(new SeriesPoint("A", source.FourToTwelveMonths[0]));
            xrGroupChart.Series[2].Points.Add(new SeriesPoint("B", source.FourToTwelveMonths[1]));
            xrGroupChart.Series[2].Points.Add(new SeriesPoint("C", source.FourToTwelveMonths[2]));
            xrGroupChart.Series[2].Points.Add(new SeriesPoint("D", source.FourToTwelveMonths[3]));
            xrGroupChart.Series[3].Points.Add(new SeriesPoint("A", source.MoreThenOneYear[0]));
            xrGroupChart.Series[3].Points.Add(new SeriesPoint("B", source.MoreThenOneYear[1]));
            xrGroupChart.Series[3].Points.Add(new SeriesPoint("C", source.MoreThenOneYear[2]));
            xrGroupChart.Series[3].Points.Add(new SeriesPoint("D", source.MoreThenOneYear[3]));
            xrGroupChart.Series[4].Points.Add(new SeriesPoint("A", source.Lifetime[0]));
            xrGroupChart.Series[4].Points.Add(new SeriesPoint("B", source.Lifetime[1]));
            xrGroupChart.Series[4].Points.Add(new SeriesPoint("C", source.Lifetime[2]));
            xrGroupChart.Series[4].Points.Add(new SeriesPoint("D", source.Lifetime[3]));
        }
    }
}
