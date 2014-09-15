namespace ProCenter.Domain.GainShortScreener
{
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Attributes;

    /// <summary>
    /// InternalizingDisorderScreenerGroup class.
    /// </summary>
    public class InternalizingDisorderScreenerGroup : Group 
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalizingDisorderScreenerGroup"/> class.
        /// </summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        public InternalizingDisorderScreenerGroup(AssessmentInstance assessmentInstance)
            : base ( assessmentInstance )
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the feeling very trapped lonely sad blue depressed hopeless about future.
        /// </summary>
        /// <value>
        /// The feeling very trapped lonely sad blue depressed hopeless about future.
        /// </value>
        [Code("6125003")]
        [DisplayOrder (0)]
        [IsRequired]
        public LastTimeFrequency FeelingVeryTrappedLonelySadBlueDepressedHopelessAboutFuture { get; protected set; }

        /// <summary>
        /// Gets or sets the sleep trouble.
        /// </summary>
        /// <value>
        /// The sleep trouble.
        /// </value>
        [Code("6125005")]
        [DisplayOrder(1)]
        [IsRequired]
        public LastTimeFrequency SleepTrouble { get; protected set; }

        /// <summary>
        /// Gets or sets the feeling very anxious nervous tense scared panicked.
        /// </summary>
        /// <value>
        /// The feeling very anxious nervous tense scared panicked.
        /// </value>
        [Code("6125006")]
        [DisplayOrder(2)]
        [IsRequired]
        public LastTimeFrequency FeelingVeryAnxiousNervousTenseScaredPanicked { get; protected set; }

        /// <summary>
        /// Gets or sets the becoming very distressted and upset about past.
        /// </summary>
        /// <value>
        /// The becoming very distressted and upset about past.
        /// </value>
        [Code("6125007")]
        [DisplayOrder(3)]
        [IsRequired]
        public LastTimeFrequency BecomingVeryDistresstedAndUpsetAboutPast { get; protected set; }

        /// <summary>
        /// Gets or sets the thinking about ending your life.
        /// </summary>
        /// <value>
        /// The thinking about ending your life.
        /// </value>
        [Code("6125008")]
        [DisplayOrder(4)]
        [IsRequired]
        public LastTimeFrequency ThinkingAboutEndingYourLife { get; protected set; }

        /// <summary>
        /// Gets or sets the seeing or hearing things no one else could see or hear.
        /// </summary>
        /// <value>
        /// The seeing or hearing things no one else could see or hear.
        /// </value>
        [Code("6125009")]
        [DisplayOrder(5)]
        [IsRequired]
        public LastTimeFrequency SeeingOrHearingThingsNoOneElseCouldSeeOrHear { get; protected set; }

        #endregion
    }
}