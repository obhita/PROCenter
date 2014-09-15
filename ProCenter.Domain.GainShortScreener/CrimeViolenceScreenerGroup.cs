namespace ProCenter.Domain.GainShortScreener
{
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Attributes;

    /// <summary>
    /// CrimeViolenceScreenerGroup Class.
    /// </summary>
    public class CrimeViolenceScreenerGroup : Group
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CrimeViolenceScreenerGroup"/> class.
        /// </summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        public CrimeViolenceScreenerGroup(AssessmentInstance assessmentInstance)
            : base ( assessmentInstance )
        {
        }
        #endregion

        /// <summary>
        /// Gets or sets the had a disagreement.
        /// </summary>
        /// <value>
        /// The had a disagreement.
        /// </value>
        [Code("6125025")]
        [DisplayOrder(0)]
        [IsRequired]
        public LastTimeFrequency HadADisagreement { get; protected set; }

        /// <summary>
        /// Gets or sets the took something from a store without paying.
        /// </summary>
        /// <value>
        /// The took something from a store without paying.
        /// </value>
        [Code("6125026")]
        [DisplayOrder(1)]
        [IsRequired]
        public LastTimeFrequency TookSomethingFromAStoreWithoutPaying { get; protected set; }

        /// <summary>
        /// Gets or sets the sold distributed or helped to make illegal drugs.
        /// </summary>
        /// <value>
        /// The sold distributed or helped to make illegal drugs.
        /// </value>
        [Code("6125027")]
        [DisplayOrder(2)]
        [IsRequired]
        public LastTimeFrequency SoldDistributedOrHelpedToMakeIllegalDrugs { get; protected set; }

        /// <summary>
        /// Gets or sets the drove under the influence of alcohol or illegal drugs.
        /// </summary>
        /// <value>
        /// The drove under the influence of alcohol or illegal drugs.
        /// </value>
        [Code("6125028")]
        [DisplayOrder(3)]
        [IsRequired]
        public LastTimeFrequency DroveUnderTheInfluenceOfAlcoholOrIllegalDrugs { get; protected set; }

        /// <summary>
        /// Gets or sets the purposely damaged property that did not belong to you.
        /// </summary>
        /// <value>
        /// The purposely damaged property that did not belong to you.
        /// </value>
        [Code("6125029")]
        [DisplayOrder(4)]
        [IsRequired]
        public LastTimeFrequency PurposelyDamagedPropertyThatDidNotBelongToYou { get; protected set; }
    }
}