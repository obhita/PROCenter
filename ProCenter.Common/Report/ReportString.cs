namespace ProCenter.Common.Report
{
    public class ReportString
    {
        #region Public Properties

        public string Value { get; set; }

        #endregion

        #region Public Methods and Operators

        public static implicit operator string ( ReportString reportString )
        {
            return reportString.Value;
        }

        public static implicit operator ReportString(string stringValue)
        {
            return new ReportString { Value = stringValue};
        }

        #endregion
    }
}