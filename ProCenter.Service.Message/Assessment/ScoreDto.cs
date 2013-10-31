namespace ProCenter.Service.Message.Assessment
{
    using System.Collections.Generic;
    using Domain.AssessmentModule;

    public class ScoreDto
    {
        public string ItemDefinitionCode { get; set; }
        public object Value { get; set; }
        public string GuidanceCode { get; set; }

        public ItemMetadata ItemMetadata { get; set; }

        public IEnumerable<ScoreDto> ScoreItems { get; set; }
    }
}
