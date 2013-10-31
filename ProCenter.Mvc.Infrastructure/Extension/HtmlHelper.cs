namespace ProCenter.Mvc.Infrastructure.Extension
{
    #region

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Resources;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.UI;
    using Common;
    using Pillar.Common.Metadata;
    using Pillar.Common.Metadata.Dtos;
    using Pillar.Security.AccessControl;
    using ProCenter.Infrastructure.Extensions;
    using ProCenter.Service.Message.Assessment;
    using ProCenter.Service.Message.Attribute;
    using ProCenter.Service.Message.Common;
    using ProCenter.Service.Message.Common.Lookups;
    using ProCenter.Service.Message.Metadata;

    #endregion

    /// <summary>
    ///     Html helper extensions.
    /// </summary>
    public static class HtmlHelpers
    {
        #region Static Fields

        public static readonly string ValidationSummaryWarningCssClassName = "validation-summary-warnings";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Checks the box list for model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="items">The items.</param>
        /// <param name="value">The value.</param>
        /// <param name="text">The text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxListForModel<T>(this HtmlHelper htmlHelper,
                                                            IEnumerable<T> items,
                                                            string value,
                                                            string text,
                                                            object htmlAttributes = null)
        {
            var propertyName = htmlHelper.ViewData.ModelMetadata.PropertyName;

            var enumerableModel = htmlHelper.ViewData.Model as IEnumerable;

            //Convert selected value list to a List<string> for easy manipulation
            var selectedValues = enumerableModel != null ? enumerableModel.OfType<T>() : Enumerable.Empty<T>();

            //Create div
            var divTag = new TagBuilder("div");
            divTag.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
            divTag.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(propertyName, htmlHelper.ViewData.ModelMetadata));
            const string labelAndCheckboxDiv = "<label tabIndex=\"{4}\"><input type=\"checkbox\" name=\"{0}\" id=\"{0}_{1}\" " +
                                               "value=\"{1}\" {2} />{3}</label>";

            var innerHtmlBuilder = new StringBuilder();
            foreach (var item in items)
            {
                innerHtmlBuilder.Append(String.Format(labelAndCheckboxDiv,
                                                      propertyName,
                                                      DataBinder.Eval(item, value),
                                                      selectedValues.Contains(item) ? "checked=\"checked\"" : "",
                                                      DataBinder.Eval(item, text),
                                                      0));
            }
            divTag.InnerHtml = innerHtmlBuilder.ToString();
            return MvcHtmlString.Create(divTag.ToString());
        }

        private class ViewDataContainer : IViewDataContainer
        {
            public ViewDataDictionary ViewData { get; set; }
        }

        /// <summary>
        ///     Gets the items to render.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>
        ///     List of <see cref="ItemDto" /> to render.
        /// </returns>
        public static IEnumerable<ItemDto> GetItemsToRender(this HtmlHelper html, IContainItems itemContainer)
        {
            return
                itemContainer.Items.Where(item => item.Metadata == null || !item.Metadata.MetadataItemExists<HiddenMetadataItem>(mi => mi.IsHidden))
                             .OrderBy(item =>
                                 {
                                     var metaItem = item.Metadata == null ? null : item.Metadata.FindMetadataItem<DisplayOrderMetadataItem>();
                                     return metaItem == null ? 0 : metaItem.Order;
                                 });
        }

        /// <summary>
        ///     Gets the lookup category.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static string GetLookupCategory(this HtmlHelper htmlHelper, string propertyName)
        {
            var metadata = ModelMetadata.FromStringExpression(propertyName, htmlHelper.ViewData);
            if (metadata.AdditionalValues.ContainsKey(LookupCategoryAttribute.LookupCategory))
            {
                var category = metadata.AdditionalValues[LookupCategoryAttribute.LookupCategory] as string;
                return category;
            }
            return propertyName;
        }

        /// <summary>
        ///     Gets the lookup options for category.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public static IEnumerable<LookupDto> GetLookupOptionsForCategory(this HtmlHelper htmlHelper, string category)
        {
            var selectListItems = htmlHelper.ViewData[category + "LookupItems"] as IList<LookupDto>;
            if (selectListItems == null)
            {
            }
            return selectListItems;
        }

        /// <summary>
        ///     Gets the question groups.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="propertyMetadata">The property metadata.</param>
        /// <returns></returns>
        public static IEnumerable<IQuestionGroup> GetQuestionGroups(this HtmlHelper htmlHelper, ModelMetadata propertyMetadata)
        {
            if (propertyMetadata.AdditionalValues.ContainsKey(QuestionGroupAttribute.QuestionGroup))
            {
                var groups =
                    (propertyMetadata.AdditionalValues[QuestionGroupAttribute.QuestionGroup] as
                     IEnumerable<IQuestionGroup>).OrderBy(qg => qg.ApplyOrder);
                return groups;
            }
            return Enumerable.Empty<IQuestionGroup>();
        }

        /// <summary>
        ///     Gets the type of the question.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="propertyMetadata">The property metadata.</param>
        /// <returns></returns>
        public static string GetQuestionType(this HtmlHelper htmlHelper, ModelMetadata propertyMetadata)
        {
            if (propertyMetadata.AdditionalValues.ContainsKey(QuestionAttribute.Question))
            {
                return propertyMetadata.AdditionalValues[QuestionAttribute.Question].ToString();
            }
            return QuestionType.GeneralQuestion.ToString();
        }

        /// <summary>
        ///     Gets the resource.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="sectionDto">The section dto.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>The resource for the section dto.</returns>
        public static string GetResource(this HtmlHelper htmlHelper, SectionDto sectionDto, string suffix = null)
        {
            var resourceManager = htmlHelper.ViewData["ResourceManager"] as ResourceManager;
            var resource = string.Empty;
            if (resourceManager != null)
            {
                resource =
                    resourceManager.GetString(SharedStringNames.ResourceKeyPrefix + sectionDto.AssessmentDefinitionCode +
                                              (suffix == null ? "" : SharedStringNames.ResourceKeyPrefix + suffix));
            }
            return resource;
        }

        /// <summary>
        ///     Gets the resource.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <returns>The named resource.</returns>
        public static string GetResource(this HtmlHelper htmlHelper, string name)
        {
            var resourceManager = htmlHelper.ViewData["ResourceManager"] as ResourceManager;
            var resource = string.Empty;
            if (resourceManager != null)
            {
                resource =
                    resourceManager.GetString(name);
            }
            return resource;
        }

        /// <summary>
        ///     Gets the resource.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="itemDto">The item dto.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>The resource for the item dto.</returns>
        public static string GetResource(this HtmlHelper htmlHelper, ItemDto itemDto, string suffix = null)
        {
            var resourceManager = htmlHelper.ViewData["ResourceManager"] as ResourceManager;
            var resource = string.Empty;
            if (resourceManager != null)
            {
                resource =
                    resourceManager.GetString(SharedStringNames.ResourceKeyPrefix + itemDto.ItemDefinitionCode +
                                              (suffix == null ? "" : SharedStringNames.ResourceKeyPrefix + suffix));
            }
            return resource;
        }

        /// <summary>
        ///     Gets the resource.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="lookupDto">The lookup dto.</param>
        /// <returns>The resource for the lookup.</returns>
        public static string GetResource(this HtmlHelper htmlHelper, LookupDto lookupDto)
        {
            var resourceManager = htmlHelper.ViewData["ResourceManager"] as ResourceManager;
            var resource = string.Empty;
            if (resourceManager != null)
            {
                resource = resourceManager.GetString("_" + lookupDto.Code);
            }
            return resource;
        }

        public static string GetAssessmentResource(this HtmlHelper htmlHelper, string assessmentCode)
        {
            var resourcesManager = htmlHelper.ViewData["ResourcesManager"] as IResourcesManager;
            var resourceManager = resourcesManager.GetResourceManagerByName(assessmentCode);
            var resource = string.Empty;
            if (resourceManager != null)
            {
                resource = resourceManager.GetString("_" + assessmentCode);
            }
            return resource;
        }

        /// <summary>
        ///     Determines whether [has check all attribute] [the specified HTML helper].
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <returns>
        ///     <c>true</c> if [has check all attribute] [the specified HTML helper]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasCheckAllAttribute(this HtmlHelper htmlHelper)
        {
            return htmlHelper.ViewData.ModelMetadata.AdditionalValues.ContainsKey(CheckAllAttribute.CheckAll);
        }

        public static bool IsLookupProperty(this HtmlHelper htmlHelper, ModelMetadata propertyMetadata)
        {
            return propertyMetadata.ModelType == typeof (LookupDto) ||
                   typeof (IEnumerable<LookupDto>).IsAssignableFrom(propertyMetadata.ModelType);
        }

        /// <summary>
        ///     Reads the only editor.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="additionalViewData">The additional view data.</param>
        /// <returns></returns>
        public static MvcHtmlString ReadOnlyEditor(this HtmlHelper html, string expression, object additionalViewData = null)
        {
            return ReadOnlyEditor(html, expression, null, null, additionalViewData);
        }

        /// <summary>
        ///     Reads the only editor.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="additionalViewData">The additional view data.</param>
        /// <returns></returns>
        public static MvcHtmlString ReadOnlyEditor(this HtmlHelper html, string expression, string templateName, object additionalViewData = null)
        {
            return ReadOnlyEditor(html, expression, templateName, null, additionalViewData);
        }

        /// <summary>
        ///     Reads the only editor.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="htmlFieldName">Name of the HTML field.</param>
        /// <param name="additionalViewData">The additional view data.</param>
        /// <returns></returns>
        public static MvcHtmlString ReadOnlyEditor(this HtmlHelper html, string expression, string templateName, string htmlFieldName, object additionalViewData = null)
        {
            var mvcHtmlString = html.Editor(expression, templateName, htmlFieldName, additionalViewData);
            var htmlString = mvcHtmlString.ToHtmlString();
            if (htmlString.Contains("<input ") || htmlString.Contains("<select "))
            {
                return new MvcHtmlString(htmlString.Replace("<input ", "<input disabled=\"disabled\"")
                                                   .Replace("<select ", "<select disabled=\"disabled\""));
            }
            return mvcHtmlString;
        }

        public static IEnumerable<ModelMetadata> GetPropertiesToRender(this HtmlHelper html)
        {
            return
                html.ViewData.ModelMetadata.Properties.Where(p => p.ShowForEdit)
                    .OrderBy(meta => meta.Order);
        }

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, string forAttributeValue)
        {
            var mvcHtmlString = html.LabelFor(expression, labelText).ToString();
            mvcHtmlString = ReplaceAttribute(mvcHtmlString, "for", forAttributeValue);
            return new MvcHtmlString(mvcHtmlString);
        }

        private static string ReplaceAttribute(string mvcHtmlString, string attribute, string attributeValue)
        {
            attribute = attribute + "=\"";
            var start = mvcHtmlString.IndexOf(attribute, StringComparison.OrdinalIgnoreCase);
            if (start > -1)
            {
                var end = mvcHtmlString.IndexOf("\"", start + attribute.Length, StringComparison.OrdinalIgnoreCase);
                var oldForAttributePair = mvcHtmlString.Substring(start, end - start + 1);
                mvcHtmlString = mvcHtmlString.Replace(oldForAttributePair, attribute + attributeValue + "\"");
            }
            return mvcHtmlString;
        }

        public static MvcHtmlString EditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string templateName,
                                                              string idAttributeValue, object viewData)
        {
            var mvcHtmlString = EditorExtensions.EditorFor(html, expression, templateName, idAttributeValue, viewData).ToString();
            mvcHtmlString = ReplaceAttribute(mvcHtmlString, "id", idAttributeValue);
            return new MvcHtmlString(mvcHtmlString);
        }

        public static MvcHtmlString EditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string templateName,
                                                              string idAttributeValue)
        {
            return html.EditorFor(expression, templateName, idAttributeValue, null);
        }

        public static bool CanAccess(this HtmlHelper htmlHelper, string controllerName, string actionName, string httpMethod = "GET")
        {
            controllerName = "ProCenter.Mvc.Controllers." + controllerName.ToFirstLetterUpper() + "Controller";
            var accessControlManager = DependencyResolver.Current.GetService<IAccessControlManager>();
            var resourceRequest = new ResourceRequest
                {
                    controllerName,
                    actionName.ToFirstLetterUpper(),
                    httpMethod.ToUpper()
                };
            var result = accessControlManager.CanAccess(resourceRequest);
            //Debug.WriteLine("*** {0} access to {1}", result ? "has" : "has NOT", resourceRequest);
            return result;
        }

        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues,
                                                     object htmlAttributes)
        {
            MvcHtmlString result;
            if (htmlHelper.CanAccess(controllerName, actionName)) // HttpMothed always be get
            {
                result = htmlHelper.ActionLink(linkText, actionName, controllerName,
                                               new RouteValueDictionary(routeValues),
                                               HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            }
            else
            {
                result = new MvcHtmlString(string.Empty);
            }
            return result;
        }

        public static string PropertyNameFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            var fullName = html.NameFor ( expression ).ToString ();
            return !string.IsNullOrWhiteSpace ( fullName ) ? fullName.Split ( '.' ).Last () : fullName;
        }

        public static SelectList ToSelectList<TEnum> ( this TEnum enumObj )
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var values = from TEnum e in Enum.GetValues ( typeof(TEnum) )
                         select new {Id = e, Name = e.ToString ()};
            return new SelectList ( values, "Id", "Name", enumObj );
        }

        #endregion
    }
}