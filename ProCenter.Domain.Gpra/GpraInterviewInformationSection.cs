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

    /// <summary>The gpra interview information section class.</summary>
    public class GpraInterviewInformationSection
    {
        #region Public Properties

        /// <summary>
        /// Gets the gpra interview information group.
        /// </summary>
        /// <value>
        /// The gpra interview information group.
        /// </value>
        public static List<ItemDefinition> GpraInterviewInformationGroup
        {
            get
            {
                return new List<ItemDefinition>
                       {
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000001",
                               "ContractGrantId" ),
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
                               "0000002",
                               "GpraPatientType" ),
                               ItemType.Question,
                               ValueType.InterviewInformationLookups,
                               InterviewInformationLookups.PatientType )
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
                               "0000003",
                               "GpraInterviewType" ),
                               ItemType.Question,
                               ValueType.InterviewInformationLookups,
                               InterviewInformationLookups.InterviewType )
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
                               "0000004",
                               "ConductedInterviewIndicator" ),
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
                               "0000005",
                               "MethamphetaminePatientIndicator" ),
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
                               "0000006",
                               "CooccuringMhSaScreenerIndicator" ),
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
                               "0000007",
                               "PositiveCooccuringMhSaScreenerIndicator" ),
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
                       };
            }
        }

        #endregion
    }
}