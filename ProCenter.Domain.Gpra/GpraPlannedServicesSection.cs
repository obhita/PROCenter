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

    /// <summary>The gpra planned services section class.</summary>
    public class GpraPlannedServicesSection
    {
        #region Public Properties

        /// <summary>
        /// Gets the after care services group.
        /// </summary>
        /// <value>
        /// The after care services group.
        /// </value>
        public static List<ItemDefinition> AfterCareServicesGroup
        {
            get
            {
                return new List<ItemDefinition>
                       {
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000050",
                               "AfterCareContinuingCareIndicator" ),
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
                               "0000051",
                               "AfterCareRelapsePreventionIndicator" ),
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
                               "0000052",
                               "AfterCareRecoveryCoachingIndicator" ),
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
                               "0000053",
                               "AfterCareSelfHelpSupportGroupsIndicator" ),
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
                               "0000054",
                               "AfterCareSpiritualSupportIndicator" ),
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
                               "0000055",
                               "AfterCareOtherIndicator" ),
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
                               "0000056",
                               "AfterCareSpecificationNote" ),
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
                       };
            }
        }

        /// <summary>
        /// Gets the case management services group.
        /// </summary>
        /// <value>
        /// The case management services group.
        /// </value>
        public static List<ItemDefinition> CaseManagementServicesGroup
        {
            get
            {
                return new List<ItemDefinition>
                       {
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000035",
                               "CaseMgmtFamilyServicesIndicator" ),
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
                               "0000036",
                               "CaseMgmtChildCareIndicator" ),
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
                               "0000037",
                               "CaseMgmtPreemploymentIndicator" ),
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
                               "0000038",
                               "CaseMgmtEmploymentCoachingIndicator" ),
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
                               "0000039",
                               "CaseMgmtIndividualServicesCoordinationIndicator" ),
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
                               "0000040",
                               "CaseMgmtTransportationIndicator" ),
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
                               "0000041",
                               "CaseMgmtHivAidsServiceIndicator" ),
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
                               "0000042",
                               "CaseMgmtTransitionalDrugFreeHousingIndicator" ),
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
                               "0000043",
                               "CaseMgmtOtherIndicator" ),
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
                               "0000044",
                               "CaseMgmtSpecificationNote" ),
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
                       };
            }
        }

        /// <summary>
        /// Gets the education services group.
        /// </summary>
        /// <value>
        /// The education services group.
        /// </value>
        public static
            List<ItemDefinition> EducationServicesGroup
        {
            get
            {
                return new List<ItemDefinition>
                       {
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000057",
                               "EducationSaIndicator" ),
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
                               "0000058",
                               "EducationHivAidsIndicator" ),
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
                               "0000059",
                               "EducationOtherIndicator" ),
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
                               "0000060",
                               "EducationSpecificationNote" ),
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
                       };
            }
        }

        /// <summary>
        /// Gets the gpra modality group.
        /// </summary>
        /// <value>
        /// The gpra modality group.
        /// </value>
        public static List<ItemDefinition> GpraModalityGroup
        {
            get
            {
                return new List<ItemDefinition>
                       {
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000008",
                               "ModalityCaseMgmtIndicator" ),
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
                               "0000009",
                               "ModalityDayTreatmentIndicator" ),
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
                               "0000010",
                               "ModalityInpatientHospitalIndicator" ),
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
                               "0000011",
                               "ModalityOutpatientIndicator" ),
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
                               "0000012",
                               "ModalityOutreachIndicator" ),
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
                               "0000013",
                               "ModalityIntensiveOutpatientIndicator" ),
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
                               "0000014",
                               "ModalityMethadoneIndicator" ),
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
                               "0000015",
                               "ModalityResidentialRehabilitationIndicator" ),
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
                               "0000016",
                               "ModalityGpraDetoxificationLocation" ),
                               ItemType.Question,
                               ValueType.PlannedServicesLookups,
                               PlannedServicesLookups.DetoxificationDropdown )
                           {
                               ItemMetadata = new ItemMetadata
                                              {
                                                  MetadataItems = new List<IMetadataItem>
                                                                  {
                                                                      new ItemTemplateMetadataItem { TemplateName = "LookupDto" },
                                                                      new RequiredForCompletenessMetadataItem ( CompletenessCategory.Report ),
                                                                  }
                                              }
                           },
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000017",
                               "ModalityAfterCareIndicator" ),
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
                               "0000018",
                               "ModalityRecoverySupportIndicator" ),
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
                               "0000019",
                               "ModalityOtherSpecificationIndicator" ),
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
                               "0000020",
                               "ModalitySpecificationNote" ),
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
                       };
            }
        }

        /// <summary>
        /// Gets the medical services group.
        /// </summary>
        /// <value>
        /// The medical services group.
        /// </value>
        public static List<ItemDefinition> MedicalServicesGroup
        {
            get
            {
                return new List<ItemDefinition>
                       {
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000045",
                               "MedicalCareIndicator" ),
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
                               "0000046",
                               "MedicalAlcoholDrugTestIndicator" ),
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
                               "0000047",
                               "MedicalHivAidsSupportAndTestingIndicator" ),
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
                               "0000048",
                               "MedicalOtherIndicator" ),
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
                               "0000049",
                               "MedicalSpecificationNote" ),
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
                       };
            }
        }

        /// <summary>
        /// Gets the peer to peer recovery support services group.
        /// </summary>
        /// <value>
        /// The peer to peer recovery support services group.
        /// </value>
        public static List<ItemDefinition> PeerToPeerRecoverySupportServicesGroup
        {
            get
            {
                return new List<ItemDefinition>
                       {
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000061",
                               "PeerToPeerRecoverySupportCoachingIndicator" ),
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
                               "0000062",
                               "PeerToPeerRecoverySupportHousingIndicator" ),
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
                               "0000063",
                               "PeerToPeerRecoverySupportAlcholDrugFreeActivitiesIndicator" ),
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
                               "0000064",
                               "PeerToPeerRecoverySupportInformationReferralIndicator" ),
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
                               "0000065",
                               "PeerToPeerRecoverySupportOtherIndicator" ),
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
                               "0000066",
                               "PeerToPeerRecoverySupportSpecificationNote" ),
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
                       };
            }
        }

        /// <summary>
        /// Gets the treatment services group.
        /// </summary>
        /// <value>
        /// The treatment services group.
        /// </value>
        public static List<ItemDefinition> TreatmentServicesGroup
        {
            get
            {
                return new List<ItemDefinition>
                       {
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000021",
                               "TreatmentScreeningIndicator" ),
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
                               "0000022",
                               "TreatmentBriefInterventionIndicator" ),
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
                               "0000023",
                               "TreatmentBriefTreatmentIndicator" ),
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
                               "0000024",
                               "TreatmentReferralToTreatmentIndicator" ),
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
                               "0000025",
                               "TreatmentAssessmentIndicator" ),
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
                               "0000026",
                               "TreatmentRecoveryPlanningIndicator" ),
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
                               "0000027",
                               "TreatmentIndividualCounselingIndicator" ),
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
                               "0000028",
                               "TreatmentGroupCounselingIndicator" ),
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
                               "0000029",
                               "TreatmentFamilyCounselingIndicator" ),
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
                               "0000030",
                               "TreatmentCooccuringTreatmentIndicator" ),
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
                               "0000031",
                               "TreatmentPharmacologicalInterventionsIndicator" ),
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
                               "0000032",
                               "TreatmentHivAidsCounselingIndicator" ),
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
                               "0000033",
                               "TreatmentOtherSpecificationIndicator" ),
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
                               "0000034",
                               "TreatmentSpecificationNote" ),
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
                       };
            }
        }

        #endregion
    }
}