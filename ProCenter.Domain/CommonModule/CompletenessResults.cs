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

namespace ProCenter.Domain.CommonModule
{
    /// <summary>The completeness results class.</summary>
    public class CompletenessResults
    {
        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="CompletenessResults" /> class.</summary>
        /// <param name="completenessCategory">The completeness category.</param>
        /// <param name="total">The total.</param>
        /// <param name="numbercomplete">The numbercomplete.</param>
        public CompletenessResults ( string completenessCategory, int total, int numbercomplete )
        {
            CompletenessCategory = completenessCategory;
            Total = total;
            NumberComplete = numbercomplete;
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets the completeness category.</summary>
        /// <value>The completeness category.</value>
        public string CompletenessCategory { get; set; }

        /// <summary>Gets the number complete.</summary>
        /// <value>The number complete.</value>
        public int NumberComplete { get; private set; }

        /// <summary>Gets the number incomplete.</summary>
        /// <value>The number incomplete.</value>
        public int NumberIncomplete
        {
            get { return Total - NumberComplete; }
        }

        /// <summary>Gets the percent complete.</summary>
        /// <value>The percent complete.</value>
        public double PercentComplete
        {
            get { return (double)NumberComplete / Total; }
        }

        /// <summary>Gets the total.</summary>
        /// <value>The total.</value>
        public int Total { get; private set; }

        /// <summary>Gets a value indicating whether [is complete].</summary>
        /// <value><c>True</c> if is complete; otherwise, <c>False</c>.</value>
        public bool IsComplete { get { return Total == NumberComplete; }}

        #endregion
    }
}