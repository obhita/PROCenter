namespace ProCenter.Mvc.Models
{
    #region Using Statements

    using System.Collections.Generic;
    using System.Runtime.Serialization;

    #endregion

    [DataContract]
    public class DataTableResponse<TModel>
    {
        [DataMember(Name = "sEcho")]
        public string Echo { get; set; }

        [DataMember(Name = "iTotalRecords")]
        public int TotalRecords { get; set; }

        [DataMember(Name = "iTotalDisplayRecords")]
        public int TotalDisplayRecords { get; set; }

        [DataMember(Name = "aaData")]
        public IEnumerable<TModel> Data { get; set; }
    }
}