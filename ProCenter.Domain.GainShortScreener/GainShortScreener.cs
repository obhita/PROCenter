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

namespace ProCenter.Domain.GainShortScreener
{
    #region Using Statements

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DevExpress.Charts.Native;
    using DevExpress.XtraPrinting.Native;

    using Pillar.Common.Metadata;
    using Pillar.FluentRuleEngine;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Attributes;
    using ProCenter.Domain.AssessmentModule.Metadata;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>The gain shorth screener class.</summary>
    [CodeSystem(CodeSystems.ObhitaCode)]
    [Code("6125004")]
    [ScoreType(ScoreTypeEnum.ScoreTypeInt)]
    public class GainShortScreener : Assessment
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the GainShortScreener class.
        /// </summary>
        static GainShortScreener()
        {
            AssessmentCodedConcept = GetCodedConcept<GainShortScreener>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GainShortScreener"/> class.
        /// </summary>
        public GainShortScreener()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GainShortScreener"/> class.
        /// </summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        public GainShortScreener(AssessmentInstance assessmentInstance)
            : base(assessmentInstance)
        {
            TotalDisorderScreenerGroup = new TotalDisorderScreenerGroup(assessmentInstance);
        }

        #endregion

        #region Public Properties

        /// <summary>Gets the assessment coded concept.</summary>
        /// <value>The assessment coded concept.</value>
        public static CodedConcept AssessmentCodedConcept { get; private set; }

        /// <summary>
        /// Gets the total disorder screener group.
        /// </summary>
        /// <value>
        /// The total disorder screener group.
        /// </value>
        [Code("6125001")]
        [DisplayOrder(0)]
        public TotalDisorderScreenerGroup TotalDisorderScreenerGroup { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [significant psychological behavioral personal problems question].
        /// </summary>
        /// <value>
        /// <c>true</c> if [significant psychological behavioral personal problems question]; otherwise, <c>false</c>.
        /// </value>
        [Code("6125030")]
        [DisplayOrder(1)]
        [IsRequired]
        public bool SignificantPsychologicalBehavioralPersonalProblemsQuestion { get; private set; }

        /// <summary>
        /// Gets the significant psychological behavioral personal problems describe.
        /// </summary>
        /// <value>
        /// The significant psychological behavioral personal problems describe.
        /// </value>
        [Code("6125031")]
        [DisplayOrder(2)]
        [IsRequired]
        [ItemTemplate("TextArea")]
        public string SignificantPsychologicalBehavioralPersonalProblemsDescribe { get; private set; }

        /// <summary>
        /// Gets the what is your gender.
        /// </summary>
        /// <value>
        /// The what is your gender.
        /// </value>
        [Code("6125032")]
        [DisplayOrder(3)]
        [IsRequired]
        public GainShortScreenerGender WhatIsYourGainShortScreenerGender { get; private set; }

        /// <summary>
        /// Gets the what is your gender other.
        /// </summary>
        /// <value>
        /// The what is your gender other.
        /// </value>
        [Code("6125033")]
        [DisplayOrder(4)]
        [IsRequired]
        [ItemTemplate("TextArea")]
        public string WhatIsYourGenderOther { get; private set; }

        /// <summary>
        /// Gets the how old are you today.
        /// </summary>
        /// <value>
        /// The how old are you today.
        /// </value>
        [Code("6125034")]
        [DisplayOrder(5)]
        [IsRequired]
        public int HowOldAreYouToday { get; private set; }

        /// <summary>
        /// Gets the how many minutes to complete survey.
        /// </summary>
        /// <value>
        /// The how many minutes to complete survey.
        /// </value>
        [Code("6125035")]
        [DisplayOrder(6)]
        [IsRequired]
        public int HowManyMinutesToCompleteSurvey { get; private set; }

        #endregion
    }
}