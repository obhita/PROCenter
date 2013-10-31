namespace ProCenter.Domain.Nida
{
    #region Using Statements

    using System.Collections.Generic;
    using AssessmentModule;
    using AssessmentModule.Lookups;
    using CommonModule;
    using Pillar.Common.Metadata;
    using Pillar.Common.Metadata.Dtos;
    using Service.Message.Metadata;

    #endregion

    public class NidaSingleQuestionScreener : AssessmentDefinition
    {
        public NidaSingleQuestionScreener() : base(AssessmentCodedConcept)
        {
            var itemDefinition =
                new ItemDefinition(
                    new CodedConcept(CodeSystems.Nci, "3254097",
                                     "SubstanceAbusePrescriptionIllicitSubstancePastYearPersonalMedicalHistory"),
                    ItemType.Question,
                    ValueType.Count)
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new ItemTemplateMetadataItem {TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    };

            AddItemDefinition(itemDefinition);
        }

        public static CodedConcept AssessmentCodedConcept
        {
            get { return new CodedConcept(CodeSystems.Nci, "3254099", "NidaSingleQuestionScreener"); }
        }
    }
}