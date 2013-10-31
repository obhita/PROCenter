namespace ProCenter.Service.Message.Patient
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using Agatha.Common;

    #endregion

    public class GetPatientDashboardResponse : Response
    {
        public IEnumerable<object> DashboardItems { get; set; }
    }
}