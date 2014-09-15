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
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>
    /// The PediatricSymptonChecklist class.
    /// </summary>
    [CodeSystem(CodeSystems.ObhitaCode)]
    [Code("7000000")]
    [ScoreType(ScoreTypeEnum.ScoreTypeInt)]
    public class PediatricSymptonChecklist : Assessment
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the PediatricSymptonChecklist class.
        /// </summary>
        static PediatricSymptonChecklist()
        {
            AssessmentCodedConcept = GetCodedConcept<PediatricSymptonChecklist>();
        }

        /// <summary>
        /// Initializes a new instance of the PediatricSymptonChecklist class.
        /// </summary>
        public PediatricSymptonChecklist()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PediatricSymptonChecklist class.
        /// </summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        public PediatricSymptonChecklist(AssessmentInstance assessmentInstance)
            : base(assessmentInstance)
        {
            ChildsEmotionalAndBehaviorProblemsGroup = new ChildsEmotionalAndBehaviorProblemsGroup(assessmentInstance);
        }

        #endregion

        #region Public Properties

        /// <summary>Gets the assessment coded concept.</summary>
        /// <value>The assessment coded concept.</value>
        public static CodedConcept AssessmentCodedConcept { get; private set; }

        /// <summary>
        /// Gets the total group.
        /// </summary>
        /// <value>
        /// The total group.
        /// </value>
        [Code("71250002")]
        [DisplayOrder(0)]
        public ChildsEmotionalAndBehaviorProblemsGroup ChildsEmotionalAndBehaviorProblemsGroup { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [does your child have any emotional or behavioral problems they need help].
        /// </summary>
        /// <value>
        /// <c>true</c> if [does your child have any emotional or behavioral problems they need help]; otherwise, <c>false</c>.
        /// </value>
        [Code("71250038")]
        [DisplayOrder(35)]
        [IsRequired]
        public bool DoesYourChildHaveAnyEmotionalOrBehavioralProblemsTheyNeedHelp { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [are there any services that you would like your child to receive for these problems].
        /// </summary>
        /// <value>
        /// <c>true</c> if [are there any services that you would like your child to receive for these problems]; otherwise, <c>false</c>.
        /// </value>
        [Code("71250039")]
        [DisplayOrder(36)]
        [IsRequired]
        public bool AreThereAnyServicesThatYouWouldLikeYourChildToReceiveForTheseProblems { get; protected set; }

        /// <summary>
        /// Gets or sets the are there any services that you would like your child to receive for these problems description.
        /// </summary>
        /// <value>
        /// The are there any services that you would like your child to receive for these problems description.
        /// </value>
        [Code("71250040")]
        [DisplayOrder(37)]
        [IsRequired]
        [ItemTemplate("TextArea")]
        public string AreThereAnyServicesThatYouWouldLikeYourChildToReceiveForTheseProblemsDescription { get; set; }

        #endregion
    }
}