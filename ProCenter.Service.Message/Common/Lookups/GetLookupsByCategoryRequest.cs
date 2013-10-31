namespace ProCenter.Service.Message.Common.Lookups
{
    #region Using Statements

    using Agatha.Common;

    #endregion

    // NOTE: to clear cache, use ICacheManager in Create/Change/Delete Lookup item handlers.
    [EnableServiceResponseCaching(Hours = 1)]
    [EnableClientResponseCaching(Hours = 4)]
    public class GetLookupsByCategoryRequest : Request
    {
        public string Category { get; set; }

        public bool IncludingInactiveItems { get; set; }
    }
}