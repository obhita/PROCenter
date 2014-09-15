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

    #endregion

    /// <summary>
    ///     The GainShortScreenerScoring class.
    /// </summary>
    public class GainShortScreenerScore : IGenerateReport
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GainShortScreenerScore" /> class.
        /// </summary>
        /// <param name="internalDisorder">The internal disorder.</param>
        /// <param name="externalDisorder">The external disorder.</param>
        /// <param name="substanceDisorder">The substance disorder.</param>
        /// <param name="criminalViolenceDisorder">The criminal violence disorder.</param>
        public GainShortScreenerScore (
            GainGroupScore internalDisorder,
            GainGroupScore externalDisorder,
            GainGroupScore substanceDisorder,
            GainGroupScore criminalViolenceDisorder )
        {
            InternalDisorder = internalDisorder;
            ExternalDisorder = externalDisorder;
            SubstanceDisorder = substanceDisorder;
            CriminalViolenceDisorder = criminalViolenceDisorder;
            TotalDisorder = new GainGroupScore (
                internalDisorder.PastMonth + externalDisorder.PastMonth + substanceDisorder.PastMonth + criminalViolenceDisorder.PastMonth,
                internalDisorder.Past90Days + externalDisorder.Past90Days + substanceDisorder.Past90Days + criminalViolenceDisorder.Past90Days,
                internalDisorder.PastYear + externalDisorder.PastYear + substanceDisorder.PastYear + criminalViolenceDisorder.PastYear,
                internalDisorder.Lifetime + externalDisorder.Lifetime + substanceDisorder.Lifetime + criminalViolenceDisorder.Lifetime
                );
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the criminal violence disorder.
        /// </summary>
        /// <value>
        ///     The criminal violence disorder.
        /// </value>
        public GainGroupScore CriminalViolenceDisorder { get; private set; }

        /// <summary>
        ///     Gets the diagnosis level.
        /// </summary>
        /// <value>
        ///     The diagnosis level.
        /// </value>
        public DiagnosisLevel DiagnosisLevel
        {
            get { return GetDiagnosisLevel (); }
        }

        /// <summary>
        ///     Gets the external disorder.
        /// </summary>
        /// <value>
        ///     The external disorder.
        /// </value>
        public GainGroupScore ExternalDisorder { get; private set; }

        /// <summary>
        ///     Gets the internal disorder.
        /// </summary>
        /// <value>
        ///     The internal disorder.
        /// </value>
        public GainGroupScore InternalDisorder { get; private set; }

        /// <summary>
        ///     Gets the severity.
        /// </summary>
        /// <value>
        ///     The severity.
        /// </value>
        public ReportSeverity Severity
        {
            get
            {
                if ( DiagnosisLevel == DiagnosisLevel.High )
                {
                    return ReportSeverity.High;
                }
                if ( DiagnosisLevel == DiagnosisLevel.Medium )
                {
                    return ReportSeverity.Low;
                }
                return ReportSeverity.Good;
            }
        }

        /// <summary>
        ///     Gets the substance disorder.
        /// </summary>
        /// <value>
        ///     The substance disorder.
        /// </value>
        public GainGroupScore SubstanceDisorder { get; private set; }

        /// <summary>
        ///     Gets the total disorder.
        /// </summary>
        /// <value>
        ///     The total disorder.
        /// </value>
        public GainGroupScore TotalDisorder { get; private set; }

        /// <summary>
        ///     Gets the total score.
        /// </summary>
        /// <value>
        ///     The total score.
        /// </value>
        public int TotalScore
        {
            get { return TotalDisorder.Lifetime; }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     The total score as a string.
        /// </returns>
        public override string ToString ()
        {
            return TotalScore.ToString ();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the diagnosis level.
        /// </summary>
        /// <returns>Returns the diagnosis level, Low, Medium or High.</returns>
        private DiagnosisLevel GetDiagnosisLevel ()
        {
            if ( TotalScore == 0 )
            {
                return DiagnosisLevel.Low;
            }
            if ( TotalScore <= 2 )
            {
                return DiagnosisLevel.Medium;
            }
            return DiagnosisLevel.High;
        }

        #endregion
    }
}