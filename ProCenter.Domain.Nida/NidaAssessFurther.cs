namespace ProCenter.Domain.Nida
{
    using System.Collections.Generic;
    using AssessmentModule;
    using AssessmentModule.Lookups;
    using CommonModule;
    using Pillar.Common.Metadata;
    using Pillar.Common.Metadata.Dtos;
    using Service.Message.Metadata;

    public class NidaAssessFurther : AssessmentDefinition
    {
        public static CodedConcept AssessmentCodedConcept
        {
            get { return new CodedConcept(CodeSystems.Nci, "0000000", "NidaAssessFurther"); }
        }

        public static List<ItemDefinition> DrugAndFrequencyGroup = new List<ItemDefinition>
                {
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3269979",
                                         "SubstanceAbuseIllicitSubstanceMarijuanaPersonalMedicalHistoryFrequency"),
                        ItemType.Question, ValueType.Frequency, Frequency.DrugUseFrequencies)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem { TemplateName = "LookupDto"},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3269980",
                                         "SubstanceAbuseIllicitSubstanceCocainePersonalMedicalHistoryFrequency"),
                        ItemType.Question, ValueType.Frequency, Frequency.DrugUseFrequencies)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem> { 
                                        new ItemTemplateMetadataItem { TemplateName = "LookupDto"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),}
                                }
                        },
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3269981",
                                         "SubstanceAbuseIllicitSubstanceOpioidPersonalMedicalHistoryFrequency"),
                        ItemType.Question, ValueType.Frequency, Frequency.DrugUseFrequencies)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem { TemplateName = "LookupDto"},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3269982",
                                         "SubstanceAbuseIllicitSubstanceStimulantPersonalMedicalHistoryFrequency"),
                        ItemType.Question, ValueType.Frequency, Frequency.DrugUseFrequencies)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem { TemplateName = "LookupDto"},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3269983",
                                         "SubstanceAbuseIllicitSubstanceSedativePersonalMedicalHistoryFrequency"),
                        ItemType.Question, ValueType.Frequency, Frequency.DrugUseFrequencies)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem { TemplateName = "LookupDto"},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3269985",
                                         "SubstanceAbuseIllicitSubstanceOtherSubstanceofAbusePersonalMedicalHistorySpecify"),
                        ItemType.Question, ValueType.Specify)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem { TemplateName = "String"},
                                        }
                                }
                        },
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3269984",
                                         "SubstanceAbuseIllicitSubstanceOtherSubstanceofAbusePersonalMedicalHistoryFrequency"),
                        ItemType.Question, ValueType.Frequency, Frequency.DrugUseFrequencies)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem { TemplateName = "LookupDto"},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                };

        public NidaAssessFurther() : base(AssessmentCodedConcept)
        {
            var itemDefinitions = DrugAndFrequencyGroup;

            var frequencyGroupItemDefinition =
                new ItemDefinition(
                    new CodedConcept(CodeSystems.Nci, "2928115", "DrugTypeandFrequencyofUse[C]"),
                    ItemType.Group,
                    null, null, itemDefinitions);
            AddItemDefinition(frequencyGroupItemDefinition);

            itemDefinitions = new List<ItemDefinition>
                {
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3269978",
                                         "SubstanceAbuseIllicitSubstanceIntravenousRouteofAdministrationPersonalMedicalHistoryInd-2"),
                        ItemType.Question, ValueType.YesOrNoResponse)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem { TemplateName = "Boolean"},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3269986",
                                         "SubstanceAbuseIllicitSubstanceIntravenousRouteofAdministrationPersonalMedicalHistoryFrequency"),
                        ItemType.Question, ValueType.Frequency, Frequency.InjectionFrequencies)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem { TemplateName = "LookupDto"},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                };

            var injectionGroupItemDefinition =
                new ItemDefinition(
                    new CodedConcept(CodeSystems.Nci, "3269973", "InjectionDrugUse[C]"),
                    ItemType.Group,
                    null, null, itemDefinitions);
            AddItemDefinition(injectionGroupItemDefinition);


            itemDefinitions = new List<ItemDefinition>
                {
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3269977",
                                         "SubstanceAbuseSubstance-RelatedDisorderPriorTherapyPersonalMedicalHistoryInd-2"),
                        ItemType.Question, ValueType.YesOrNoResponse)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem { TemplateName = "Boolean"},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3269976",
                                         "SubstanceAbuseSubstance-RelatedDisorderCurrentTherapyPersonalMedicalHistoryInd-2"),
                        ItemType.Question, ValueType.YesOrNoResponse)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem { TemplateName = "Boolean"},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                };

            var sudTreatmentGroupItemDefinition =
                new ItemDefinition(
                    new CodedConcept(CodeSystems.Nci, "2928120", "SUDTreatment/Status[C]"),
                    ItemType.Group,
                    null, null, itemDefinitions);
            AddItemDefinition(sudTreatmentGroupItemDefinition);
        }
    }
}