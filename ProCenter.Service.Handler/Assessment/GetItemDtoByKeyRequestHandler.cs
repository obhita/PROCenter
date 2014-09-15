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

    /// <summary>Gets a Item dto.</summary>
    public class GetItemDtoByKeyRequestHandler :
        ServiceRequestHandler<GetItemDtoByKeyRequest, GetItemDtoByKeyResponse>
    {
        #region Fields

        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GetItemDtoByKeyRequestHandler" /> class.
        /// </summary>
        /// <param name="assessmentDefinitionRepository">The assessment definition repository.</param>
        public GetItemDtoByKeyRequestHandler (
            IAssessmentDefinitionRepository assessmentDefinitionRepository)
        {
            _assessmentDefinitionRepository = assessmentDefinitionRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        protected override void Handle ( GetItemDtoByKeyRequest request, GetItemDtoByKeyResponse response )
        {
            var assessmentDefinition = _assessmentDefinitionRepository.GetByKey(request.AssessmentDefinitionKey);

            ItemDefinition itemDefinition;
            if ( request.ItemDefinitionCode == null )
            {
                itemDefinition = assessmentDefinition.ItemDefinitions.FirstOrDefault(i => i.ItemType == ItemType.Question);
            }
            else
            {
                itemDefinition = assessmentDefinition.GetItemDefinitionByCode(request.ItemDefinitionCode);
            }

            if (itemDefinition == null)
            {
                throw new InvalidOperationException ( "Item does not exist." );
            }
            var itemDto = CreateQuestion(itemDefinition);
            response.DataTransferObject = itemDto;
        }

        private static ItemDto CreateQuestion ( ItemDefinition itemDefinition )
        {
            var item = new ItemDto
                       {
                           Metadata = itemDefinition.ItemMetadata,
                           ItemDefinitionCode = itemDefinition.CodedConcept.Code,
                           ItemDefinitionName = itemDefinition.CodedConcept.Name,
                           Options = Mapper.Map<IEnumerable<Lookup>, IEnumerable<LookupDto>> ( itemDefinition.Options ),
                           ItemType = itemDefinition.ItemType.CodedConcept.Code
                       };
            return item;
        }

        #endregion
    }
}