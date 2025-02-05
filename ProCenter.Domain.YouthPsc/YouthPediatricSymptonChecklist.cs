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

namespace ProCenter.Domain.YouthPsc
{
    #region Using Statements

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Attributes;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>
    /// The YouthPediatricSymptonChecklist class.
    /// </summary>
    [CodeSystem(CodeSystems.ObhitaCode)]
    [Code("8000000")]
    public class YouthPediatricSymptonChecklist : Assessment
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the YouthPediatricSymptonChecklist class.
        /// </summary>
        static YouthPediatricSymptonChecklist()
        {
            AssessmentCodedConcept = GetCodedConcept<YouthPediatricSymptonChecklist>();
        }

        /// <summary>
        /// Initializes a new instance of the YouthPediatricSymptonChecklist class.
        /// </summary>
        public YouthPediatricSymptonChecklist()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the YouthPediatricSymptonChecklist class.
        /// </summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        public YouthPediatricSymptonChecklist(AssessmentInstance assessmentInstance)
            : base(assessmentInstance)
        {
            YouthChildsEmotionalAndBehaviorProblemsGroup = new YouthChildsEmotionalAndBehaviorProblemsGroup(assessmentInstance);
        }

        #endregion

        #region Public Properties

        /// <summary>Gets the assessment coded concept.</summary>
        /// <value>The assessment coded concept.</value>
        public static CodedConcept AssessmentCodedConcept { get; private set; }

        /// <summary>
        /// Gets the childs emotional and behavior problems group.
        /// </summary>
        /// <value>
        /// The childs emotional and behavior problems group.
        /// </value>
        [Code("81250002")]
        [DisplayOrder(0)]
        public YouthChildsEmotionalAndBehaviorProblemsGroup YouthChildsEmotionalAndBehaviorProblemsGroup { get; private set; }

        #endregion
    }
}