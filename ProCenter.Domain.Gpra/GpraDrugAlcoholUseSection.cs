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

    /// <summary>The gpra drug alcohol use section class.</summary>
    public class GpraDrugAlcoholUseSection
    {
        #region Public Properties

        /// <summary>
        /// Gets the gpra drug alcohol use group.
        /// </summary>
        /// <value>
        /// The gpra drug alcohol use group.
        /// </value>
        public static List<ItemDefinition> GpraDrugAlcoholUseGroup
            {
            get
                    {
                return new List<ItemDefinition>
                       {
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000086",
                               "AnyAlcoholDayCount" ),
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
                               "0000087",
                               "AlcholIntoxicationFivePlusDrinksDayCount" ),
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
                               "0000088",
                               "AlcholIntoxicationFourOrFewerDrinksDayCount" ),
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
                               "0000089",
                               "IllegaldrugsDayCount" ),
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
                               "0000090",
                               "SameDayAlcoholDrugsDayCount" ),
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
                               "0000091",
                               "CocaineCrackDayCount" ),
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
                               "0000092",
                               "CocaineCrackGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000093",
                               "MarijuanaHashishDayCount" ),
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
                               "0000094",
                               "MarijuanaHashishGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000095",
                               "HerionDayCount" ),
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
                               "0000096",
                               "HerionGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000097",
                               "MorphineDayCount" ),
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
                               "0000098",
                               "MorphineGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000099",
                               "DiluadidDayCount" ),
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
                               "0000100",
                               "DiluadidGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000101",
                               "DermerolDayCount" ),
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
                               "0000102",
                               "DermerolGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000103",
                               "PercocetDayCount" ),
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
                               "0000104",
                               "PercocetGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000105",
                               "DarvonDayCount" ),
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
                               "0000106",
                               "DarvonGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000107",
                               "CodeineDayCount" ),
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
                               "0000108",
                               "CodeineGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000109",
                               "TylenolDayCount" ),
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
                               "0000110",
                               "TylenolGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000111",
                               "OxycontinOxycodoneDayCount" ),
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
                               "0000112",
                               "OxycontinOxycodoneGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000113",
                               "NonPrescriptionMethadoneDayCount" ),
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
                               "0000114",
                               "NonPrescriptionMethadoneGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000115",
                               "HallucinogensDayCount" ),
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
                               "0000116",
                               "HallucinogensGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000117",
                               "MethamphetamineDayCount" ),
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
                               "0000118",
                               "MethamphetamineGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000119",
                               "BenzodiazepinesDayCount" ),
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
                               "0000120",
                               "BenzodiazepinesGpraDrugCount" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000121",
                               "NonPrescriptionGhbDayCount" ),
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
                               "0000122",
                               "NonPrescriptionGhbGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000123",
                               "BarbituratesDayCount" ),
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
                               "0000124",
                               "BarbituratesGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000125",
                               "KetamineDayCount" ),
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
                               "0000126",
                               "KetamineGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000127",
                               "TranquilizersDayCount" ),
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
                               "0000128",
                               "TranquilizersGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000129",
                               "InhalantsDayCount" ),
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
                               "0000130",
                               "InhalantsGpraDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000131",
                               "OtherIllegalDrugsDayCount" ),
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
                               "0000132",
                               "OtherIllegalGpraDrugsDrugRoute" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.AdministrationRoute )
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
                               "0000133",
                               "OtherIllegalDrugsSpecificationNote" ),
                               ItemType.Question,
                               ValueType.Specify )
                    {
                        ItemMetadata = new ItemMetadata
                            {
                                MetadataItems = new List<IMetadataItem>
                                    {
                                                                      new ItemTemplateMetadataItem { TemplateName = "String" },
                                    }
                            }
                    },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000134",
                               "InjectedDrugsIndicator" ),
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
                               "0000135",
                               "InjectionGpraFrequencyOfUseOfUsedItems" ),
                               ItemType.Question,
                               ValueType.DrugAlcoholLookups,
                               DrugAlcoholLookups.Frequency )
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