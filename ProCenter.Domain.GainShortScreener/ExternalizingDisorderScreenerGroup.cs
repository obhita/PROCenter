namespace ProCenter.Domain.GainShortScreener
{
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Attributes;

    /// <summary>
    /// ExternalizingDisorderScreenerGroup class.
    /// </summary>
    public class ExternalizingDisorderScreenerGroup : Group
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalizingDisorderScreenerGroup"/> class.
        /// </summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        public ExternalizingDisorderScreenerGroup(AssessmentInstance assessmentInstance)
            : base ( assessmentInstance )
        {
        }
        #endregion

        /// <summary>
        /// Gets or sets the lied or conned.
        /// </summary>
        /// <value>
        /// The lied or conned.
        /// </value>
        [Code("6125011")]
        [DisplayOrder(0)]
        [IsRequired]
        public LastTimeFrequency LiedOrConned { get; protected set; }

        /// <summary>
        /// Gets or sets the hard time paying attention.
        /// </summary>
        /// <value>
        /// The hard time paying attention.
        /// </value>
        [Code("6125012")]
        [DisplayOrder(1)]
        [IsRequired]
        public LastTimeFrequency HardTimePayingAttention { get; protected set; }

        /// <summary>
        /// Gets or sets the hard time listening.
        /// </summary>
        /// <value>
        /// The hard time listening.
        /// </value>
        [Code("6125013")]
        [DisplayOrder(2)]
        [IsRequired]
        public LastTimeFrequency HardTimeListening { get; protected set; }

        /// <summary>
        /// Gets or sets the hard time waiting for your turn.
        /// </summary>
        /// <value>
        /// The hard time waiting for your turn.
        /// </value>
        [Code("6125014")]
        [DisplayOrder(3)]
        [IsRequired]
        public LastTimeFrequency HardTimeWaitingForYourTurn { get; protected set; }

        /// <summary>
        /// Gets or sets the were a bully or threatended other people.
        /// </summary>
        /// <value>
        /// The were a bully or threatended other people.
        /// </value>
        [Code("6125015")]
        [DisplayOrder(4)]
        [IsRequired]
        public LastTimeFrequency WereABullyOrThreatendedOtherPeople { get; protected set; }

        /// <summary>
        /// Gets or sets the started physical fights.
        /// </summary>
        /// <value>
        /// The started physical fights.
        /// </value>
        [Code("6125016")]
        [DisplayOrder(5)]
        [IsRequired]
        public LastTimeFrequency StartedPhysicalFights { get; protected set; }

        /// <summary>
        /// Gets or sets the tried to win back gambling losses.
        /// </summary>
        /// <value>
        /// The tried to win back gambling losses.
        /// </value>
        [Code("6125017")]
        [DisplayOrder(6)]
        [IsRequired]
        public LastTimeFrequency TriedToWinBackGamblingLosses { get; protected set; }
    }
}