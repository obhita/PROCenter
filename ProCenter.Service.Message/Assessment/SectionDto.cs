namespace ProCenter.Service.Message.Assessment
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common;
    using Pillar.Common.Metadata;
    using Pillar.Common.Metadata.Dtos;

    #endregion

    public class SectionDto : KeyedDataTransferObject, IAssessmentDto, IContainItems
    {
        public IList<ItemDto> Items { get; set; }

        public string AssessmentName { get; set; }

        public Guid AssessmentKey { get; set; }

        public Guid AssessmentDefinitionKey { get; set; }

        public string AssessmentDefinitionCode { get; set; }

        public Guid PatientKey { get; set; }

        public ScoreDto Score { get; set; }

        public bool IsSubmitted { get; set; }

        public bool IsComplete { get; set; }

        public double PercentComplete { get; set; }
    }

    public interface IAssessmentDto
    {
    }
}