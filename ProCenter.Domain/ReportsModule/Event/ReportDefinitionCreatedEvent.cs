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

namespace ProCenter.Domain.ReportsModule.Event
{
    #region Using Statements

    using System;

    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>The report template created event class.</summary>
    public class ReportDefinitionCreatedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReportDefinitionCreatedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="systemAccountKey">The system account key.</param>
        /// <param name="reportName">Name of the report.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="isPatientCentric">
        ///     If set to <c>true</c> [is patient centric].
        /// </param>
        public ReportDefinitionCreatedEvent ( Guid key, int version, Guid systemAccountKey, string reportName, string displayName, bool isPatientCentric )
            : base ( key, version )
        {
            SystemAccountKey = systemAccountKey;
            ReportName = reportName;
            DisplayName = displayName;
            IsPatientCentric = isPatientCentric;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the display name.
        /// </summary>
        /// <value>
        ///     The display name.
        /// </value>
        public string DisplayName { get; private set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is patient centric.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is patient centric; otherwise, <c>false</c>.
        /// </value>
        public bool IsPatientCentric { get; set; }

        /// <summary>
        ///     Gets the name of the report.
        /// </summary>
        /// <value>
        ///     The name of the report.
        /// </value>
        public string ReportName { get; private set; }

        /// <summary>
        ///     Gets the system account key.
        /// </summary>
        /// <value>
        ///     The system account key.
        /// </value>
        public Guid SystemAccountKey { get; private set; }

        #endregion
    }
}