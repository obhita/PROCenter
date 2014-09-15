namespace ProCenter.Domain.Nih
{
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Attributes;

    /// <summary>
    /// The ExerciseGroup class.
    /// </summary>
    public class ExerciseGroup : Group 
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseGroup"/> class.
        /// </summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        public ExerciseGroup(AssessmentInstance assessmentInstance)
            : base ( assessmentInstance )
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the how many days moderate to strenuous excercise.
        /// </summary>
        /// <value>
        /// The how many days moderate to strenuous excercise.
        /// </value>
        [Code("7125009")]
        [DisplayOrder(0)]
        [IsRequired]
        public int HowManyDaysModerateToStrenuousExcercise { get; protected set; }

        /// <summary>
        /// Gets or sets the how many minutes on average do you excercise.
        /// </summary>
        /// <value>
        /// The how many minutes on average do you excercise.
        /// </value>
        [Code("7125010")]
        [DisplayOrder(1)]
        [IsRequired]
        public int HowManyMinutesOnAverageDoYouExcercise { get; protected set; }

        #endregion
    }
}