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

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Attributes;

    #endregion

    /// <summary>
    /// The drug use frequency group class.
    /// </summary>
    public class TotalDisorderScreenerGroup : Group
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TotalDisorderScreenerGroup"/> class.
        /// </summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        public TotalDisorderScreenerGroup(AssessmentInstance assessmentInstance)
            : base ( assessmentInstance )
        {
            InternalizingDisorderScreenerGroup = new InternalizingDisorderScreenerGroup(assessmentInstance);
            ExternalizingDisorderScreenerGroup = new ExternalizingDisorderScreenerGroup(assessmentInstance);
            SubstanceDisorderScreenerGroup = new SubstanceDisorderScreenerGroup(assessmentInstance);
            CrimeViolenceScreenerGroup = new CrimeViolenceScreenerGroup(assessmentInstance);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the internalizing disorder screener group.
        /// </summary>
        /// <value>
        /// The internalizing disorder screener group.
        /// </value>
        [Code("6125002")]
        [DisplayOrder (0)]
        public InternalizingDisorderScreenerGroup InternalizingDisorderScreenerGroup { get; protected set; }

        /// <summary>
        /// Gets or sets the externalizing disorder screener group.
        /// </summary>
        /// <value>
        /// The externalizing disorder screener group.
        /// </value>
        [Code("6125010")]
        [DisplayOrder(1)]
        public ExternalizingDisorderScreenerGroup ExternalizingDisorderScreenerGroup { get; protected set; }

        /// <summary>
        /// Gets or sets the substance disorder screener group.
        /// </summary>
        /// <value>
        /// The substance disorder screener group.
        /// </value>
        [Code("6125018")]
        [DisplayOrder(2)]
        public SubstanceDisorderScreenerGroup SubstanceDisorderScreenerGroup { get; protected set; }

        /// <summary>
        /// Gets or sets the crime violence screener group.
        /// </summary>
        /// <value>
        /// The crime violence screener group.
        /// </value>
        [Code("6125024")]
        [DisplayOrder(3)]
        public CrimeViolenceScreenerGroup CrimeViolenceScreenerGroup { get; protected set; }

        #endregion
    }
}