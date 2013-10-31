namespace ProCenter.Service.Message.Assessment
{
    #region Using Statements

    using System.Collections.Generic;
    using Common;
    using Common.Lookups;
    using Domain.AssessmentModule;
    using Metadata;

    #endregion

    public class ItemDto : KeyedDataTransferObject, IAssessmentDto, IContainItems
    {
        public string ItemDefinitionCode { get; set; }

        public string ItemDefinitionName { get; set; }

        public IEnumerable<LookupDto> Options { get; set; }

        public object Value { get; set; }

        public ItemMetadata Metadata { get; set; }

        public string ItemType { get; set; }

        public IList<ItemDto> Items { get; set; } 
    }
}