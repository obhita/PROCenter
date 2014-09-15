namespace ProCenter.Domain.AssessmentModule.Metadata
{
    #region Using Statements

    using System.Collections.Generic;

    using Pillar.Common.Metadata;

    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>The non response type metadata item class.</summary>
    public class NonResponseTypeMetadataItem : IMetadataItem
    {
        #region Public Properties

        /// <summary>Gets or sets the answers to exclude.</summary>
        /// <value>The answers to exclude.</value>
        public IList<Lookup> AnswersToExclude { get; set; }

        #endregion
    }
}