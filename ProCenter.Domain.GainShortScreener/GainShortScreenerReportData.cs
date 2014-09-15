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

    #endregion

    /// <summary>
    ///     NIDA Report Data.
    /// </summary>
    public class GainShortScreenerReportData
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GainShortScreenerReportData"/> class.
        /// </summary>
        public GainShortScreenerReportData ()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GainShortScreenerReportData" /> class.
        /// </summary>
        /// <param name="gainShortScreenerScore">The gain short screener score.</param>
        /// <param name="summaryReportInfo">The summary report information.</param>
        public GainShortScreenerReportData ( GainShortScreenerScore gainShortScreenerScore, SummaryReportInfo summaryReportInfo )
        {
            InternalizingDisorder = gainShortScreenerScore.InternalDisorder;
            ExternalizingDisorder = gainShortScreenerScore.ExternalDisorder;
            SubstanceDisorder = gainShortScreenerScore.SubstanceDisorder;
            CriminalViolenceDisorder = gainShortScreenerScore.CriminalViolenceDisorder;
            TotalDisorder = gainShortScreenerScore.TotalDisorder;
            SummaryReportInfo = summaryReportInfo;
            Lifetime = new List<int> ();
            ( (List<int>)Lifetime ).AddRange (
                                              new[]
                                              {
                                                  gainShortScreenerScore.InternalDisorder.Lifetime,
                                                  gainShortScreenerScore.ExternalDisorder.Lifetime,
                                                  gainShortScreenerScore.SubstanceDisorder.Lifetime,
                                                  gainShortScreenerScore.CriminalViolenceDisorder.Lifetime
                                              } );

            PastMonth = new List<int> ();
            ( (List<int>)PastMonth ).AddRange (
                                               new[]
                                               {
                                                   gainShortScreenerScore.InternalDisorder.PastMonth,
                                                   gainShortScreenerScore.ExternalDisorder.PastMonth,
                                                   gainShortScreenerScore.SubstanceDisorder.PastMonth,
                                                   gainShortScreenerScore.CriminalViolenceDisorder.PastMonth
                                               } );

            TwoToThreeMonths = new List<int> ();
            ( (List<int>)TwoToThreeMonths ).AddRange (
                                                      new[]
                                                      {
                                                          gainShortScreenerScore.InternalDisorder.Past90Days - gainShortScreenerScore.InternalDisorder.PastMonth,
                                                          gainShortScreenerScore.ExternalDisorder.Past90Days - gainShortScreenerScore.ExternalDisorder.PastMonth,
                                                          gainShortScreenerScore.SubstanceDisorder.Past90Days - gainShortScreenerScore.SubstanceDisorder.PastMonth,
                                                          gainShortScreenerScore.CriminalViolenceDisorder.Past90Days - gainShortScreenerScore.CriminalViolenceDisorder.PastMonth
                                                      } );

            FourToTwelveMonths = new List<int> ();
            ( (List<int>)FourToTwelveMonths ).AddRange (
                                                        new[]
                                                        {
                                                            gainShortScreenerScore.InternalDisorder.PastYear - gainShortScreenerScore.InternalDisorder.Past90Days,
                                                            gainShortScreenerScore.ExternalDisorder.PastYear - gainShortScreenerScore.ExternalDisorder.Past90Days,
                                                            gainShortScreenerScore.SubstanceDisorder.PastYear - gainShortScreenerScore.SubstanceDisorder.Past90Days,
                                                            gainShortScreenerScore.CriminalViolenceDisorder.PastYear - gainShortScreenerScore.CriminalViolenceDisorder.Past90Days
                                                        } );

            MoreThenOneYear = new List<int> ();
            ( (List<int>)MoreThenOneYear ).AddRange (
                                                     new[]
                                                     {
                                                         gainShortScreenerScore.InternalDisorder.Lifetime - gainShortScreenerScore.InternalDisorder.PastYear,
                                                         gainShortScreenerScore.ExternalDisorder.Lifetime - gainShortScreenerScore.ExternalDisorder.PastYear,
                                                         gainShortScreenerScore.SubstanceDisorder.Lifetime - gainShortScreenerScore.SubstanceDisorder.PastYear,
                                                         gainShortScreenerScore.CriminalViolenceDisorder.Lifetime - gainShortScreenerScore.CriminalViolenceDisorder.PastYear
                                                     } );

            TotalLifetime = new List<int> { gainShortScreenerScore.TotalDisorder.Lifetime };
            TotalPastMonth = new List<int> { gainShortScreenerScore.TotalDisorder.PastMonth };
            TotalTwoToThreeMonths = new List<int> { gainShortScreenerScore.TotalDisorder.Past90Days - gainShortScreenerScore.TotalDisorder.PastMonth };
            TotalFourToTwelveMonths = new List<int> { gainShortScreenerScore.TotalDisorder.PastYear - gainShortScreenerScore.TotalDisorder.Past90Days };
            TotalMoreThenOneYear = new List<int> { gainShortScreenerScore.TotalDisorder.Lifetime - gainShortScreenerScore.TotalDisorder.PastYear };
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the summary report information.
        /// </summary>
        /// <value>
        /// The summary report information.
        /// </value>
        public SummaryReportInfo SummaryReportInfo { get; private set; }

        /// <summary>
        ///     Gets the four to twelve months.
        /// </summary>
        /// <value>
        ///     The four to twelve months.
        /// </value>
        public IList<int> FourToTwelveMonths { get; private set; }

        /// <summary>
        /// Gets the internalizing disorder.
        /// </summary>
        /// <value>
        /// The internalizing disorder.
        /// </value>
        public GainGroupScore InternalizingDisorder { get; private set; }

        /// <summary>
        /// Gets the externalizing disorder.
        /// </summary>
        /// <value>
        /// The externalizing disorder.
        /// </value>
        public GainGroupScore ExternalizingDisorder { get; private set; }

        /// <summary>
        /// Gets the substance disorder.
        /// </summary>
        /// <value>
        /// The substance disorder.
        /// </value>
        public GainGroupScore SubstanceDisorder { get; private set; }

        /// <summary>
        /// Gets the criminal violence disorder.
        /// </summary>
        /// <value>
        /// The criminal violence disorder.
        /// </value>
        public GainGroupScore CriminalViolenceDisorder { get; private set; }

        /// <summary>
        /// Gets the total disorder.
        /// </summary>
        /// <value>
        /// The total disorder.
        /// </value>
        public GainGroupScore TotalDisorder { get; private set; }

        /// <summary>
        ///     Gets the lifetime.
        /// </summary>
        /// <value>
        ///     The lifetime.
        /// </value>
        public IList<int> Lifetime { get; private set; }

        /// <summary>
        ///     Gets the more then one year.
        /// </summary>
        /// <value>
        ///     The more then one year.
        /// </value>
        public IList<int> MoreThenOneYear { get; private set; }

        /// <summary>
        ///     Gets the past month.
        /// </summary>
        /// <value>
        ///     The past month.
        /// </value>
        public IList<int> PastMonth { get; private set; }

        /// <summary>
        ///     Gets the total four to twelve months.
        /// </summary>
        /// <value>
        ///     The total four to twelve months.
        /// </value>
        public IList<int> TotalFourToTwelveMonths { get; private set; }

        /// <summary>
        ///     Gets the total lifetime.
        /// </summary>
        /// <value>
        ///     The total lifetime.
        /// </value>
        public IList<int> TotalLifetime { get; private set; }

        /// <summary>
        ///     Gets the total more then one year.
        /// </summary>
        /// <value>
        ///     The total more then one year.
        /// </value>
        public IList<int> TotalMoreThenOneYear { get; private set; }

        /// <summary>
        ///     Gets the total past month.
        /// </summary>
        /// <value>
        ///     The total past month.
        /// </value>
        public IList<int> TotalPastMonth { get; private set; }

        /// <summary>
        ///     Gets the total two to three months.
        /// </summary>
        /// <value>
        ///     The total two to three months.
        /// </value>
        public IList<int> TotalTwoToThreeMonths { get; private set; }

        /// <summary>
        ///     Gets the two to three months.
        /// </summary>
        /// <value>
        ///     The two to three months.
        /// </value>
        public IList<int> TwoToThreeMonths { get; private set; }

        #endregion
    }
}