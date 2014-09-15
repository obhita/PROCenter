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
    using ProCenter.Domain.CommonModule.Lookups;
    using ProCenter.Domain.Gpra.Lookups;
    using ProCenter.Service.Message.Metadata;

    #endregion

    /// <summary>The gpra demographics section class.</summary>
    public class GpraDemographicsSection
    {
        #region Public Properties

        /// <summary>
        ///     Gets the gpra demographics group.
        /// </summary>
        /// <value>
        ///     The gpra demographics group.
        /// </value>
        public static List<ItemDefinition> GpraDemographicsGroup
        {
            get
            {
                return new List<ItemDefinition>
                       {
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000067",
                               "GpraPatientGender" ),
                               ItemType.Question,
                               ValueType.Gender,
                               Gender.GenderList )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem
                                                                      {
                                                                          AnswersToExclude = new List<Lookup>
                                                                                             {
                                                                                                 NonResponseLookups.DontKnow
                                                                                             }
                                                                      },
                                                                      new ItemTemplateMetadataItem { TemplateName = "LookupDto" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000068",
                               "GpraPatientGenderSpecificationNote" ),
                               ItemType.Question,
                               ValueType.Specify )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem
                                                                      {
                                                                          AnswersToExclude = new List<Lookup>
                                                                                             {
                                                                                                 NonResponseLookups.DontKnow
                                                                                             }
                                                                      },
                                                                      new ItemTemplateMetadataItem { TemplateName = "String" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000069",
                               "HiSpanicLatinoIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem
                                                                      {
                                                                          AnswersToExclude = new List<Lookup>
                                                                                             {
                                                                                                 NonResponseLookups.DontKnow
                                                                                             }
                                                                      },
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000070",
                               "EthnicGroupCentralAmericanIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem
                                                                      {
                                                                          AnswersToExclude = new List<Lookup>
                                                                                             {
                                                                                                 NonResponseLookups.DontKnow
                                                                                             }
                                                                      },
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000071",
                               "EthnicGroupCubanIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem
                                                                      {
                                                                          AnswersToExclude = new List<Lookup>
                                                                                             {
                                                                                                 NonResponseLookups.DontKnow
                                                                                             }
                                                                      },
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000072",
                               "EthnicGroupDominicanIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem
                                                                      {
                                                                          AnswersToExclude = new List<Lookup>
                                                                                             {
                                                                                                 NonResponseLookups.DontKnow
                                                                                             }
                                                                      },
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000073",
                               "EthnicGroupMexicanIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem
                                                                      {
                                                                          AnswersToExclude = new List<Lookup>
                                                                                             {
                                                                                                 NonResponseLookups.DontKnow
                                                                                             }
                                                                      },
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000074",
                               "EthnicGroupPuertoRicanIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem
                                                                      {
                                                                          AnswersToExclude = new List<Lookup>
                                                                                             {
                                                                                                 NonResponseLookups.DontKnow
                                                                                             }
                                                                      },
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000075",
                               "EthnicGroupSouthAmericanIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem
                                                                      {
                                                                          AnswersToExclude = new List<Lookup>
                                                                                             {
                                                                                                 NonResponseLookups.DontKnow
                                                                                             }
                                                                      },
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000076",
                               "EthnicGroupOtherIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem
                                                                      {
                                                                          AnswersToExclude = new List<Lookup>
                                                                                             {
                                                                                                 NonResponseLookups.DontKnow
                                                                                             }
                                                                      },
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000077",
                               "EthnicGroupSpecificationNote" ),
                               ItemType.Question,
                               ValueType.Specify )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new ItemTemplateMetadataItem { TemplateName = "String" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000078",
                               "RaceBlackAfricanAmericanIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem
                                                                      {
                                                                          AnswersToExclude = new List<Lookup>
                                                                                             {
                                                                                                 NonResponseLookups.DontKnow
                                                                                             }
                                                                      },
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000079",
                               "RaceAsianIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem
                                                                      {
                                                                          AnswersToExclude = new List<Lookup>
                                                                                             {
                                                                                                 NonResponseLookups.DontKnow
                                                                                             }
                                                                      },
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000080",
                               "RaceNativeHawaiianOtherPacificIslanderIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem
                                                                      {
                                                                          AnswersToExclude = new List<Lookup>
                                                                                             {
                                                                                                 NonResponseLookups.DontKnow
                                                                                             }
                                                                      },
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000081",
                               "RaceAlaskaNativeIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem
                                                                      {
                                                                          AnswersToExclude = new List<Lookup>
                                                                                             {
                                                                                                 NonResponseLookups.DontKnow
                                                                                             }
                                                                      },
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000082",
                               "RaceWhiteIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem
                                                                      {
                                                                          AnswersToExclude = new List<Lookup>
                                                                                             {
                                                                                                 NonResponseLookups.DontKnow
                                                                                             }
                                                                      },
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000083",
                               "RaceAmericanIndianIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem
                                                                      {
                                                                          AnswersToExclude = new List<Lookup>
                                                                                             {
                                                                                                 NonResponseLookups.DontKnow
                                                                                             }
                                                                      },
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000084",
                               "DateOfBirth" ),
                               ItemType.Question,
                               ValueType.Specify )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem
                                                                      {
                                                                          AnswersToExclude = new List<Lookup>
                                                                                             {
                                                                                                 NonResponseLookups.DontKnow
                                                                                             }
                                                                      },
                                                                      new ItemTemplateMetadataItem { TemplateName = "DateTime" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000085",
                               "VeteranIndicator" ),
                               ItemType.Question,
                               ValueType.YesOrNoResponse )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new NonResponseTypeMetadataItem (),
                                                                      new ItemTemplateMetadataItem { TemplateName = "Boolean" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                       };
            }
        }

        #endregion
    }
}