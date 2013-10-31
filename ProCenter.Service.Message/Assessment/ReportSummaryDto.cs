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
namespace ProCenter.Service.Message.Assessment
{
    #region Using Statements

    using System;
    using Common;
    using Domain.AssessmentModule;

    #endregion

    /// <summary>
    ///     Data transfer object for report summary.
    /// </summary>
    public class ReportSummaryDto : KeyedDataTransferObject
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the report severity.
        /// </summary>
        /// <value>
        /// The report severity.
        /// </value>
        public ReportSeverity ReportSeverity { get; set; }

        /// <summary>
        /// Gets or sets the report status.
        /// </summary>
        /// <value>
        /// The report status.
        /// </value>
        public string ReportStatus { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance can customize.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can customize; otherwise, <c>false</c>.
        /// </value>
        public bool CanCustomize { get; set; }

        /// <summary>
        ///     Gets or sets the created time.
        /// </summary>
        /// <value>
        ///     The created time.
        /// </value>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        ///     Gets the created time string.
        /// </summary>
        /// <value>
        ///     The created time string.
        /// </value>
        public string CreatedTimeString
        {
            get { return CreatedTime.ToShortDateString (); }
        }

        /// <summary>
        ///     Gets or sets the display name.
        /// </summary>
        /// <value>
        ///     The display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the source key.
        /// </summary>
        /// <value>
        ///     The source key.
        /// </value>
        public Guid SourceKey { get; set; }

        #endregion
    }
}