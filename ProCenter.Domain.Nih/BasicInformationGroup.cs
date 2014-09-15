namespace ProCenter.Domain.Nih
{
    using System;
    using System.Collections.Generic;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Attributes;

    /// <summary>
    /// The BasicInformationGroup class.
    /// </summary>
    public class BasicInformationGroup : Group 
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicInformationGroup"/> class.
        /// </summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        public BasicInformationGroup(AssessmentInstance assessmentInstance)
            : base ( assessmentInstance )
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the what year were you born.
        /// </summary>
        /// <value>
        /// The what year were you born.
        /// </value>
        [Code("7125023")]
        [DisplayOrder(0)]
        [IsRequired]
        public int WhatYearWereYouBorn { get; protected set; }

        /// <summary>
        /// Gets or sets the what is your sex.
        /// </summary>
        /// <value>
        /// The what is your sex.
        /// </value>
        [Code("7125024")]
        [DisplayOrder(1)]
        [IsRequired]
        public NihHealthBehaviorsAssessmentGender WhatIsYourSex { get; protected set; }

        /// <summary>
        /// Gets or sets the are you latino.
        /// </summary>
        /// <value>
        /// The are you latino.
        /// </value>
        [Code("7125025")]
        [DisplayOrder(2)]
        [IsRequired]
        [ItemTemplate("MultipleSelect")]
        public IEnumerable<NihHealthBehaviorsAssessmentHispanicOrSpanish> AreYouLatino { get; protected set; }

        /// <summary>
        /// Gets or sets the what is your race.
        /// </summary>
        /// <value>
        /// The what is your race.
        /// </value>
        [Code("7125026")]
        [DisplayOrder(3)]
        [IsRequired]
        [ItemTemplate("MultipleSelect")]
        public IEnumerable<NihHealthBehaviorsAssessmentRace> WhatIsYourRace { get; protected set; }

        /// <summary>
        /// Gets or sets the what country were you born.
        /// </summary>
        /// <value>
        /// The what country were you born.
        /// </value>
        [Code("7125027")]
        [DisplayOrder(4)]
        [IsRequired]
        public string WhatCountryWereYouBorn { get; protected set; }

        /// <summary>
        /// Gets or sets the how well do you speak english.
        /// </summary>
        /// <value>
        /// The how well do you speak english.
        /// </value>
        [Code("7125028")]
        [DisplayOrder(5)]
        [IsRequired]
        public NihHealthBehaviorsAssessmentSpeakEnglish HowWellDoYouSpeakEnglish { get; protected set; }

        /// <summary>
        /// Gets or sets the what language do you feel most comfortable with.
        /// </summary>
        /// <value>
        /// The what language do you feel most comfortable with.
        /// </value>
        [Code("7125029")]
        [DisplayOrder(6)]
        [IsRequired]
        public string WhatLanguageDoYouFeelMostComfortableWith { get; protected set; }

        /// <summary>
        /// Gets or sets the do you need an interpreter.
        /// </summary>
        /// <value>
        /// The do you need an interpreter.
        /// </value>
        [Code("7125030")]
        [DisplayOrder(7)]
        [IsRequired]
        public NihHealthBehaviorsAssessmentNeedInterpreter DoYouNeedAnInterpreter { get; protected set; }

        /// <summary>
        /// Gets or sets the employment status.
        /// </summary>
        /// <value>
        /// The employment status.
        /// </value>
        [Code("7125031")]
        [DisplayOrder(8)]
        [IsRequired]
        [ItemTemplate("MultipleSelect")]
        public IEnumerable<NihHealthBehaviorsAssessmentEmploymentStatus> EmploymentStatus { get; protected set; }

        /// <summary>
        /// Gets or sets the employment status other.
        /// </summary>
        /// <value>
        /// The employment status other.
        /// </value>
        [Code("7125038")]
        [DisplayOrder(9)]
        [IsRequired]
        public string EmploymentStatusOther { get; protected set; }

        /// <summary>
        /// Gets or sets the marit status.
        /// </summary>
        /// <value>
        /// The marit status.
        /// </value>
        [Code("7125032")]
        [DisplayOrder(10)]
        [IsRequired]
        public NihHealthBehaviorsAssessmentMaritalStatus MaritStatus { get; protected set; }

        /// <summary>
        /// Gets or sets the what is the highest level of school completed.
        /// </summary>
        /// <value>
        /// The what is the highest level of school completed.
        /// </value>
        [Code("7125033")]
        [DisplayOrder(11)]
        [IsRequired]
        public NihHealthBehaviorsAssessmentSchoolLevel WhatIsTheHighestLevelOfSchoolCompleted { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [did you ever serve on active duty].
        /// </summary>
        /// <value>
        /// <c>true</c> if [did you ever serve on active duty]; otherwise, <c>false</c>.
        /// </value>
        [Code("7125034")]
        [DisplayOrder(12)]
        [IsRequired]
        public bool DidYouEverServeOnActiveDuty { get; protected set; }

        #endregion
    }
}