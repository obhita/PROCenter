#region License Header
// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/
#endregion
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