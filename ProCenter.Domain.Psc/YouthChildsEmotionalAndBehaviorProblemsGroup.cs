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

namespace ProCenter.Domain.Psc
{
    #region Using Statements

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Attributes;
    
    #endregion

    /// <summary>
    /// The YouthChildsEmotionalAndBehaviorProblemsGroup class.
    /// </summary>
    public class YouthChildsEmotionalAndBehaviorProblemsGroup : Group
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the YouthChildsEmotionalAndBehaviorProblemsGroup class.
        /// </summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        public YouthChildsEmotionalAndBehaviorProblemsGroup ( AssessmentInstance assessmentInstance )
            : base ( assessmentInstance )
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the complains of aches and pains.
        /// </summary>
        /// <value>
        /// The complains of aches and pains.
        /// </value>
        [Code("81250003")]
        [DisplayOrder(0)]
        [IsRequired]
        public TimeFrequency ComplainsOfAchesAndPains { get; protected set; }

        /// <summary>
        /// Gets or sets the spends more time alone.
        /// </summary>
        /// <value>
        /// The spends more time alone.
        /// </value>
        [Code("81250004")]
        [DisplayOrder(1)]
        [IsRequired]
        public TimeFrequency SpendsMoreTimeAlone { get; protected set; }

        /// <summary>
        /// Gets or sets the tires easily.
        /// </summary>
        /// <value>
        /// The tires easily.
        /// </value>
        [Code("81250005")]
        [DisplayOrder(2)]
        [IsRequired]
        public TimeFrequency TiresEasily { get; protected set; }

        /// <summary>
        /// Gets or sets the fidgety unable to sit still.
        /// </summary>
        /// <value>
        /// The fidgety unable to sit still.
        /// </value>
        [Code("81250006")]
        [DisplayOrder(3)]
        [IsRequired]
        public TimeFrequency FidgetyUnableToSitStill { get; protected set; }

        /// <summary>
        /// Gets or sets the has trouble with teacher.
        /// </summary>
        /// <value>
        /// The has trouble with teacher.
        /// </value>
        [Code("81250007")]
        [DisplayOrder(4)]
        [IsRequired]
        public TimeFrequency HasTroubleWithTeacher { get; protected set; }

        /// <summary>
        /// Gets or sets the less interested in school.
        /// </summary>
        /// <value>
        /// The less interested in school.
        /// </value>
        [Code("81250008")]
        [DisplayOrder(5)]
        [IsRequired]
        public TimeFrequency LessInterestedInSchool { get; protected set; }

        /// <summary>
        /// Gets or sets the acts as if driven by a motor.
        /// </summary>
        /// <value>
        /// The acts as if driven by a motor.
        /// </value>
        [Code("81250009")]
        [DisplayOrder(6)]
        [IsRequired]
        public TimeFrequency ActsAsIfDrivenByAMotor { get; protected set; }

        /// <summary>
        /// Gets or sets the daydreams too much.
        /// </summary>
        /// <value>
        /// The daydreams too much.
        /// </value>
        [Code("81250010")]
        [DisplayOrder(7)]
        [IsRequired]
        public TimeFrequency DaydreamsTooMuch { get; protected set; }

        /// <summary>
        /// Gets or sets the distracted easily.
        /// </summary>
        /// <value>
        /// The distracted easily.
        /// </value>
        [Code("81250011")]
        [DisplayOrder(8)]
        [IsRequired]
        public TimeFrequency DistractedEasily { get; protected set; }

        /// <summary>
        /// Gets or sets the is afraid of new situations.
        /// </summary>
        /// <value>
        /// The is afraid of new situations.
        /// </value>
        [Code("81250012")]
        [DisplayOrder(9)]
        [IsRequired]
        public TimeFrequency IsAfraidOfNewSituations { get; protected set; }

        /// <summary>
        /// Gets or sets the feels sad unhappy.
        /// </summary>
        /// <value>
        /// The feels sad unhappy.
        /// </value>
        [Code("81250013")]
        [DisplayOrder(10)]
        [IsRequired]
        public TimeFrequency FeelsSadUnhappy { get; protected set; }

        /// <summary>
        /// Gets or sets the is irritable angry.
        /// </summary>
        /// <value>
        /// The is irritable angry.
        /// </value>
        [Code("81250014")]
        [DisplayOrder(11)]
        [IsRequired]
        public TimeFrequency IsIrritableAngry { get; protected set; }

        /// <summary>
        /// Gets or sets the feels hopeless.
        /// </summary>
        /// <value>
        /// The feels hopeless.
        /// </value>
        [Code("81250015")]
        [DisplayOrder(12)]
        [IsRequired]
        public TimeFrequency FeelsHopeless { get; protected set; }

        /// <summary>
        /// Gets or sets the has trouble concentrating.
        /// </summary>
        /// <value>
        /// The has trouble concentrating.
        /// </value>
        [Code("81250016")]
        [DisplayOrder(13)]
        [IsRequired]
        public TimeFrequency HasTroubleConcentrating { get; protected set; }

        /// <summary>
        /// Gets or sets the less interested in friends.
        /// </summary>
        /// <value>
        /// The less interested in friends.
        /// </value>
        [Code("81250017")]
        [DisplayOrder(14)]
        [IsRequired]
        public TimeFrequency LessInterestedInFriends { get; protected set; }

        /// <summary>
        /// Gets or sets the fights with other children.
        /// </summary>
        /// <value>
        /// The fights with other children.
        /// </value>
        [Code("81250018")]
        [DisplayOrder(15)]
        [IsRequired]
        public TimeFrequency FightsWithOtherChildren { get; protected set; }

        /// <summary>
        /// Gets or sets the absent from school.
        /// </summary>
        /// <value>
        /// The absent from school.
        /// </value>
        [Code("81250019")]
        [DisplayOrder(16)]
        [IsRequired]
        public TimeFrequency AbsentFromSchool { get; protected set; }

        /// <summary>
        /// Gets or sets the school grades dropping.
        /// </summary>
        /// <value>
        /// The school grades dropping.
        /// </value>
        [Code("81250020")]
        [DisplayOrder(17)]
        [IsRequired]
        public TimeFrequency SchoolGradesDropping { get; protected set; }

        /// <summary>
        /// Gets or sets the is down on him or herself.
        /// </summary>
        /// <value>
        /// The is down on him or herself.
        /// </value>
        [Code("81250021")]
        [DisplayOrder(18)]
        [IsRequired]
        public TimeFrequency IsDownOnHimOrHerself { get; protected set; }

        /// <summary>
        /// Gets or sets the visits the doctor with doctor finding nothing wrong.
        /// </summary>
        /// <value>
        /// The visits the doctor with doctor finding nothing wrong.
        /// </value>
        [Code("81250022")]
        [DisplayOrder(19)]
        [IsRequired]
        public TimeFrequency VisitsTheDoctorWithDoctorFindingNothingWrong { get; protected set; }

        /// <summary>
        /// Gets or sets the has trouble sleeping.
        /// </summary>
        /// <value>
        /// The has trouble sleeping.
        /// </value>
        [Code("81250023")]
        [DisplayOrder(20)]
        [IsRequired]
        public TimeFrequency HasTroubleSleeping { get; protected set; }

        /// <summary>
        /// Gets or sets the worries alot.
        /// </summary>
        /// <value>
        /// The worries alot.
        /// </value>
        [Code("81250024")]
        [DisplayOrder(21)]
        [IsRequired]
        public TimeFrequency WorriesAlot { get; protected set; }

        /// <summary>
        /// Gets or sets the wants to be with you more than before.
        /// </summary>
        /// <value>
        /// The wants to be with you more than before.
        /// </value>
        [Code("81250025")]
        [DisplayOrder(22)]
        [IsRequired]
        public TimeFrequency WantsToBeWithYouMoreThanBefore { get; protected set; }

        /// <summary>
        /// Gets or sets the feels he or she is bad.
        /// </summary>
        /// <value>
        /// The feels he or she is bad.
        /// </value>
        [Code("81250026")]
        [DisplayOrder(23)]
        [IsRequired]
        public TimeFrequency FeelsHeOrSheIsBad { get; protected set; }

        /// <summary>
        /// Gets or sets the takes unnecessary risks.
        /// </summary>
        /// <value>
        /// The takes unnecessary risks.
        /// </value>
        [Code("81250027")]
        [DisplayOrder(24)]
        [IsRequired]
        public TimeFrequency TakesUnnecessaryRisks { get; protected set; }

        /// <summary>
        /// Gets or sets the gets hurt frequently.
        /// </summary>
        /// <value>
        /// The gets hurt frequently.
        /// </value>
        [Code("81250028")]
        [DisplayOrder(25)]
        [IsRequired]
        public TimeFrequency GetsHurtFrequently { get; protected set; }

        /// <summary>
        /// Gets or sets the seems to be having less fun.
        /// </summary>
        /// <value>
        /// The seems to be having less fun.
        /// </value>
        [Code("81250029")]
        [DisplayOrder(26)]
        [IsRequired]
        public TimeFrequency SeemsToBeHavingLessFun { get; protected set; }

        /// <summary>
        /// Gets or sets the acts younger than children his or her age.
        /// </summary>
        /// <value>
        /// The acts younger than children his or her age.
        /// </value>
        [Code("81250030")]
        [DisplayOrder(27)]
        [IsRequired]
        public TimeFrequency ActsYoungerThanChildrenHisOrHerAge { get; protected set; }

        /// <summary>
        /// Gets or sets the does not listen to rules.
        /// </summary>
        /// <value>
        /// The does not listen to rules.
        /// </value>
        [Code("81250031")]
        [DisplayOrder(28)]
        [IsRequired]
        public TimeFrequency DoesNotListenToRules { get; protected set; }

        /// <summary>
        /// Gets or sets the does no show feelings.
        /// </summary>
        /// <value>
        /// The does no show feelings.
        /// </value>
        [Code("81250032")]
        [DisplayOrder(29)]
        [IsRequired]
        public TimeFrequency DoesNoShowFeelings { get; protected set; }

        /// <summary>
        /// Gets or sets the does not understand other peoples feelings.
        /// </summary>
        /// <value>
        /// The does not understand other peoples feelings.
        /// </value>
        [Code("81250033")]
        [DisplayOrder(30)]
        [IsRequired]
        public TimeFrequency DoesNotUnderstandOtherPeoplesFeelings { get; protected set; }

        /// <summary>
        /// Gets or sets the teases others.
        /// </summary>
        /// <value>
        /// The teases others.
        /// </value>
        [Code("81250034")]
        [DisplayOrder(31)]
        [IsRequired]
        public TimeFrequency TeasesOthers { get; protected set; }

        /// <summary>
        /// Gets or sets the blames others for his or her troubles.
        /// </summary>
        /// <value>
        /// The blames others for his or her troubles.
        /// </value>
        [Code("81250035")]
        [DisplayOrder(32)]
        [IsRequired]
        public TimeFrequency BlamesOthersForHisOrHerTroubles { get; protected set; }

        /// <summary>
        /// Gets or sets the takes things that do not belong to him or her.
        /// </summary>
        /// <value>
        /// The takes things that do not belong to him or her.
        /// </value>
        [Code("81250036")]
        [DisplayOrder(33)]
        [IsRequired]
        public TimeFrequency TakesThingsThatDoNotBelongToHimOrHer { get; protected set; }

        /// <summary>
        /// Gets or sets the refuses to share.
        /// </summary>
        /// <value>
        /// The refuses to share.
        /// </value>
        [Code("81250037")]
        [DisplayOrder(34)]
        [IsRequired]
        public TimeFrequency RefusesToShare { get; protected set; }

        #endregion
    }
}