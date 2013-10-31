namespace ProCenter.Mvc.Models
{
    #region

    using System.Runtime.Serialization;

    #endregion

    [DataContract]
    public class CalendarEventModel
    {
        [DataMember(Name = "id")]
        public string Key { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "allDay")]
        public bool AllDay { get; set; }

        [DataMember(Name = "start")]
        public double Start { get; set; }

        [DataMember(Name = "end")]
        public double End { get; set; }
    }
}