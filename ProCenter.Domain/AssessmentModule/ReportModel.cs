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
namespace ProCenter.Domain.AssessmentModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;

    #endregion

    public class ReportModel
    {
        public ReportModel ()
        {
            ItemMetadata = new ItemMetadata ();
            ReportItems = new List<ReportItem> ();
        }
        #region Public Properties

        public bool IsCustomizable { get; set; }

        public ReportSeverity ReportSeverity { get; set; }

        public string ReportStatus { get; set; }

        public ItemMetadata ItemMetadata { get; set; }

        public string Name { get; set; }

        public IEnumerable<ReportItem> ReportItems { get; set; }

        public bool IsPatientViewable { get; set; }

        #endregion

        #region Public Methods and Operators

        public void AddReportItem ( ReportItem reportItem )
        {
            ( ReportItems as IList<ReportItem> ).Add ( reportItem );
        }

        public ReportItem FindReportItem ( string name )
        {
            foreach ( var reportItem in ReportItems )
            {
                ReportItem foundReportItem = null;
                ReportItemHelper ( reportItem, ri =>
                {
                    if (ri.Name == name)
                    {
                        foundReportItem = ri;
                        return false;
                    }
                    return true;
                });
                if ( foundReportItem != null )
                {
                    return foundReportItem;
                }
            }
            return null;
        }

        public void UpdateReportItem ( string name, bool? shouldShow, string text )
        {
            var item = FindReportItem ( name );
            item.Update ( shouldShow, text );
        }

        public void RecurseReportItems( Func<ReportItem, bool> action )
        {
            foreach (var reportItem in ReportItems)
            {
                if ( !ReportItemHelper ( reportItem, action ) )
                {
                    break;
                }
            }
        }

        #endregion

        #region Methods

        private bool ReportItemHelper(ReportItem reportItem, Func<ReportItem, bool> action)
        {
            if ( action(reportItem) )
            {
                if (reportItem.ReportItems != null)
                {
                    foreach (var subReportItem in reportItem.ReportItems)
                    {
                        var shouldContinue = ReportItemHelper(subReportItem, action);
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