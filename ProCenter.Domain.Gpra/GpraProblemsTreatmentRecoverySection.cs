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

namespace ProCenter.Domain.Gpra
{
    #region Using Statements

using System.Collections.Generic;

using Pillar.Common.Metadata;

using ProCenter.Domain.AssessmentModule;
using ProCenter.Domain.AssessmentModule.Lookups;
using ProCenter.Domain.AssessmentModule.Metadata;
using ProCenter.Domain.CommonModule;
using ProCenter.Domain.Gpra.Lookups;
using ProCenter.Service.Message.Metadata;

    #endregion

    /// <summary>The gpra problems treatment recovery section class.</summary>
    public class GpraProblemsTreatmentRecoverySection
    {
        #region Public Properties

        /// <summary>
        /// Gets the gpra problems treatment recovery group.
        /// </summary>
        /// <value>
        /// The gpra problems treatment recovery group.
        /// </value>
        public static List<ItemDefinition> GpraProblemsTreatmentRecoveryGroup
            {
            get
                    {
                return new List<ItemDefinition>
                       {
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000167",
                               "GpraOverallHealth" ),
                               ItemType.Question,
                               ValueType.TreatmentRecoveryLookups,
                               TreatmentRecoveryLookups.HealthRating )
                           {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "LookupDto"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000168",
                               "InpatientPhysicalComplaintIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000169",
                               "InpatientPhysicalComplaintNights" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000170",
                               "InpatientMentalEmotionalDifficultiesIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000171",
                               "InpatientMentalEmotionalDifficultiesNights" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000172",
                               "InpatientAlcoholSubstanceAbuseIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000173",
                               "InpatientAlcoholSubstanceAbuseNights" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000174",
                               "OutpatientPhysicalComplaintIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000175",
                               "OutpatientPhysicalComplaintTimes" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000176",
                               "OutpatientMentalEmotionalDifficultiesIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000177",
                               "OutpatientMentalEmotionalDifficultiesTimes" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000178",
                               "OutpatientAlcoholSubstanceAbuseIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000179",
                               "OutpatientAlcoholSubstanceAbuseTimes" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000180",
                               "ERPhysicalComplaintIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000181",
                               "ERPhysicalComplaintTimes" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000182",
                               "ERMentalEmotionalDifficultiesIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000183",
                               "ERMentalEmotionalDifficultiesTimes" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000184",
                               "ERAlcoholSubstanceAbuseIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000185",
                               "ERAlcoholSubstanceAbuseTimes" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000186",
                               "GpraSexualActivity" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Boolean"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000187",
                               "SexualContactsCount" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000188",
                               "UnProtectedSexualContactsCount" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000189",
                               "UnProtectedSexualHivContactsCount" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000190",
                               "UnProtectedSexualInjectionDrugContactsCount" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000191",
                               "UnProtectedSexualHighSAContactsCount" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000192",
                               "HivTest" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Boolean"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000193",
                               "HivTestResultsKnownIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000194",
                               "DepressionDayCount" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000195",
                               "AnxietyDayCount" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000196",
                               "HallucinationsDayCount" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000197",
                               "BrainMisfunctionDayCount" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000198",
                               "ViolentBehaviorDayCount" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000199",
                               "SuicideDayCount" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000200",
                               "PsychologicalEmotionalMedicationDayCount" ),
                               ItemType.Question,
                               ValueType.Count )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "Int32"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000201",
                               "GpraPsychologicalImpact" ),
                               ItemType.Question,
                               ValueType.TreatmentRecoveryLookups,
                               TreatmentRecoveryLookups.PsychologicalImpact )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                        new NonResponseTypeMetadataItem(),
                                        new ItemTemplateMetadataItem { TemplateName = "LookupDto"},
                                        new RequiredForCompletenessMetadataItem(CompletenessCategory.Report),
                                    }
                            }
                    },
            };
    }
}

        #endregion
    }
}