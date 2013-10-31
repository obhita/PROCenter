namespace ProCenter.Mvc.Infrastructure.Extension
{
    #region Using Statements

    using System;
    using System.Linq;
    using Pillar.Common.Metadata;
    using Pillar.Common.Metadata.Dtos;

    #endregion

    /// <summary>
    ///     Metadata Dto Extensions
    /// </summary>
    public static class MetadataDtoExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Metadatas the item exists.
        /// </summary>
        /// <typeparam name="TMetadataItem">The type of the metadata item.</typeparam>
        /// <param name="metadataDto">The metadata dto.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public static bool MetadataItemExists<TMetadataItem> ( this MetadataDto metadataDto, Predicate<TMetadataItem> callback ) where TMetadataItem : IMetadataItemDto
        {
            if ( metadataDto.MetadataItemDtos != null )
            {
                return metadataDto.MetadataItemDtos.Any ( ( m => m.GetType () == typeof(TMetadataItem) && callback ( (TMetadataItem) m ) ) );
            }
            return false;
        }

        #endregion
    }
}