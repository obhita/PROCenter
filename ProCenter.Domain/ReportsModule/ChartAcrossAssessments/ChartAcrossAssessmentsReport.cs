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

using System.Collections.Generic;
using System.Globalization;
using DevExpress.XtraCharts;
using ProCenter.Domain.ReportsModule.PatientsWithSpecificResponseReport;

namespace ProCenter.Domain.ReportsModule.ChartAcrossAssessments
{
    #region Using Statements

    using System;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Linq;
    using DevExpress.XtraReports.UI;
    using CommonModule;

    #endregion

    /// <summary>
    ///     The ChartAcrossAssessmentsReport class.
    /// </summary>
    public partial class ChartAcrossAssessmentsReport : XtraReport, IReport
    {
        private TimePeriod _timePeriod;

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ChartAcrossAssessmentsReport" /> class.
        /// </summary>
        public ChartAcrossAssessmentsReport()
        {
            InitializeComponent();
            BeforePrint += OnBeforePrint;
        }

        private enum TimePeriod
        {
            Days,
            Months,
            Years
        }

        #endregion

        #region Methods

        private CustomAxisLabel GetCustomAxisLabel(string value, string name)
        {
            return new CustomAxisLabel {AxisValue = value, Name = name};
        }

        private void SetXAxis(XYDiagram xyDiagram, DateTime? startDate, DateTime? endDate)
        {
            var days = Math.Abs(endDate.GetValueOrDefault().Date.Subtract(startDate.GetValueOrDefault().Date).Days);
            switch (_timePeriod)
            {
                case TimePeriod.Years:
                case TimePeriod.Months:
                    var maxCount = (endDate.GetValueOrDefault().Month - startDate.GetValueOrDefault().Month) + 
                        (12 * (endDate.GetValueOrDefault().Year - startDate.GetValueOrDefault().Year));
                    SetXAxisMonths(xyDiagram, startDate.GetValueOrDefault(), maxCount);
                    break;
                case TimePeriod.Days:
                    SetXAxisOneMonth(xyDiagram, days, startDate.GetValueOrDefault());
                    break;
            }
        }

        private void SetTimePeriod(DateTime? startDate, DateTime? endDate)
        {
            var days = Math.Abs(endDate.GetValueOrDefault().Date.Subtract(startDate.GetValueOrDefault().Date).Days);
            var months = Math.Abs(endDate.GetValueOrDefault().Month - startDate.GetValueOrDefault().Month) + 
                         (12 * (endDate.GetValueOrDefault().Year - startDate.GetValueOrDefault().Year));
            var years = Math.Abs(endDate.GetValueOrDefault().Year - startDate.GetValueOrDefault().Year);

            if (days <= 31 && months <= 2)
            {
                _timePeriod = TimePeriod.Days;
            }
            else if (years >= 3)
            {
                _timePeriod = TimePeriod.Years;
            }
            else if (years >= 2)
            {
                _timePeriod = TimePeriod.Years;
            }
            else if (years >= 1)
            {
                _timePeriod = TimePeriod.Months;
            }
            else if (months >= 6)
            {
                _timePeriod = TimePeriod.Months;
            }
            else if (months >= 3)
            {
                _timePeriod = TimePeriod.Months;
            }
        }

        private void SetXAxisOneMonth(XYDiagram xyDiagram, int days, DateTime currDate)
        {
            var val = 1;
            for (var x = 0; x <= days; x++)
            {
                if (x%5 == 0 || x == 0)
                {
                    xyDiagram.AxisX.CustomLabels.Add(GetCustomAxisLabel(val.ToString(),
                        currDate.Month + "/" + currDate.Day));
                }
                val += 1;
                currDate = currDate.AddDays(1);
            }
            xyDiagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Day;
            xyDiagram.AxisX.Label.DateTimeOptions.Format = DateTimeFormat.Custom;
            xyDiagram.AxisX.Label.DateTimeOptions.FormatString = "MMM-yyyy";
            xyDiagram.AxisX.Label.NumericOptions.Format = NumericFormat.FixedPoint;
            xyDiagram.AxisX.Label.NumericOptions.Precision = 1;
            xyDiagram.AxisX.Range.AlwaysShowZeroLevel = true;
            xyDiagram.AxisX.Range.Auto = false;
            xyDiagram.AxisX.Range.SideMarginsEnabled = true;
            xyDiagram.AxisX.MinorCount = 4;
            xyDiagram.AxisX.Tickmarks.MinorLength = 3;
            xyDiagram.AxisX.Tickmarks.Thickness = 1;
            xyDiagram.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram.AxisX.Tickmarks.Length = 5;
            xyDiagram.AxisX.Tickmarks.Thickness = 2;
        }

        private void SetXAxisMonths(XYDiagram xyDiagram, DateTime currDate, int numberOfMonths)
        {
            var startMonth = currDate.Month;
            var endMonth = startMonth + numberOfMonths + 1;

            for (var x = startMonth; x < endMonth; x++)
            {
                if (_timePeriod == TimePeriod.Years)
                {
                    xyDiagram.AxisX.CustomLabels.Add(GetCustomAxisLabel(currDate.ToString("yyyy"), currDate.Year.ToString()));
                }
                else
                {
                    xyDiagram.AxisX.CustomLabels.Add(GetCustomAxisLabel(currDate.ToString("MMM-yyyy"),
                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currDate.Month).Substring(0, 3) + "-" +
                    currDate.Year));
                }
                currDate = currDate.AddMonths(1);
            }
            xyDiagram.AxisX.DateTimeScaleOptions.GridAlignment = _timePeriod == TimePeriod.Years ? DateTimeGridAlignment.Year : DateTimeGridAlignment.Month;
            xyDiagram.AxisX.Label.DateTimeOptions.Format = DateTimeFormat.Custom;
            xyDiagram.AxisX.Label.DateTimeOptions.FormatString = "MMM-yyyy";
            xyDiagram.AxisX.Label.NumericOptions.Format = NumericFormat.FixedPoint;
            xyDiagram.AxisX.Label.NumericOptions.Precision = 1;
            xyDiagram.AxisX.Range.AlwaysShowZeroLevel = true;
            xyDiagram.AxisX.Range.Auto = false;
            xyDiagram.AxisX.Range.SideMarginsEnabled = true;
            xyDiagram.AxisX.MinorCount = 1;
            xyDiagram.AxisX.Tickmarks.MinorLength = 3;
            xyDiagram.AxisX.Tickmarks.Thickness = 1;
            xyDiagram.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram.AxisX.Tickmarks.Length = 5;
            xyDiagram.AxisX.Tickmarks.Thickness = 2;
        }

        private void SetYaxis(XYDiagram xyDiagram, IEnumerable<KeyValuePair<string, string>> values)
        {
            foreach (var val in values)
            {
                xyDiagram.AxisY.CustomLabels.Add(GetCustomAxisLabel(val.Key, val.Value));
            }
            xyDiagram.AxisY.MinorCount = 5;
            xyDiagram.AxisY.Range.AlwaysShowZeroLevel = true;
            xyDiagram.AxisY.Range.SideMarginsEnabled = true;
            xyDiagram.AxisY.Tickmarks.MinorLength = 1;
            xyDiagram.AxisY.Tickmarks.MinorVisible = false;
            xyDiagram.AxisY.Tickmarks.Thickness = 2;
            xyDiagram.AxisY.VisibleInPanesSerializable = "-1";
        }

        private IEnumerable<Series> GetSeries(IEnumerable<BubbleValue> values)
        {
            var sList = new List<Series>();
            var seriesName = string.Empty;
            Series s = null;
            BubbleSeriesView bubbleSeriesViewAlways = null;
            foreach (var val in values)
            {
                if (seriesName == string.Empty || seriesName != val.SeriesName)
                {
                    if (seriesName != string.Empty)
                    {
                        sList.Add(s);
                    }
                    seriesName = val.SeriesName;
                    s = new Series("Data Series " + val.SeriesName, ViewType.Bubble)
                    {
                        ArgumentScaleType = ScaleType.Auto,
                        ValueScaleType = ScaleType.Numerical,
                        LabelsVisibility = DevExpress.Utils.DefaultBoolean.True,
                        Name = val.SeriesName
                    };
                    bubbleSeriesViewAlways = new BubbleSeriesView();
                }
                s.View = bubbleSeriesViewAlways;
                s.Points.Add(new SeriesPoint(val.Xvalue, GetYvalue(val), val.Count));
            }
            if (seriesName != string.Empty)
            {
                sList.Add(s);
            }
            return sList;
        }

        private string GetYvalue(BubbleValue val)
        {
            return string.IsNullOrEmpty(val.Yvalue) ? "0" : val.Yvalue;
        }

        private XRChart GetChart(
            Point location,
            IEnumerable<KeyValuePair<string, string>> axisValues,
            List<BubbleValue> dataValues,
            DateTime? startDate,
            DateTime? endDate)
        {
            var chart = new XRChart {Location = location};
            var series = GetSeries(dataValues).ToList();

            foreach (var s in series)
            {
                chart.Series.Add(s);
                ((BubbleSeriesView)s.View).MinSize = .8;
                ((BubbleSeriesView)s.View).MaxSize = 1.5;
                ((BubbleSeriesView) s.View).Transparency = 200;
            }
            SetXAxis(((XYDiagram) chart.Diagram), startDate, endDate);
            SetYaxis(((XYDiagram) chart.Diagram), axisValues);
            ((XYDiagram) chart.Diagram).EnableAxisXZooming = true;
            chart.Size = new Size(900, 300);
            chart.LocationF = location;

            SetMinMaxBubbleSize(series, dataValues);
 
            return chart;
        }

        private List<double> GetHighLowValueForSeries(List<BubbleValue> dataValues, string seriesName)
        {
            var highVal= dataValues.Where(a => a.SeriesName == seriesName).Select(v => double.Parse(v.Count.ToString())).Concat(new double[] {0}).Max();
            var lowVal = -1;
            foreach (var val in dataValues.Where(a => a.SeriesName == seriesName))
            {
                if (lowVal == -1 || val.Count < lowVal)
                {
                    lowVal = val.Count;
                }
            }
            var returnVals = new List<double> { lowVal, highVal };
            return returnVals;
        }

        private List<double> GetHighLowValuesForAllSeries(List<BubbleValue> dataValues)
        {
            double highVal = 0;
            double lowVal = -1;
            string seriesName = string.Empty;
            foreach (var s in dataValues.OrderBy(a => a.SeriesName))
            {
                if (seriesName == string.Empty || seriesName != s.SeriesName)
                {
                    var vals = GetHighLowValueForSeries(dataValues, s.SeriesName);
                    var currLowVal = vals.ElementAt(0);
                    var currHighVal = vals.ElementAt(1);
                    if (currHighVal > highVal)
                    {
                        highVal = currHighVal;
                    }
                    if (currLowVal < lowVal || lowVal.Equals(-1))
                    {
                        lowVal = currLowVal;
                    }
                    seriesName = s.SeriesName;
                }
            }
            var returnVals = new List<double> { lowVal, highVal };
            return returnVals;
        }

        private void SetMinMaxBubbleSize(IEnumerable<Series> series, List<BubbleValue> dataValues)
        {
            var highLowVals = GetHighLowValuesForAllSeries(dataValues);
            var highLowRange = highLowVals.ElementAt(1) - highLowVals.ElementAt(0);
            if (highLowRange.Equals(0) || (highLowVals.ElementAt(0).Equals(0) && highLowVals.ElementAt(1).Equals(0)))
            {
                highLowRange = 12;
            }
            var minSize = .5 * (highLowVals.ElementAt(0) / highLowRange);
            var maxSize = .75 * (highLowVals.ElementAt(1) / highLowRange);
            if (minSize < 0)
            {
                minSize = .1;
            }
            if (maxSize < minSize)
            {
                maxSize = minSize * 4;
            }
            foreach (var s in series)
            {
                var seriesLowHigh = GetHighLowValueForSeries(dataValues, s.Name);
                var seriesMin = (seriesLowHigh.ElementAt(0) / highLowVals.ElementAt(0)) * minSize;
                var seriesMax = (seriesLowHigh.ElementAt(1) / highLowVals.ElementAt(1)) * maxSize;
                if (seriesMin < 0)
                {
                    seriesMin = .1;
                }
                if (seriesMax < seriesMin)
                {
                    seriesMax = seriesMin * 2;
                }

                ((BubbleSeriesView)s.View).MaxSize = 200;
                ((BubbleSeriesView)s.View).MinSize = 50;

                ((BubbleSeriesView)s.View).MinSize = seriesMin;
                ((BubbleSeriesView)s.View).MaxSize = seriesMax;
            }
        }

        private List<KeyValuePair<string, string>> GetKeyValuePairsForQuestion(
            IEnumerable<PatientsWithSpecificResponseDataObject> dataPoints)
        {
            var kvReturn = new List<KeyValuePair<string, string>>();
            var question = string.Empty;

            foreach (var q in dataPoints.OrderBy(a => a.Response))
            {
                if (question != string.Empty && question == q.Response)
                {
                    continue;
                }
                if (q.IsCode)
                {
                    kvReturn.Add(new KeyValuePair<string, string>(q.CodeValue.GetValueOrDefault().ToString(), q.Response));
                    question = q.Response;
                }
                else
                {
                    kvReturn.Add(new KeyValuePair<string, string>(q.Response, q.Response));
                    question = q.Response;
                }
            }
            return kvReturn;
        }

        private IEnumerable<BubbleValue> GetBubbleValues(
            List<PatientsWithSpecificResponseDataObject> dataPoints,
            IEnumerable<KeyValuePair<string, string>> questions,
            DateTime? startDate,
            DateTime? endDate)
        {
            var maxCount = 0;
            switch (_timePeriod)
            {
                case TimePeriod.Years:
                    maxCount = Math.Abs(endDate.GetValueOrDefault().Year - startDate.GetValueOrDefault().Year);
                    break;
                case TimePeriod.Months:
                    maxCount =  (endDate.GetValueOrDefault().Month - startDate.GetValueOrDefault().Month) + 
                                (12 * (endDate.GetValueOrDefault().Year - startDate.GetValueOrDefault().Year));
                    break;
                case TimePeriod.Days:
                    maxCount =
                        Math.Abs(startDate.GetValueOrDefault().Date.Subtract(endDate.GetValueOrDefault().Date).Days) + 1;
                    break;
            }

            var bvReturn = new List<BubbleValue>();
            foreach (var question in questions)
            {
                var currDate = startDate.GetValueOrDefault();
                for (var x = 0; x <= maxCount; x++)
                {
                    var count = GetCountForInterval(_timePeriod, currDate, dataPoints, question);
                    if (count > 0)
                    {
                        var codeValue = string.Empty;
                        var patientsWithSpecificResponseDataObject =
                            GetResponseValue(_timePeriod, currDate, dataPoints, question);
                        if (patientsWithSpecificResponseDataObject != null)
                        {
                            codeValue = patientsWithSpecificResponseDataObject.IsCode ? 
                                patientsWithSpecificResponseDataObject.CodeValue.ToString() : 
                                patientsWithSpecificResponseDataObject.Response;
                        }
                        bvReturn.Add(new BubbleValue
                        {
                            Count = count,
                            SeriesName = question.Value,
                            Xvalue = GetXValue(currDate),
                            Yvalue = codeValue
                        });
                    }
                    switch (_timePeriod)
                    {
                        case TimePeriod.Years:
                            currDate = currDate.AddYears(1);
                            break;
                        case TimePeriod.Months:
                            currDate = currDate.AddMonths(1);
                            break;
                        case TimePeriod.Days:
                            currDate = currDate.AddDays(1);
                            break;
                    }
                }
            }
            return bvReturn;
        }

        private string GetXValue(DateTime currDate)
        {
            var returnValue = string.Empty;
            switch (_timePeriod)
            {
                case TimePeriod.Years:
                    returnValue = currDate.Year.ToString();
                    break;
                case TimePeriod.Months:
                    returnValue = currDate.ToString("MMM-yyyy");
                    break;
                case TimePeriod.Days:
                    returnValue = currDate.Day.ToString();
                    break;
            }
            return returnValue;
        }

        private PatientsWithSpecificResponseDataObject GetResponseValue(
            TimePeriod timePeriod,
            DateTime currDate,
            IEnumerable<PatientsWithSpecificResponseDataObject> dataPoints,
            KeyValuePair<string, string> question)
        {
            PatientsWithSpecificResponseDataObject patientsWithSpecificResponseDataObject = null;
            switch (timePeriod)
            {
                case TimePeriod.Days:
                    patientsWithSpecificResponseDataObject = dataPoints.FirstOrDefault(
                        a =>
                            a.Response == question.Value &&
                            DateTime.Parse(a.AssessmentDate).ToShortDateString() == currDate.ToShortDateString());
                    break;
                case TimePeriod.Months:
                    patientsWithSpecificResponseDataObject = dataPoints.FirstOrDefault(
                        a =>
                            a.Response == question.Value &&
                            DateTime.Parse(a.AssessmentDate).Month == currDate.Month &&
                            DateTime.Parse(a.AssessmentDate).Year == currDate.Year);
                    break;
                case TimePeriod.Years:
                    patientsWithSpecificResponseDataObject = dataPoints.FirstOrDefault(
                        a =>
                            a.Response == question.Value &&
                            DateTime.Parse(a.AssessmentDate).Year == currDate.Year);
                    break;
            }
            return patientsWithSpecificResponseDataObject;
        }

        private int GetCountForInterval(
            TimePeriod timePeriod,
            DateTime currDate,
            IEnumerable<PatientsWithSpecificResponseDataObject> dataPoints,
            KeyValuePair<string, string> question)
        {
            var count = 0;
            if (timePeriod == TimePeriod.Days)
            {
                count = dataPoints.Count(
                    a =>
                        a.Response == question.Value &&
                        DateTime.Parse(a.AssessmentDate).ToShortDateString() == currDate.ToShortDateString());
            }
            else if (timePeriod == TimePeriod.Months)
            {
                count = dataPoints.Count(
                    a =>
                        a.Response == question.Value &&
                        DateTime.Parse(a.AssessmentDate).Month == currDate.Month &&
                        DateTime.Parse(a.AssessmentDate).Year == currDate.Year);
            }
            else if (timePeriod == TimePeriod.Years)
            {
                count = dataPoints.Count(
                    a =>
                        a.Response == question.Value &&
                        DateTime.Parse(a.AssessmentDate).Year == currDate.Year);
            }
            return count;
        }

        private void BindData()
        {
            var source =
                (DataSource as PatientsWithSpecificResponseDataCollection)[0] as PatientsWithSpecificResponseData;
            if (source == null)
            {
                return;
            }
            var itemDefinitionCode = string.Empty;
            var y = 0;

            SetTimePeriod(source.StartDate, source.EndDate);
            foreach (var dataPoint in source.Data)
            {
                if (itemDefinitionCode == string.Empty || itemDefinitionCode != dataPoint.ItemDefinitionCode)
                {
                    var itemDef = dataPoint.ItemDefinitionCode;
                    var questions =
                        source.Data.Where(a => a.ItemDefinitionCode == itemDef).ToList();
                    var kvData = GetKeyValuePairsForQuestion(questions);
                    var bubbleData = GetBubbleValues(questions, kvData, source.StartDate, source.EndDate);
                    itemDefinitionCode = questions.ElementAt(0).ItemDefinitionCode;
                    var chart = GetChart(new Point(0, y), kvData, bubbleData.ToList(), source.StartDate, source.EndDate);
                    var chartTitle = new ChartTitle
                    {
                        Text = GetTitle(questions.ElementAt(0).AssessmentName, questions.ElementAt(0).Question),
                        WordWrap = true,
                        Alignment = StringAlignment.Near
                    };
                    chart.Titles.Add(chartTitle);
                    Bands[BandKind.Detail].Controls.Add(chart);
                    y += 325;
                }
            }
            Bands[BandKind.Detail].CanGrow = true;
        }

        private string GetTitle(string assessmentName, string question)
        {
            return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
                "<color=#8A8A8A>" + PatientsWithSpecificResponseAcrossAssessments.HeaderAssessmentName + ": </color>",
                "<b>",
                assessmentName.ToUpper(),
                "</b>",
                Environment.NewLine,
                "<size=12><color=#8A8A8A>" + PatientsWithSpecificResponseAcrossAssessments.HeaderQuestion + "</color></size>",
                Environment.NewLine,
                "<b><size=12>",
                question,
                "</size></b>");
        }

        private void OnBeforePrint(object sender, PrintEventArgs printEventArgs)
        {
            BindData();
        }

        #endregion
    }
}