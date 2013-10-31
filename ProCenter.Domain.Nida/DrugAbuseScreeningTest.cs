namespace ProCenter.Domain.Nida
{
    using System.Collections.Generic;
    using AssessmentModule;
    using AssessmentModule.Lookups;
    using CommonModule;
    //using Infrastructure.Service.Completeness;
    using Pillar.Common.Metadata;
    using Pillar.Common.Metadata.Dtos;
    using Service.Message.Metadata;

    public class DrugAbuseScreeningTest : AssessmentDefinition
    {
        public static CodedConcept AssessmentCodedConcept
        {
            get { return new CodedConcept(CodeSystems.Nci, "3254100", "DrugAbuseScreeningTest"); }
        }

        public DrugAbuseScreeningTest() : base(AssessmentCodedConcept)
        {
            var booleanType = typeof (bool).Name;
            var itemDefinitions = new List<ItemDefinition>
                {
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3254039",
                                         "SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductPersonalMedicalHistoryInd-2"),
                        ItemType.Question, ValueType.YesOrNoResponse)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem {TemplateName = booleanType},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3254057",
                                         "SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductConcurrentUsePersonalMedicalHistoryInd-2"),
                        ItemType.Question, ValueType.YesOrNoResponse)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem {TemplateName = booleanType},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3254058",
                                         "SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductCessationAbilityPersonalMedicalHistoryInd-2"),
                        ItemType.Question, ValueType.YesOrNoResponse)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem {TemplateName = booleanType},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3254061",
                                         "SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductBlackoutFlashbacksPersonalMedicalHistoryInd-2"),
                        ItemType.Question, ValueType.YesOrNoResponse)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem {TemplateName = booleanType},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3254063",
                                         "SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductGuiltRegretPersonalMedicalHistoryInd-2"),
                        ItemType.Question, ValueType.YesOrNoResponse)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem {TemplateName = booleanType},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3254065",
                                         "SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductDomesticPartnershipSpouseComplainPersonalMedicalHistoryInd-2"),
                        ItemType.Question, ValueType.YesOrNoResponse)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem {TemplateName = booleanType},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3254066",
                                         "SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductFamilyNeglectPersonalMedicalHistoryInd-2"),
                        ItemType.Question, ValueType.YesOrNoResponse)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem {TemplateName = booleanType},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3254067",
                                         "SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductCrimeObtainPersonalMedicalHistoryInd-2"),
                        ItemType.Question, ValueType.YesOrNoResponse)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem {TemplateName = booleanType},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3254070",
                                         "SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductSubstanceWithdrawalSyndromePersonalMedicalHistoryInd-2"),
                        ItemType.Question, ValueType.YesOrNoResponse)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem {TemplateName = booleanType},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                    new ItemDefinition(
                        new CodedConcept(CodeSystems.Nci, "3254072",
                                         "SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductAssociatedDiseaseorDisorderPersonalMedicalHistoryInd-2"),
                        ItemType.Question, ValueType.YesOrNoResponse)
                        {
                            ItemMetadata = new ItemMetadata
                                {
                                    MetadataItems = new List<IMetadataItem>
                                        {
                                            new ItemTemplateMetadataItem {TemplateName = booleanType},
                                            new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                        }
                                }
                        },
                    //new ItemDefinition(
                    //    new CodedConcept(CodeSystems.Nci, "3254089",
                    //                     "SubstanceAbusePrescriptionIllicitSubstanceOvertheCounterProductScreeningTestScore"),
                    //    ItemType.Question, ValueType.Score)
                    //    {
                    //        ItemMetadata = new ItemMetadata
                    //            {
                    //                MetadataItems = new List<IMetadataItemDto>
                    //                    {
                    //                        new ItemTemplateMetadataItemDto {TemplateName = "Int32"}
                    //                    }
                    //            }
                    //    },
                };

            foreach (var itemDefinition in itemDefinitions)
            {
                AddItemDefinition(itemDefinition);
            }
        }
    }
}