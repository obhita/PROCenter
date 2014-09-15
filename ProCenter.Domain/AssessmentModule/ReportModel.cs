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

namespace ProCenter.Domain.AssessmentModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>The report model class.</summary>
    public class ReportModel : IReportModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportModel"/> class.
        /// </summary>
        public ReportModel ()
        {
            ItemMetadata = new ItemMetadata ();
            ReportItems = new List<ReportItem> ();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether [is customizable].
        /// </summary>
        /// <value>
        ///   <c>True</c> if [is customizable]; otherwise, <c>false</c>.
        /// </value>
        public bool IsCustomizable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is patient viewable].
        /// </summary>
        /// <value>
        ///   <c>True</c> if [is patient viewable]; otherwise, <c>false</c>.
        /// </value>
        public bool IsPatientViewable { get; set; }

        /// <summary>
        /// Gets or sets the item metadata.
        /// </summary>
        /// <value>
        /// The item metadata.
        /// </value>
        public ItemMetadata ItemMetadata { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the report items.
        /// </summary>
        /// <value>
        /// The report items.
        /// </value>
        public IEnumerable<ReportItem> ReportItems { get; set; }

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
        /// Gets or sets the type of the report.
        /// </summary>
        /// <value>
        /// The type of the report.
        /// </value>
        public ReportType ReportType { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>Adds the report item.</summary>
        /// <param name="reportItem">The report item.</param>
        public void AddReportItem ( ReportItem reportItem )
        {
            ( ReportItems as IList<ReportItem> ).Add ( reportItem );
        }

        /// <summary>Finds the report item.</summary>
        /// <param name="name">The name.</param>
        /// <returns>A <see cref="ReportItem"/>.</returns>
        public ReportItem FindReportItem ( string name )
        {
            foreach ( var reportItem in ReportItems )
            {
                ReportItem foundReportItem = null;
                ReportItemHelper (
                                  reportItem,
                    ri =>
                    {
                        if ( ri.Name == name )
                        {
                            foundReportItem = ri;
                            return false;
                        }
                        return true;
                    } );
                if ( foundReportItem != null )
                {
                    return foundReportItem;
                }
            }
            return null;
        }

        /// <summary>Recurses the report items.</summary>
        /// <param name="action">The action.</param>
        public void RecurseReportItems ( Func<ReportItem, bool> action )
        {
            foreach ( var reportItem in ReportItems )
            {
                if ( !ReportItemHelper ( reportItem, action ) )
                {
                    break;
                }
            }
        }

        /// <summary>Updates the report item.</summary>
        /// <param name="name">The name.</param>
        /// <param name="shouldShow">The should show.</param>
        /// <param name="text">The text.</param>
        public void UpdateReportItem ( string name, bool? shouldShow, string text )
        {
            var item = FindReportItem ( name );
            item.Update ( shouldShow, text );
        }

        #endregion

        #region Methods

        private bool ReportItemHelper ( ReportItem reportItem, Func<ReportItem, bool> action )
        {
            if ( action ( reportItem ) )
            {
                if ( reportItem.ReportItems != null )
                {
                    foreach ( var subReportItem in reportItem.ReportItems )
                    {
                        var shouldContinue = ReportItemHelper ( subReportItem, action );
                        if ( !shouldContinue )
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        #endregion
    }
}