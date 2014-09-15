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

namespace ProCenter.Service.Handler.Assessment
{
    #region Using Statements

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using global::AutoMapper;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Lookups;
    using ProCenter.Domain.CommonModule.Lookups;
    using ProCenter.Service.Handler.Common;
    using ProCenter.Service.Message.Assessment;
    using ProCenter.Service.Message.Common.Lookups;

    #endregion

    /// <summary>Gets a section dto.</summary>
    public class GetSectionDtoByKeyRequestHandler :
        ServiceRequestHandler<GetSectionDtoByKeyRequest, GetSectionDtoByKeyResponse>
    {
        #region Fields

        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;

        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GetSectionDtoByKeyRequestHandler" /> class.
        /// </summary>
        /// <param name="assessmentDefinitionRepository">The assessment definition repository.</param>
        /// <param name="assessmentInstanceRepository">The assessment instance repository.</param>
        public GetSectionDtoByKeyRequestHandler (
            IAssessmentDefinitionRepository assessmentDefinitionRepository,
            IAssessmentInstanceRepository assessmentInstanceRepository )
        {
            _assessmentDefinitionRepository = assessmentDefinitionRepository;
            _assessmentInstanceRepository = assessmentInstanceRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( GetSectionDtoByKeyRequest request, GetSectionDtoByKeyResponse response )
        {
            var assessmentInstance = _assessmentInstanceRepository.GetByKey ( request.Key );
            var assessmentDefinition = _assessmentDefinitionRepository.GetByKey ( assessmentInstance.AssessmentDefinitionKey );

            var sectionDto = new SectionDto
                             {
                                 Key = assessmentInstance.Key,
                                 AssessmentName = assessmentDefinition.CodedConcept.Name,
                                 Items = new List<ItemDto> (),
                             };
            IContainItemDefinitions itemContainer;
            if ( request.SectionItemDefinitionCode == null )
            {
                itemContainer = (IContainItemDefinitions)assessmentDefinition.ItemDefinitions.FirstOrDefault ( i => i.ItemType == ItemType.Section ) ?? assessmentDefinition;
            }
            else
            {
                itemContainer = assessmentDefinition.GetItemDefinitionByCode ( request.SectionItemDefinitionCode );
            }

            if ( itemContainer == null )
            {
                throw new InvalidOperationException ( "Section does not exist." );
            }
            sectionDto.ItemDefinitionCode = itemContainer.CodedConcept.Code;
            MapItems ( sectionDto, itemContainer, assessmentInstance );

            response.DataTransferObject = sectionDto;
        }

        private static ItemDto CreateQuestion ( ItemDefinition itemDefinition, AssessmentInstance assessmentInstance )
        {
            var itemInstance =
                assessmentInstance.ItemInstances.FirstOrDefault (
                    i => i.ItemDefinitionCode == itemDefinition.CodedConcept.Code );
            var item = new ItemDto
                       {
                           Metadata = itemDefinition.ItemMetadata,
                           Key = assessmentInstance.Key,
                           ItemDefinitionCode = itemDefinition.CodedConcept.Code,
                           ItemDefinitionName = itemDefinition.CodedConcept.Name,
                           Options = Mapper.Map<IEnumerable<Lookup>, IEnumerable<LookupDto>> ( itemDefinition.Options ),
                           ItemType = itemDefinition.ItemType.CodedConcept.Code
                       };
            if ( itemInstance == null || itemInstance.Value == null )
            {
                item.Value = null;
            }
            else if ( itemInstance.Value is Lookup )
            {
                item.Value = Mapper.Map<Lookup, LookupDto> ( itemInstance.Value as Lookup );
            }
            else if (itemInstance.Value is IEnumerable && !(itemInstance.Value is string))
            {
                item.Value = Mapper.Map ( itemInstance.Value, itemInstance.Value.GetType (), typeof(IEnumerable<LookupDto>) );
            }
            else
            {
                item.Value = itemInstance.Value;
            }
            return item;
        }

        private static void MapItems ( IContainItems itemContainer, IContainItemDefinitions itemDefinitionContainer, AssessmentInstance assessmentInstance )
        {
            foreach ( var itemDefinition in itemDefinitionContainer.ItemDefinitions )
            {
                if ( itemDefinition.ItemType == ItemType.Question )
                {
                    itemContainer.Items.Add ( CreateQuestion ( itemDefinition, assessmentInstance ) );
                }
                else if ( itemDefinition.ItemType == ItemType.Group || itemDefinition.ItemType == ItemType.Section )
                {
                    var groupItemDto = new ItemDto
                                       {
                                           Metadata = itemDefinition.ItemMetadata,
                                           Key = assessmentInstance.Key,
                                           ItemDefinitionCode = itemDefinition.CodedConcept.Code,
                                           ItemDefinitionName = itemDefinition.CodedConcept.Name,
                                           Items = new List<ItemDto> (),
                                           ItemType = itemDefinition.ItemType.CodedConcept.Code
                                       };

                    MapItems ( groupItemDto, itemDefinition, assessmentInstance );

                    itemContainer.Items.Add ( groupItemDto );
                }
            }
        }

        #endregion
    }
}