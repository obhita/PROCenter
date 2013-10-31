namespace ProCenter.Service.Message.Attribute
{
    #region Using Statements

    using System;
    using System.Web.Mvc;

    #endregion

    public class LookupCategoryAttribute : Attribute, IMetadataAware
    {
        public const string LookupCategory = "LookupCategory";

        public LookupCategoryAttribute(string category)
        {
            Category = category;
        }

        public string Category { get; private set; }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[LookupCategory] = Category;
        }
    }

    public class CheckAllAttribute : Attribute, IMetadataAware
    {
        public const string CheckAll = "CheckAll";

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[CheckAll] = true;
        }
    }
}