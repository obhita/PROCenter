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

namespace ProCenter.Mvc.Models
{
    #region Using Statements

    using ProCenter.Mvc.Infrastructure.Service.Completeness;
    using ProCenter.Service.Message.Assessment;

    #endregion

    /// <summary>The assessment view model class.</summary>
    public class AssessmentViewModel : IValidateCompleteness
    {
        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="AssessmentViewModel" /> class.</summary>
        /// <param name="assessmentSectionSummaryDto">The assessment section summary dto.</param>
        /// <param name="currentSectionDto">The current section dto.</param>
        public AssessmentViewModel ( AssessmentSectionSummaryDto assessmentSectionSummaryDto, SectionDto currentSectionDto )
        {
            AssessmentSectionSummaryDto = assessmentSectionSummaryDto;
            CurrentSectionDto = currentSectionDto;
        }

        #endregion

        #region Public Properties

        /// <summary>Gets the assessment section summary dto.</summary>
        /// <value>The assessment section summary dto.</value>
        public AssessmentSectionSummaryDto AssessmentSectionSummaryDto { get; private set; }

        /// <summary>Gets the current section dto.</summary>
        /// <value>The current section dto.</value>
        public SectionDto CurrentSectionDto { get; private set; }

        #endregion
    }
}