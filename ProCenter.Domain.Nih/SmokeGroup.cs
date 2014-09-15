namespace ProCenter.Domain.Nih
{
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Attributes;

    /// <summary>
    /// The SmokeGroup class.
    /// </summary>
    public class SmokeGroup : Group 
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SmokeGroup"/> class.
        /// </summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        public SmokeGroup(AssessmentInstance assessmentInstance)
            : base ( assessmentInstance )
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether [have used tobacco in last30 days smoked].
        /// </summary>
        /// <value>
        /// <c>true</c> if [have used tobacco in last30 days smoked]; otherwise, <c>false</c>.
        /// </value>
        [Code("7125036")]
        [DisplayOrder(0)]
        [IsRequired]
        public bool HaveUsedTobaccoInLast30DaysSmoked { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [have used tobacco in last30 days smokeless tobacco].
        /// </summary>
        /// <value>
        /// <c>true</c> if [have used tobacco in last30 days smokeless tobacco]; otherwise, <c>false</c>.
        /// </value>
        [Code("7125037")]
        [DisplayOrder(1)]
        [IsRequired]
        public bool HaveUsedTobaccoInLast30DaysSmokelessTobacco { get; protected set; }

        #endregion
    }
}