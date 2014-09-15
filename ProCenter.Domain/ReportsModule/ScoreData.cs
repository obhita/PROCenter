namespace ProCenter.Domain.ReportsModule
{
    #region Using Statements

    using System;

    #endregion

    /// <summary>The score data class.</summary>
    public class ScoreData
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreData"/> class.
        /// </summary>
        /// <param name="assessmentScore">The assessment score.</param>
        /// <param name="scoredDate">The scored date.</param>
        public ScoreData ( string assessmentScore, DateTime scoredDate )
        {
            Score = double.Parse ( assessmentScore );
            Date = scoredDate;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public double Score { get; set; }

        #endregion
    }
}