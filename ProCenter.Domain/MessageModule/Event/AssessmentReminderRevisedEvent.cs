#region Licence Header
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
namespace ProCenter.Domain.MessageModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;
    using Pillar.Domain.Primitives;

    #endregion

    /// <summary>
    ///     Event for when assessment reminder time is revised.
    /// </summary>
    public class AssessmentReminderRevisedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssessmentReminderRevisedEvent" /> class.
        /// </summary>
        public AssessmentReminderRevisedEvent ()
            : base ( Guid.Empty, -1 )
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssessmentReminderRevisedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="time">The time.</param>
        /// <param name="unit">The unit.</param>
        public AssessmentReminderRevisedEvent ( Guid key, int version, double time, AssessmentReminderUnit unit, Email sendToEmail = null )
            : base ( key, version )
        {
            Time = time;
            Unit = unit;
            SendToEmail = sendToEmail;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the time.
        /// </summary>
        /// <value>
        ///     The time.
        /// </value>
        public double Time { get; private set; }

        /// <summary>
        ///     Gets the unit.
        /// </summary>
        /// <value>
        ///     The unit.
        /// </value>
        public AssessmentReminderUnit Unit { get; private set; }

        /// <summary>
        /// Gets the send to email.
        /// </summary>
        /// <value>
        /// The send to email.
        /// </value>
        public Email SendToEmail { get; private set; }

        #endregion
    }
}