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

namespace ProCenter.Mvc.Infrastructure.Service.Completeness
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Domain.AssessmentModule.Lookups;

    using ProCenter.Domain.AssessmentModule.Metadata;
    using ProCenter.Service.Message.Assessment;
    using ProCenter.Service.Message.Common.Lookups;
    using ProCenter.Service.Message.Metadata;

    #endregion

    /// <summary>The completeness model validtor provider class.</summary>
    public class CompletenessModelValidtorProvider : ModelValidatorProvider
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets a list of validators.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// A list of validators.
        /// </returns>
        public override IEnumerable<ModelValidator> GetValidators ( ModelMetadata metadata, ControllerContext context )
        {
            if ( metadata.PropertyName != null && 
                context is ViewContext && 
                (context.Controller.ViewData.Model is IValidateCompleteness || context.Controller.ViewData.Model is SectionDto ))
            {
                var viewContext = context as ViewContext;
                if ( viewContext.ViewData != null )
                {
                    if ( ( metadata.ContainerType == typeof(ItemDto) && metadata.PropertyName == "Value" ) ||
                         ( metadata.ContainerType == typeof(LookupDto) && metadata.PropertyName == "Code" ) ) 
                    {
                        // only care about value
                        var sectionDto = context.Controller.ViewData.Model is IValidateCompleteness ? (context.Controller.ViewData.Model as IValidateCompleteness).CurrentSectionDto
                            : context.Controller.ViewData.Model as SectionDto;
                        if ( sectionDto != null )
                        {
                            var propertyParts = viewContext.ViewData.TemplateInfo.GetFullHtmlFieldName ( metadata.PropertyName ).Split ( '.' );
                            if ( propertyParts.Length == 2 )
                            {
                                var index = propertyParts[0].IndexOf ( "_Value", StringComparison.Ordinal );
                                if ( index != -1 )
                                {
                                    var code = propertyParts[0].Substring ( 0, index );
                                    var itemDto = GetItemDtoByCode ( sectionDto, code );
                                    if ( itemDto != null )
                                    {
                                        var metadataItemDto = itemDto.Metadata.FindMetadataItem<RequiredForCompletenessMetadataItem> ();
                                        if ( metadataItemDto != null )
                                        {
                                            yield return new CompletenessModelValidator ( metadata, context, metadataItemDto.CompletenessCategory );
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Methods

        private static ItemDto GetItemDtoByCode ( IContainItems sectionDto, string code )
        {
            foreach ( var itemDto in sectionDto.Items )
            {
                if ( itemDto.ItemDefinitionCode == code && itemDto.ItemType == ItemType.Question.CodedConcept.Code )
                {
                    return itemDto;
                }
                if ( itemDto.Items != null )
                {
                    foreach (
                        var childItemDto in
                            itemDto.Items.Where ( childItemDto => childItemDto.ItemDefinitionCode == code && childItemDto.ItemType == ItemType.Question.CodedConcept.Code ) )
                    {
                        return childItemDto;
                    }
                    foreach (var containerDto in itemDto.Items.Where(i => i.ItemType != ItemType.Question.CodedConcept.Code).OfType<IContainItems> ())
                    {
                        var childItem = GetItemDtoByCode ( containerDto, code );
                        if ( childItem != null )
                        {
                            return childItem;
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}