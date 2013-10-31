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