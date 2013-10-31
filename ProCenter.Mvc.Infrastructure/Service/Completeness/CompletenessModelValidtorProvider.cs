namespace ProCenter.Mvc.Infrastructure.Service.Completeness
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Domain.AssessmentModule.Lookups;
    using Pillar.Common.Metadata;
    using ProCenter.Infrastructure.Service.Completeness;
    using ProCenter.Service.Message.Assessment;
    using ProCenter.Service.Message.Common.Lookups;
    using ProCenter.Service.Message.Metadata;

    #endregion

    public class CompletenessModelValidtorProvider : ModelValidatorProvider
    {
        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
        {
            if (metadata.PropertyName != null && context is ViewContext && context.Controller.ViewData.Model is IAssessmentDto)
            {
                var viewContext = context as ViewContext;
                if (viewContext.ViewData != null)
                {
                    if ((metadata.ContainerType == typeof (ItemDto) && metadata.PropertyName == "Value") ||
                        (metadata.ContainerType == typeof (LookupDto) && metadata.PropertyName == "Code")) // only care about value
                    {
                        var sectionDto = context.Controller.ViewData.Model as SectionDto;
                        if (sectionDto != null)
                        {
                            var propertyParts = viewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(metadata.PropertyName).Split('.');
                            //System.Diagnostics.Debug.WriteLine(viewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(metadata.PropertyName), "PropertyParts");
                            if (propertyParts.Length == 2)
                            {
                                var index = propertyParts[0].IndexOf("_Value", System.StringComparison.Ordinal);
                                if (index != -1)
                                {
                                    var code = propertyParts[0].Substring(0, index);
                                    var itemDto = GetItemDtoByCode(sectionDto, code);
                                    if (itemDto != null)
                                    {
                                        var metadataItemDto = itemDto.Metadata.FindMetadataItem<RequiredForCompletenessMetadataItem>();
                                        if (metadataItemDto != null)
                                        {
                                            yield return new CompletenessModelValidator(metadata, context, metadataItemDto.CompletenessCategory);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        private static ItemDto GetItemDtoByCode(IContainItems sectionDto, string code)
        {
            foreach (var itemDto in sectionDto.Items)
            {
                if (itemDto.ItemDefinitionCode == code && itemDto.ItemType == ItemType.Question.CodedConcept.Code)
                {
                    return itemDto;
                }
                if (itemDto.Items != null)
                {
                    foreach (
                        var childItemDto in
                            itemDto.Items.Where(childItemDto => childItemDto.ItemDefinitionCode == code && childItemDto.ItemType == ItemType.Question.CodedConcept.Code))
                    {
                        return childItemDto;
                    }
                }
            }
            return null;
        }
    }
}