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