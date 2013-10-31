namespace ProCenter.Service.Message.Common.Lookups
{
    #region Using Statements

    using System.Collections.Generic;
    using Agatha.Common;

    #endregion

    public class GetLookupsByCategoryResponse : Response
    {
        public GetLookupsByCategoryResponse()
        {
            Lookups = new List<LookupDto>();
        }

        public IList<LookupDto> Lookups { get; set; }

        public string Category { get; set; }
    }
}