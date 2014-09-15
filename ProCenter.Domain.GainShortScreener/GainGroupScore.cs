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
    /// <summary>
    ///     The gain group score class.
    /// </summary>
    public class GainGroupScore
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GainGroupScore" /> class.
        /// </summary>
        /// <param name="pastMonth">The past month.</param>
        /// <param name="past90Days">The past 90 days.</param>
        /// <param name="pastYear">The past year.</param>
        /// <param name="lifetime">The lifetime.</param>
        public GainGroupScore ( int pastMonth, int past90Days, int pastYear, int lifetime )
        {
            PastMonth = pastMonth;
            Past90Days = past90Days;
            PastYear = pastYear;
            Lifetime = lifetime;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the lifetime.
        /// </summary>
        /// <value>
        ///     The lifetime.
        /// </value>
        public int Lifetime { get; private set; }

        /// <summary>
        ///     Gets the past90 days.
        /// </summary>
        /// <value>
        ///     The past90 days.
        /// </value>
        public int Past90Days { get; private set; }

        /// <summary>
        ///     Gets the past month.
        /// </summary>
        /// <value>
        ///     The past month.
        /// </value>
        public int PastMonth { get; private set; }

        /// <summary>
        ///     Gets the past year.
        /// </summary>
        /// <value>
        ///     The past year.
        /// </value>
        public int PastYear { get; private set; }

        #endregion
    }
}