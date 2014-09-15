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

namespace ProCenter.Domain.AssessmentModule.Event
{
    #region Using Statements

    using System;

    using ProCenter.Common.Email;
    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>Updates email sent date.</summary>
    public class UpdateEmailSentDateEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateEmailSentDateEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="emailSentDate">The email sent date.</param>
        /// <param name="emailFailedDate">The email failed date.</param>
        public UpdateEmailSentDateEvent ( Guid key, int version, DateTime? emailSentDate, DateTime? emailFailedDate)
            : base ( key, version )
        {
            EmailSentDate = emailSentDate;
            EmailFailedDate = emailFailedDate;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the email sent date.
        /// </summary>
        /// <value>
        /// The email sent date.
        /// </value>
        public DateTime? EmailSentDate { get; private set; }

        /// <summary>
        /// Gets the email failed date.
        /// </summary>
        /// <value>
        /// The email failed date.
        /// </value>
        public DateTime? EmailFailedDate { get; private set; }

        #endregion
    }
}