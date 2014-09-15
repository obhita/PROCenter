namespace ProCenter.Domain.ReportsModule.ChartAcrossAssessments
{
    /// <summary>
    /// The BubbleValue class.
    /// </summary>
    public class BubbleValue
    {
        /// <summary>
        /// Gets or sets the name of the series.
        /// </summary>
        /// <value>
        /// The name of the series.
        /// </value>
        public string SeriesName { get; set; }

        /// <summary>
        /// Gets or sets the x value.
        /// </summary>
        /// <value>
        /// The x value.
        /// </value>
        public string Xvalue { get; set; }

        /// <summary>
        /// Gets or sets the y value.
        /// </summary>
        /// <value>
        /// The y value.
        /// </value>
        public string Yvalue { get; set; }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count { get; set; }
    }
}
