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

namespace ProCenter.Domain.Psc
{
    #region Using Statements

    #endregion

    /// <summary>
    ///     The PediatricSymptonChecklistScore class.
    /// </summary>
    public class PediatricSymptonChecklistScore
    {
        #region Static Fields

        #endregion

        #region Constructors and Destructors

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the test date.
        /// </summary>
        /// <value>
        /// The test date.
        /// </value>
        public string TestDate { get; set; }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>
        /// The age.
        /// </value>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the completed by.
        /// </summary>
        /// <value>
        /// The completed by.
        /// </value>
        public string CompletedBy { get; set; }

        /// <summary>
        /// Gets or sets the does your child have any emotional or behavioral problems description.
        /// </summary>
        /// <value>
        /// The does your child have any emotional or behavioral problems description.
        /// </value>
        public string DoesYourChildHaveAnyEmotionalOrBehavioralProblemsDescription { get; set; }

        /// <summary>
        /// Gets or sets the are there any services that you would like your child to receive for these problems answer.
        /// </summary>
        /// <value>
        /// The are there any services that you would like your child to receive for these problems answer.
        /// </value>
        public string AreThereAnyServicesThatYouWouldLikeYourChildToReceiveForTheseProblemsAnswer { get; set; }

        /// <summary>
        /// Gets or sets the are there any services that you would like your child to receive for these problems description.
        /// </summary>
        /// <value>
        /// The are there any services that you would like your child to receive for these problems description.
        /// </value>
        public string AreThereAnyServicesThatYouWouldLikeYourChildToReceiveForTheseProblemsDescription { get; set; }

        /// <summary>
        /// Gets or sets the total score.
        /// </summary>
        /// <value>
        /// The total score.
        /// </value>
        public int TotalScore { get; set; }

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

        #endregion
    }
}