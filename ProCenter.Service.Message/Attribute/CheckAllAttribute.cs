namespace ProCenter.Service.Message.Attribute
{
    #region Using Statements

    using System;
    using System.Web.Mvc;

    #endregion

    /// <summary>The check all attribute class.</summary>
    public class CheckAllAttribute : Attribute, IMetadataAware
    {
        #region Constants

        public const string CheckAll = "CheckAll";

        #endregion

        #region Public Methods and Operators

        /// <summary>When implemented in a class, provides metadata to the model metadata creation process.</summary>
        /// <param name="metadata">The model metadata.</param>
        public void OnMetadataCreated ( ModelMetadata metadata )
        {
            metadata.AdditionalValues[CheckAll] = true;
        }

        #endregion
    }
}