namespace ProCenter.Domain.Nih
{
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Attributes;

    /// <summary>
    /// The ProblemGroup class.
    /// </summary>
    public class ProblemGroup : Group 
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProblemGroup"/> class.
        /// </summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        public ProblemGroup(AssessmentInstance assessmentInstance)
            : base ( assessmentInstance )
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the feeling nervous anxious.
        /// </summary>
        /// <value>
        /// The feeling nervous anxious.
        /// </value>
        [Code("7125013")]
        [DisplayOrder(0)]
        [IsRequired]
        public DaysFrequency FeelingNervousAnxious { get; protected set; }

        /// <summary>
        /// Gets or sets the not being able to stop worrying.
        /// </summary>
        /// <value>
        /// The not being able to stop worrying.
        /// </value>
        [Code("7125014")]
        [DisplayOrder(1)]
        [IsRequired]
        public DaysFrequency NotBeingAbleToStopWorrying { get; protected set; }

        /// <summary>
        /// Gets or sets the feeling down depressed or hopeless.
        /// </summary>
        /// <value>
        /// The feeling down depressed or hopeless.
        /// </value>
        [Code("7125015")]
        [DisplayOrder(2)]
        [IsRequired]
        public DaysFrequency FeelingDownDepressedOrHopeless { get; protected set; }

        /// <summary>
        /// Gets or sets the little interest or pleasure in doing things.
        /// </summary>
        /// <value>
        /// The little interest or pleasure in doing things.
        /// </value>
        [Code("7125016")]
        [DisplayOrder(3)]
        [IsRequired]
        public DaysFrequency LittleInterestOrPleasureInDoingThings { get; protected set; }

        #endregion
    }
}