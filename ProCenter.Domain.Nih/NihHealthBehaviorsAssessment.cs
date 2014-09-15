#region License Header

// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/

#endregion

namespace ProCenter.Domain.Nih
{
    #region Using Statements
    
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Attributes;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>The NihHealthBehaviorsAssessment assessment class.</summary>
    [CodeSystem(CodeSystems.ObhitaCode)]
    [Code("7125000")]
    [ScoreType(ScoreTypeEnum.NoScore)]
    public class NihHealthBehaviorsAssessment : Assessment
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the NihHealthBehaviorsAssessment class.
        /// </summary>
        static NihHealthBehaviorsAssessment()
        {
            AssessmentCodedConcept = GetCodedConcept<NihHealthBehaviorsAssessment>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NihHealthBehaviorsAssessment"/> class.
        /// </summary>
        public NihHealthBehaviorsAssessment()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NihHealthBehaviorsAssessment"/> class.
        /// </summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        public NihHealthBehaviorsAssessment(AssessmentInstance assessmentInstance)
            : base(assessmentInstance)
        {
            DietGroup = new DietGroup ( assessmentInstance );
            ExerciseGroup = new ExerciseGroup ( assessmentInstance );
            ProblemGroup = new ProblemGroup ( assessmentInstance );
            SmokeGroup = new SmokeGroup ( assessmentInstance );
            BasicInformationGroup = new BasicInformationGroup ( assessmentInstance ); 
        }

        #endregion

        #region Public Properties

        /// <summary>Gets the assessment coded concept.</summary>
        /// <value>The assessment coded concept.</value>
        public static CodedConcept AssessmentCodedConcept { get; private set; }

        /// <summary>
        /// Gets the diet group.
        /// </summary>
        /// <value>
        /// The diet group.
        /// </value>
        [Code("7125002")]
        [DisplayOrder(0)]
        public DietGroup DietGroup { get; private set; }

        /// <summary>
        /// Gets or sets the how much do you weight.
        /// </summary>
        /// <value>
        /// The how much do you weight.
        /// </value>
        [Code("7125039")]
        [DisplayOrder(1)]
        [IsRequired]
        public int WhatIsYourWeight { get; protected set; }

        /// <summary>
        /// Gets or sets the height of the what is your.
        /// </summary>
        /// <value>
        /// The height of the what is your.
        /// </value>
        [Code("7125040")]
        [DisplayOrder(2)]
        [IsRequired]
        [ItemTemplate("Height")]
        public int WhatIsYourHeight { get; protected set; }

        /// <summary>
        /// Gets the exercise group.
        /// </summary>
        /// <value>
        /// The exercise group.
        /// </value>
        [Code("7125008")]
        [DisplayOrder(3)]
        public ExerciseGroup ExerciseGroup { get; private set; }

        /// <summary>
        /// Gets or sets the how much stress in last7 days.
        /// </summary>
        /// <value>
        /// The how much stress in last7 days.
        /// </value>
        [Code("7125011")]
        [DisplayOrder(4)]
        [IsRequired]
        public int HowMuchStressInLast7Days { get; protected set; }

        /// <summary>
        /// Gets the problem group.
        /// </summary>
        /// <value>
        /// The problem group.
        /// </value>
        [Code("7125012")]
        [DisplayOrder(5)]
        public ProblemGroup ProblemGroup { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [do you snore or has anybody told you].
        /// </summary>
        /// <value>
        /// <c>true</c> if [do you snore or has anybody told you]; otherwise, <c>false</c>.
        /// </value>
        [Code("7125017")]
        [DisplayOrder(6)]
        [IsRequired]
        public bool DoYouSnoreOrHasAnybodyToldYou { get; protected set; }

        /// <summary>
        /// Gets or sets the how often sleep during the day.
        /// </summary>
        /// <value>
        /// The how often sleep during the day.
        /// </value>
        [Code("7125018")]
        [DisplayOrder(7)]
        [IsRequired]
        public SleepDuringDayFrequency HowOftenSleepDuringTheDay { get; protected set; }

        /// <summary>
        /// Gets the smoke group.
        /// </summary>
        /// <value>
        /// The smoke group.
        /// </value>
        [Code("7125019")]
        [DisplayOrder(8)]
        public SmokeGroup SmokeGroup { get; private set; }

        /// <summary>
        /// Gets or sets the how many times have you had four to five or more drinks in a day.
        /// </summary>
        /// <value>
        /// The how many times have you had four to five or more drinks in a day.
        /// </value>
        [Code("7125020")]
        [DisplayOrder(9)]
        [IsRequired]
        public AlcoholDrugUseFrequency HowManyTimesHaveYouHadFourToFiveOrMoreDrinksInADay { get; protected set; }

        /// <summary>
        /// Gets or sets the how many times have you used illegal drugs for non medical reasons.
        /// </summary>
        /// <value>
        /// The how many times have you used illegal drugs for non medical reasons.
        /// </value>
        [Code("7125021")]
        [DisplayOrder(10)]
        [IsRequired]
        public AlcoholDrugUseFrequency HowManyTimesHaveYouUsedIllegalDrugsForNonMedicalReasons { get; protected set; }

        /// <summary>
        /// Gets or sets the in general would you say your health is.
        /// </summary>
        /// <value>
        /// The in general would you say your health is.
        /// </value>
        [Code("7125022")]
        [DisplayOrder(11)]
        [IsRequired]
        public HealthCondition InGeneralWouldYouSayYourHealthIs { get; protected set; }

        /// <summary>
        /// Gets or sets the poor health factors.
        /// </summary>
        /// <value>
        /// The poor health factors.
        /// </value>
        [Code("7125041")]
        [DisplayOrder(12)]
        [IsRequired]
        [ItemTemplate("TextArea")]
        public string PoorHealthFactors { get; protected set; }

        /// <summary>
        /// Gets or sets the basic information group.
        /// </summary>
        /// <value>
        /// The basic information group.
        /// </value>
        [Code("7125035")]
        [DisplayOrder(13)]
        public BasicInformationGroup BasicInformationGroup { get; protected set; }

        #endregion
    }
}