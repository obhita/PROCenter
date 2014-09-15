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

    using System.Resources;

    using ProCenter.Common;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>
    /// The NihScoringEngine class.
    /// </summary>
    public class NihHealthBehaviorsAssessmentScoringEngine : IScoringEngine
    {
        #region Fields

        private readonly IResourcesManager _resourcesManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NihHealthBehaviorsAssessmentScoringEngine"/> class.
        /// </summary>
        public NihHealthBehaviorsAssessmentScoringEngine ()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NihHealthBehaviorsAssessmentScoringEngine"/> class.
        /// </summary>
        /// <param name="resourcesManager">The resources manager.</param>
        public NihHealthBehaviorsAssessmentScoringEngine ( IResourcesManager resourcesManager )
        {
            _resourcesManager = resourcesManager;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the name of the assessment.
        /// </summary>
        /// <value>
        ///     The name of the assessment.
        /// </value>
        public string AssessmentName
        {
            get { return NihHealthBehaviorsAssessment.AssessmentCodedConcept.Name; }
        }

        /// <summary>
        ///     Gets the resource manager.
        /// </summary>
        /// <value>
        ///     The resource manager.
        /// </value>
        public ResourceManager ResourceManager
        {
            get { return _resourcesManager.GetResourceManagerByName ( AssessmentName ); }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Calculates the score.
        /// </summary>
        /// <param name="assessment">The assessment.</param>
        public void CalculateScore ( AssessmentInstance assessment )
        {
            assessment.ScoreComplete(new CodedConcept(CodeSystems.Obhita, string.Empty, string.Empty), "Completed", true);
        }

        #endregion

        #region Methods

        #endregion
    }
}