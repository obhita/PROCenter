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

namespace ProCenter.Service.Handler.Assessment
{
    #region Using Statements

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.AssessmentModule;
    using Domain.CommonModule.Lookups;
    using Infrastructure.Service.Completeness;

    using ProCenter.Domain.CommonModule;

    using Service.Message.Assessment;
    using Service.Message.Common.Lookups;

    #endregion

    /// <summary>The save assessment item request handler class.</summary>
    internal class SaveAssessmentItemRequestHandler :
        ServiceRequestHandler<SaveAssessmentItemRequest, SaveAssessmentItemResponse>
    {
        #region Fields

        private readonly IAssessmentCompletenessManager _assessmentCompletenessManager;

        private readonly ILookupProvider _lookupProvider;

        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;
        private readonly IAssessmentInstanceRepository _assessmentInstanceRepository;

        #endregion

        #region Constructors and Destructors

        public SaveAssessmentItemRequestHandler ( IAssessmentDefinitionRepository assessmentDefinitionRepository,
            IAssessmentInstanceRepository assessmentInstanceRepository,
            IAssessmentCompletenessManager assessmentCompletenessManager,
            ILookupProvider lookupProvider)
        {
            _assessmentDefinitionRepository = assessmentDefinitionRepository;
            _assessmentInstanceRepository = assessmentInstanceRepository;
            _assessmentCompletenessManager = assessmentCompletenessManager;
            _lookupProvider = lookupProvider;
        }

        #endregion

        #region Methods

        protected override void Handle ( SaveAssessmentItemRequest request, SaveAssessmentItemResponse response )
        {
            var assessmentInstance = _assessmentInstanceRepository.GetByKey ( request.Key );
            var assessmentDefinition = _assessmentDefinitionRepository.GetByKey ( assessmentInstance.AssessmentDefinitionKey );

            var item = assessmentInstance.ItemInstances.FirstOrDefault ( i => i.ItemDefinitionCode == request.Item.ItemDefinitionCode );
            var itemDefinition = assessmentDefinition.GetItemDefinitionByCode ( request.Item.ItemDefinitionCode );
            var value = request.Item.Value;
            if ( value is LookupDto )
            {
                var lookupDto = value as LookupDto;
                if ( string.IsNullOrWhiteSpace ( lookupDto.Code ) )
                {
                    value = null;
                }
                else
                {
                    value = itemDefinition.Options.Single ( l => l.CodedConcept.Code == lookupDto.Code );
                }
            }
            else if ( value is IEnumerable<LookupDto> )
            {
                var lookupType = itemDefinition.Options.ElementAt ( 0 ).GetType ();
                var lookupList = (IList)Activator.CreateInstance ( typeof(List<>).MakeGenericType ( lookupType ) );
                foreach (var lookupDto in (value as IEnumerable<LookupDto>).Where ( l => !string.IsNullOrWhiteSpace(l.Code) ))
                {
                    lookupList.Add(itemDefinition.Options.Single(l => l.CodedConcept.Code == lookupDto.Code));
                }
                value = lookupList.Count == 0 ? null : lookupList;
            }
            var hasItemUpdated = HasItemUpdated ( item, value );
            if ( hasItemUpdated )
            {
                if ( value != null )
                {
                    value = string.IsNullOrEmpty ( value.ToString () ) ? null : value;
                }
                assessmentInstance.UpdateItem ( itemDefinition, value );
            }
            var assessmentCompleteness = assessmentInstance.CalculateCompleteness ();
            response.CanSubmit = !assessmentInstance.IsSubmitted && assessmentCompleteness.IsComplete;
            var containingSection = assessmentDefinition.GetContainingSection ( itemDefinition );
            if ( containingSection is AssessmentDefinition )
            {
                response.PercentComplete = assessmentCompleteness.PercentComplete;
            }
            else if(containingSection is ItemDefinition)
            {
                response.PercentComplete = _assessmentCompletenessManager.CalculateCompleteness ( assessmentInstance, containingSection as ItemDefinition ).PercentComplete;
            }
        }

        private static bool HasItemUpdated ( ItemInstance item, object value )
        {
            if ( item == null || item.Value == null )
            {
                if ( value is Lookup )
                {
                    return true;
                }
                if ( value == null )
                {
                    return false;
                }
                return !string.IsNullOrEmpty ( value.ToString () );
            }
            if ( value == null || string.IsNullOrEmpty ( value.ToString () ) )
            {
                return true;
            }
            if ( item.Value is Lookup )
            {
                return !( item.Value as Lookup ).Equals ( value as Lookup );
            }
            return !Equals ( item.Value, value );
        }

        #endregion
    }
}