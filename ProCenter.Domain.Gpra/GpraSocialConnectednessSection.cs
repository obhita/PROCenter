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

    /// <summary>The gpra social connectedness section class.</summary>
    public class GpraSocialConnectednessSection
    {
        #region Public Properties

        /// <summary>
        /// Gets the gpra social connectedness group.
        /// </summary>
        /// <value>
        /// The gpra social connectedness group.
        /// </value>
        public static List<ItemDefinition> GpraSocialConnectednessGroup
            {
            get
                    {
                return new List<ItemDefinition>
                       {
                           new ItemDefinition (
                               new CodedConcept (
                               CodeSystems.Obhita,
                               "0000202",
                               "AttendVoluntaryGroupsIndicator" ),
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
                               "0000203",
                               "AttendVoluntaryGroupsCount" ),
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
                               "0000204",
                               "AttendReligiousGroupsIndicator" ),
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
                               "0000205",
                               "AttendReligiousGroupsCount" ),
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
                               "0000206",
                               "AttendOtherGroupsIndicator" ),
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
                               "0000207",
                               "AttendOtherGroupsCount" ),
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
                               "0000208",
                               "InteractFamilyFriendsIndicator" ),
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
                               "0000209",
                               "GpraTroubleContact" ),
                               ItemType.Question,
                               ValueType.SocialConnectednessLookups,
                               SocialConnectednessLookups.HavingTroubleContact )
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
                               "0000210",
                               "GpraTroubleContactSpecificationNote" ),
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