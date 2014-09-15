namespace ProCenter.Domain.Nih
{
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Attributes;

    /// <summary>
    /// The DietGroup class.
    /// </summary>
    public class DietGroup : Group 
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DietGroup"/> class.
        /// </summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        public DietGroup(AssessmentInstance assessmentInstance)
            : base ( assessmentInstance )
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the how many times did you eat fast food or snacks.
        /// </summary>
        /// <value>
        /// The how many times did you eat fast food or snacks.
        /// </value>
        [Code("7125003")]
        [DisplayOrder(0)]
        [IsRequired]
        public FastFoodFrequency HowManyTimesDidYouEatFastFoodOrSnacks { get; protected set; }

        /// <summary>
        /// Gets or sets the how many times did youeat fruits or vegetables.
        /// </summary>
        /// <value>
        /// The how many times did youeat fruits or vegetables.
        /// </value>
        [Code("7125004")]
        [DisplayOrder (1)]
        [IsRequired]
        public FruitsVegetablesFrequency HowManyTimesDidYoueatFruitsOrVegetables { get; protected set; }

        /// <summary>
        /// Gets or sets the how many sod and sugar drinks.
        /// </summary>
        /// <value>
        /// The how many sod and sugar drinks.
        /// </value>
        [Code("7125005")]
        [DisplayOrder(2)]
        [IsRequired]
        public SodaSugarFrequency HowManySodAndSugarDrinks { get; protected set; }

        #endregion
    }
}