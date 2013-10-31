#region Licence Header
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