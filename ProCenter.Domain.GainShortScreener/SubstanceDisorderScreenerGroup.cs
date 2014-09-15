namespace ProCenter.Domain.GainShortScreener
{
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Attributes;

    /// <summary>
    /// SubstanceDisorderScreenerGroup class.
    /// </summary>
    public class SubstanceDisorderScreenerGroup : Group
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SubstanceDisorderScreenerGroup"/> class.
        /// </summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        public SubstanceDisorderScreenerGroup(AssessmentInstance assessmentInstance)
            : base ( assessmentInstance )
        {
        }
        #endregion

        /// <summary>
        /// Gets or sets the used alcohol or other drugs weekly.
        /// </summary>
        /// <value>
        /// The used alcohol or other drugs weekly.
        /// </value>
        [Code("6125019")]
        [DisplayOrder(0)]
        [IsRequired]
        public LastTimeFrequency UsedAlcoholOrOtherDrugsWeekly { get; protected set; }

        /// <summary>
        /// Gets or sets the spent time getting alcohol or other drugs.
        /// </summary>
        /// <value>
        /// The spent time getting alcohol or other drugs.
        /// </value>
        [Code("6125020")]
        [DisplayOrder(1)]
        [IsRequired]
        public LastTimeFrequency SpentTimeGettingAlcoholOrOtherDrugs { get; protected set; }

        /// <summary>
        /// Gets or sets the kept using alcohol or drugs causing social problems.
        /// </summary>
        /// <value>
        /// The kept using alcohol or drugs causing social problems.
        /// </value>
        [Code("6125021")]
        [DisplayOrder(2)]
        [IsRequired]
        public LastTimeFrequency KeptUsingAlcoholOrDrugsCausingSocialProblems { get; protected set; }

        /// <summary>
        /// Gets or sets your use of alcohol or drugs cause you to give up.
        /// </summary>
        /// <value>
        /// Your use of alcohol or drugs cause you to give up.
        /// </value>
        [Code("6125022")]
        [DisplayOrder(3)]
        [IsRequired]
        public LastTimeFrequency YourUseOfAlcoholOrDrugsCauseYouToGiveUp { get; protected set; }

        /// <summary>
        /// Gets or sets you had withdrawal problems from alcohol or other drugs.
        /// </summary>
        /// <value>
        /// You had withdrawal problems from alcohol or other drugs.
        /// </value>
        [Code("6125023")]
        [DisplayOrder(4)]
        [IsRequired]
        public LastTimeFrequency YouHadWithdrawalProblemsFromAlcoholOrOtherDrugs { get; protected set; }
    }
}