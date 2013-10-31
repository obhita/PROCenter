namespace ProCenter.Domain.AssessmentModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Pillar.Common.Metadata;
    using Pillar.Common.Metadata.Dtos;

    #endregion

    public class ItemMetadata
    {
        public IEnumerable<IMetadataItem> MetadataItems { get; set; }

        public TMetadataItem FindMetadataItem<TMetadataItem>() where TMetadataItem : class, IMetadataItem
        {
            if ( MetadataItems == null )
            {
                return null;
            }
            return (TMetadataItem) (MetadataItems).SingleOrDefault((m => m.GetType() == typeof (TMetadataItem)));
        }

        public bool MetadataItemExists<TMetadataItem>(Predicate<TMetadataItem> callback = null) where TMetadataItem : IMetadataItem
        {
            callback = callback ?? (m => true);
            if (MetadataItems != null)
            {
                return MetadataItems.Any((m => m.GetType() == typeof(TMetadataItem) && callback((TMetadataItem)m)));
            }
            return false;
        }
    }
}