var chart;

var chartData = [{
    date: new Date(2012, 0, 1),
    value: 8},
{
    date: new Date(2012, 0, 1),
    value: 8},
{
    date: new Date(2012, 0, 2),
    value: 10},
{
    date: new Date(2012, 0, 3),
    value: 12},
{
    date: new Date(2012, 0, 4),
    value: 14},
{
    date: new Date(2012, 0, 5),
    value: 11},
{
    date: new Date(2012, 0, 6),
    value: 6},
{
    date: new Date(2012, 0, 7),
    value: 7},
{
    date: new Date(2012, 0, 8),
    value: 9},
{
    date: new Date(2012, 0, 9),
    value: 13},
{
    date: new Date(2012, 0, 10),
    value: 15},
{
    date: new Date(2012, 0, 11),
    value: 19},
{
    date: new Date(2012, 0, 12),
    value: 21},
{
    date: new Date(2012, 0, 13),
    value: 22},
{
    date: new Date(2012, 0, 14),
    value: 20},
{
    date: new Date(2012, 0, 15),
    value: 18},
{
    date: new Date(2012, 0, 16),
    value: 14},
{
    date: new Date(2012, 0, 17),
    value: 16},
{
    date: new Date(2012, 0, 18),
    value: 18},
{
    date: new Date(2012, 0, 19),
    value: 17},
{
    date: new Date(2012, 0, 20),
    value: 15},
{
    date: new Date(2012, 0, 21),
    value: 12},
{
    date: new Date(2012, 0, 22),
    value: 10},
{
    date: new Date(2012, 0, 23),
    value: 8}];


AmCharts.ready(function() {
    // SERIAL CHART        
    chart = new AmCharts.AmSerialChart();
    chart.pathToImages = "http://www.amcharts.com/lib/images/";
    chart.dataProvider = chartData;
    chart.marginTop = 0;
    chart.marginRight = 10;
    chart.autoMarginOffser = 0;
    chart.categoryField = "date";


    // AXES
    // category
    var categoryAxis = chart.categoryAxis;
    categoryAxis.parseDates = true; // as our data is date-based, we set parseDates to true
    categoryAxis.minPeriod = "DD"; // our data is daily, so we set minPeriod to DD                 
    categoryAxis.gridAlpha = 0.10;
    categoryAxis.axisAlpha = 0;
    categoryAxis.inside = true;

    // value
    var valueAxis = new AmCharts.ValueAxis();
    valueAxis.tickLength = 0;
    valueAxis.axisAlpha = 0;
    valueAxis.gridAlpha = 0;
    valueAxis.showFirstLabel = false;
    valueAxis.showLastLabel = false;
    chart.addValueAxis(valueAxis);

    // GRAPH
    var graph = new AmCharts.AmGraph();
    graph.dashLength = 3;
    graph.lineColor = "#5475d3";
    graph.valueField = "value";
    graph.dashLength = 3;
    graph.bullet = "round";
    chart.addGraph(graph);

    // CURSOR
    var chartCursor = new AmCharts.ChartCursor();
    chart.addChartCursor(chartCursor);

    // SCROLLBAR
    var chartScrollbar = new AmCharts.ChartScrollbar();
    chart.addChartScrollbar(chartScrollbar);

    // HORIZONTAL Yellow RANGE
    var guide = new AmCharts.Guide();
    guide.value = 10;
    guide.toValue = 20;
    guide.fillColor = "#ffdb12";
    guide.inside = true;
    guide.fillAlpha = 0.5;
    guide.lineAlpha = 0;
    valueAxis.addGuide(guide);

    // TREND LINES
    // first trend line
    var trendLine = new AmCharts.TrendLine();
    trendLine.initialDate = new Date(2012, 0, 2, 12); // 12 is hour - to start trend line in the middle of the day
    trendLine.finalDate = new Date(2012, 0, 11, 12);
    trendLine.initialValue = 10;
    trendLine.finalValue = 19;
    trendLine.lineColor = "#00CCFF";
    chart.addTrendLine(trendLine);

    // second trend line
    trendLine = new AmCharts.TrendLine();
    trendLine.initialDate = new Date(2012, 0, 17, 12);
    trendLine.finalDate = new Date(2012, 0, 22, 12);
    trendLine.initialValue = 16;
    trendLine.finalValue = 10;
    trendLine.lineColor = "#00CCFF";
    chart.addTrendLine(trendLine);

    // WRITE
    chart.write("chartdiv");
});