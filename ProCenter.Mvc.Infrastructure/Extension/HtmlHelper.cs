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

namespace ProCenter.Mvc.Infrastructure.Extension
{
    #region Using Statements

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Resources;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.UI;

    using Pillar.Common.Metadata;
    using Pillar.Security.AccessControl;

    using ProCenter.Common;
    using ProCenter.Infrastructure.Extensions;
    using ProCenter.Mvc.Infrastructure.Service;
    using ProCenter.Service.Message.Assessment;
    using ProCenter.Service.Message.Attribute;
    using ProCenter.Service.Message.Common;
    using ProCenter.Service.Message.Common.Lookups;
    using ProCenter.Service.Message.Metadata;

    #endregion

    /// <summary>Html helper extensions.</summary>
    public static class HtmlHelpers
    {
        #region Static Fields

        public static readonly string ValidationSummaryWarningCssClassName = "validation-summary-warnings";

        #endregion

        #region Public Methods and Operators

        /// <summary>Determines whether this instance can access the specified HTML helper.</summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <returns><c>True</c> if access allowed, otherwise <c>False</c></returns>
        /// <exception cref="System.InvalidOperationException">Unknown controller:  + controllerName.</exception>
        public static bool CanAccess ( this HtmlHelper htmlHelper, string controllerName, string actionName, string httpMethod = "GET" )
        {
            Type controllerType = null;
            var controllerTypeCache = ControllerBuilder.Current.GetControllerFactory () as ExposedTypeCacheControllerFactory;
            if ( controllerTypeCache != null )
            {
                controllerType = controllerTypeCache.GetControllerTypeByName ( controllerName );
            }
            if ( controllerType == null )
            {
                var controllerMapping = GlobalConfiguration.Configuration.Services.GetHttpControllerSelector ().GetControllerMapping ();
                controllerType = controllerMapping.ContainsKey ( controllerName ) ? controllerMapping[controllerName].ControllerType : null;
            }
            if ( controllerType == null )
            {
                throw new InvalidOperationException ( "Unknown controller: " + controllerName );
            }
            controllerName = controllerType.FullName;
            var accessControlManager = DependencyResolver.Current.GetService<IAccessControlManager> ();
            var resourceRequest = new ResourceRequest
                                  {
                                      controllerName,
                                      actionName.ToFirstLetterUpper (),
                                      httpMethod.ToUpper ()
                                  };
            var result = accessControlManager.CanAccess ( resourceRequest );
            return result;
        }

        /// <summary>
        ///     Checks the box list for model.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="items">The items.</param>
        /// <param name="value">The value.</param>
        /// <param name="text">The text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>A Mvc string for the checkbox list.</returns>
        public static MvcHtmlString CheckBoxListForModel<T> (
            this HtmlHelper htmlHelper,
            IEnumerable<T> items,
            string value,
            string text,
            object htmlAttributes = null )
        {
            var propertyName = htmlHelper.ViewData.ModelMetadata.PropertyName;

            var enumerableModel = htmlHelper.ViewData.Model as IEnumerable;

            //Convert selected value list to a List<string> for easy manipulation
            var selectedValues = enumerableModel != null ? enumerableModel.OfType<T> () : Enumerable.Empty<T> ();

            //Create div
            var divTag = new TagBuilder ( "div" );
            divTag.MergeAttributes ( new RouteValueDictionary ( htmlAttributes ), true );
            divTag.MergeAttributes ( htmlHelper.GetUnobtrusiveValidationAttributes ( propertyName, htmlHelper.ViewData.ModelMetadata ) );
            const string LabelAndCheckboxDiv = "<label tabIndex=\"{4}\"><input type=\"checkbox\" name=\"{0}\" id=\"{0}_{1}\" " +
                                               "value=\"{1}\" {2} />{3}</label>";

            var innerHtmlBuilder = new StringBuilder ();
            foreach ( var item in items )
            {
                innerHtmlBuilder.Append (
                    string.Format (
                        LabelAndCheckboxDiv,
                        propertyName,
                        DataBinder.Eval ( item, value ),
                        selectedValues.Contains ( item ) ? "checked=\"checked\"" : string.Empty,
                        DataBinder.Eval ( item, text ),
                        0 ) );
            }
            divTag.InnerHtml = innerHtmlBuilder.ToString ();
            return MvcHtmlString.Create ( divTag.ToString () );
        }

        /// <summary>Editors for.</summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="idAttributeValue">The identifier attribute value.</param>
        /// <param name="viewData">The view data.</param>
        /// <returns>A <see cref="MvcHtmlString" />.</returns>
        public static MvcHtmlString EditorFor<TModel, TValue> (
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            string templateName,
            string idAttributeValue,
            object viewData )
        {
            var mvcHtmlString = EditorExtensions.EditorFor ( html, expression, templateName, idAttributeValue, viewData ).ToString ();
            mvcHtmlString = ReplaceAttribute ( mvcHtmlString, "id", idAttributeValue );
            return new MvcHtmlString ( mvcHtmlString );
        }

        /// <summary>Editors for.</summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="idAttributeValue">The identifier attribute value.</param>
        /// <returns>A <see cref="MvcHtmlString" />.</returns>
        public static MvcHtmlString EditorFor<TModel, TValue> (
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            string templateName,
            string idAttributeValue )
        {
            return html.EditorFor ( expression, templateName, idAttributeValue, null );
        }

        /// <summary>
        ///     Formats the phone.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns>
        ///     If can format string to phone number format then will return a properly formatted phone number, otherwise an
        ///     empty string.
        /// </returns>
        public static string FormatPhone ( this HtmlHelper htmlHelper, string phoneNumber )
        {
            if ( string.IsNullOrWhiteSpace ( phoneNumber ) )
            {
                return string.Empty;
            }
            const string AllowedChars = "01234567890";
            phoneNumber = new string(phoneNumber.Where(AllowedChars.Contains).ToArray());

            long longPhone;
            var isLong = long.TryParse ( phoneNumber, out longPhone );
            if ( !isLong )
            {
                return string.Empty;
            }
            return string.Format ( "{0:(###) ###-####}", long.Parse ( phoneNumber ) );
        }

        /// <summary>Gets the assessment resource.</summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="assessmentCode">The assessment code.</param>
        /// <returns>A <see cref="String" />.</returns>
        public static string GetAssessmentResource ( this HtmlHelper htmlHelper, string assessmentCode )
        {
            var resourcesManager = htmlHelper.ViewData["ResourcesManager"] as IResourcesManager;
            var resourceManager = resourcesManager.GetResourceManagerByName ( assessmentCode );
            var resource = string.Empty;
            if ( resourceManager != null )
            {
                resource = resourceManager.GetString ( "_" + assessmentCode );
            }
            return resource;
        }

        /// <summary>
        ///     Gets the items to render.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="itemContainer">The item container.</param>
        /// <returns>
        ///     List of <see cref="ItemDto" /> to render.
        /// </returns>
        public static IEnumerable<ItemDto> GetItemsToRender ( this HtmlHelper html, IContainItems itemContainer )
        {
            return
                itemContainer.Items.Where ( item => item.Metadata == null || !item.Metadata.MetadataItemExists<HiddenMetadataItem> ( mi => mi.IsHidden ) )
                    .OrderBy (
                        item =>
                        {
                            var metaItem = item.Metadata == null ? null : item.Metadata.FindMetadataItem<DisplayOrderMetadataItem> ();
                            return metaItem == null ? 0 : metaItem.Order;
                        } );
        }

        /// <summary>
        ///     Gets the items to render.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="items">The items.</param>
        /// <returns>
        ///     List of <see cref="ItemDto" /> to render.
        /// </returns>
        public static IEnumerable<ItemDto> GetItemsToRender ( this HtmlHelper html, IList<ItemDto> items )
        {
            return
                items.Where ( item => item.Metadata == null || !item.Metadata.MetadataItemExists<HiddenMetadataItem> ( mi => mi.IsHidden ) )
                    .OrderBy (
                        item =>
                        {
                            var metaItem = item.Metadata == null ? null : item.Metadata.FindMetadataItem<DisplayOrderMetadataItem> ();
                            return metaItem == null ? 0 : metaItem.Order;
                        } );
        }

        /// <summary>
        ///     Gets the lookup category.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The lookup category.</returns>
        public static string GetLookupCategory ( this HtmlHelper htmlHelper, string propertyName )
        {
            var metadata = ModelMetadata.FromStringExpression ( propertyName, htmlHelper.ViewData );
            if ( metadata.AdditionalValues.ContainsKey ( LookupCategoryAttribute.LookupCategory ) )
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
        /// <returns>List of lookups.</returns>
        public static IEnumerable<LookupDto> GetLookupOptionsForCategory ( this HtmlHelper htmlHelper, string category )
        {
            var selectListItems = htmlHelper.ViewData[category + "LookupItems"] as IList<LookupDto>;
            if ( selectListItems == null )
            {
            }
            return selectListItems;
        }

        /// <summary>Gets the properties to render.</summary>
        /// <param name="html">The HTML.</param>
        /// <returns>A <see cref="IEnumerable{ModelMetadata}" />.</returns>
        public static IEnumerable<ModelMetadata> GetPropertiesToRender ( this HtmlHelper html )
        {
            return
                html.ViewData.ModelMetadata.Properties.Where ( p => p.ShowForEdit )
                    .OrderBy ( meta => meta.Order );
        }

        /// <summary>
        ///     Gets the question groups.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="propertyMetadata">The property metadata.</param>
        /// <returns>List of collections groups.</returns>
        public static IEnumerable<IQuestionGroup> GetQuestionGroups ( this HtmlHelper htmlHelper, ModelMetadata propertyMetadata )
        {
            if ( propertyMetadata.AdditionalValues.ContainsKey ( QuestionGroupAttribute.QuestionGroup ) )
            {
                var groups =
                    ( propertyMetadata.AdditionalValues[QuestionGroupAttribute.QuestionGroup] as
                        IEnumerable<IQuestionGroup> ).OrderBy ( qg => qg.ApplyOrder );
                return groups;
            }
            return Enumerable.Empty<IQuestionGroup> ();
        }

        /// <summary>
        ///     Gets the type of the question.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="propertyMetadata">The property metadata.</param>
        /// <returns>The question type.</returns>
        public static string GetQuestionType ( this HtmlHelper htmlHelper, ModelMetadata propertyMetadata )
        {
            if ( propertyMetadata.AdditionalValues.ContainsKey ( QuestionAttribute.Question ) )
            {
                return propertyMetadata.AdditionalValues[QuestionAttribute.Question].ToString ();
            }
            return QuestionType.GeneralQuestion.ToString ();
        }

        /// <summary>
        ///     Gets the resource.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="sectionDto">The section dto.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>The resource for the section dto.</returns>
        public static string GetResource ( this HtmlHelper htmlHelper, SectionDto sectionDto, string suffix = null )
        {
            var resourceManager = htmlHelper.ViewData["ResourceManager"] as ResourceManager;
            var resource = string.Empty;
            if ( resourceManager != null )
            {
                resource =
                    resourceManager.GetString (
                        SharedStringNames.ResourceKeyPrefix + sectionDto.ItemDefinitionCode +
                        ( suffix == null ? string.Empty : SharedStringNames.ResourceKeyPrefix + suffix ) );
            }
            return resource;
        }

        /// <summary>
        ///     Gets the resource.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <returns>The named resource.</returns>
        public static string GetResource ( this HtmlHelper htmlHelper, string name )
        {
            var resourceManager = htmlHelper.ViewData["ResourceManager"] as ResourceManager;
            var resource = string.Empty;
            if ( resourceManager != null )
            {
                resource =
                    resourceManager.GetString ( name );
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
        public static string GetResource ( this HtmlHelper htmlHelper, ItemDto itemDto, string suffix = null )
        {
            var resourceManager = htmlHelper.ViewData["ResourceManager"] as ResourceManager;
            var resource = string.Empty;
            if ( resourceManager != null )
            {
                resource =
                    resourceManager.GetString (
                        SharedStringNames.ResourceKeyPrefix + itemDto.ItemDefinitionCode +
                        ( suffix == null ? string.Empty : SharedStringNames.ResourceKeyPrefix + suffix ) );
            }
            return resource;
        }

        /// <summary>
        ///     Gets the resource.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="sectionSummaryDto">The item dto.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>The resource for the item dto.</returns>
        public static string GetResource ( this HtmlHelper htmlHelper, SectionSummaryDto sectionSummaryDto, string suffix = null )
        {
            var resourceManager = htmlHelper.ViewData["ResourceManager"] as ResourceManager;
            var resource = string.Empty;
            if ( resourceManager != null )
            {
                resource =
                    resourceManager.GetString (
                        SharedStringNames.ResourceKeyPrefix + sectionSummaryDto.ItemDefinitionCode +
                        ( suffix == null ? string.Empty : SharedStringNames.ResourceKeyPrefix + suffix ) );
            }
            return resource;
        }

        /// <summary>
        ///     Gets the resource.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="lookupDto">The lookup dto.</param>
        /// <returns>The resource for the lookup.</returns>
        public static string GetResource ( this HtmlHelper htmlHelper, LookupDto lookupDto )
        {
            var resourceManager = htmlHelper.ViewData["ResourceManager"] as ResourceManager;
            var resource = string.Empty;
            if ( resourceManager != null )
            {
                resource = resourceManager.GetString ( "_" + lookupDto.Code );
            }
            return resource;
        }

        /// <summary>
        ///     Determines whether [has check all attribute] [the specified HTML helper].
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <returns>
        ///     <c>True</c> if [has check all attribute] [the specified HTML helper]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasCheckAllAttribute ( this HtmlHelper htmlHelper )
        {
            return htmlHelper.ViewData.ModelMetadata.AdditionalValues.ContainsKey ( CheckAllAttribute.CheckAll );
        }

        /// <summary>Determines whether [is lookup property] [the specified HTML helper].</summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="propertyMetadata">The property metadata.</param>
        /// <returns><c>True</c> if is lookup property, otherwise <c>False</c>.</returns>
        public static bool IsLookupProperty ( this HtmlHelper htmlHelper, ModelMetadata propertyMetadata )
        {
            return propertyMetadata.ModelType == typeof(LookupDto) ||
                   typeof(IEnumerable<LookupDto>).IsAssignableFrom ( propertyMetadata.ModelType );
        }

        /// <summary>Labels for.</summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="labelText">The label text.</param>
        /// <param name="forAttributeValue">For attribute value.</param>
        /// <returns>A <see cref="MvcHtmlString" />.</returns>
        public static MvcHtmlString LabelFor<TModel, TValue> (
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            string labelText,
            string forAttributeValue )
        {
            var mvcHtmlString = html.LabelFor ( expression, labelText ).ToString ();
            mvcHtmlString = ReplaceAttribute ( mvcHtmlString, "for", forAttributeValue );
            return new MvcHtmlString ( mvcHtmlString );
        }

        /// <summary>Properties the name for.</summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>A <see cref="String" />.</returns>
        public static string PropertyNameFor<TModel, TProperty> ( this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression )
        {
            var fullName = html.NameFor ( expression ).ToString ();
            return !string.IsNullOrWhiteSpace ( fullName ) ? fullName.Split ( '.' ).Last () : fullName;
        }

        /// <summary>RadioButtons for select list.</summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="listOfValues">The list of values.</param>
        /// <returns>A <see cref="MvcHtmlString" />.</returns>
        public static MvcHtmlString RadioButtonForSelectList<TModel, TProperty> (
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> listOfValues )
        {
            var metaData = ModelMetadata.FromLambdaExpression ( expression, htmlHelper.ViewData );

            var innerHtmlBuilder = new StringBuilder ();
            foreach ( var item in listOfValues )
            {
                var id = string.Format ( "{0}_{1}", metaData.PropertyName, item.Value );

                var radio = htmlHelper.RadioButtonFor ( expression, item.Value, new { id = id } ).ToHtmlString ();
                innerHtmlBuilder.AppendFormat ( "<label for=\"{0}\">{1} {2}</label> ", id, radio, HttpUtility.HtmlEncode ( item.Text ) );
            }

            return MvcHtmlString.Create ( innerHtmlBuilder.ToString () );
        }

        /// <summary>
        ///     Reads the only editor.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="additionalViewData">The additional view data.</param>
        /// <returns>Mvc string for readonly editor.</returns>
        public static MvcHtmlString ReadOnlyEditor ( this HtmlHelper html, string expression, object additionalViewData = null )
        {
            return ReadOnlyEditor ( html, expression, null, null, additionalViewData );
        }

        /// <summary>
        ///     Reads the only editor.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="additionalViewData">The additional view data.</param>
        /// <returns>Mvc string for readonly editor.</returns>
        public static MvcHtmlString ReadOnlyEditor ( this HtmlHelper html, string expression, string templateName, object additionalViewData = null )
        {
            return ReadOnlyEditor ( html, expression, templateName, null, additionalViewData );
        }

        /// <summary>
        ///     Reads the only editor.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="htmlFieldName">Name of the HTML field.</param>
        /// <param name="additionalViewData">The additional view data.</param>
        /// <returns>Mvc string for readonly editor.</returns>
        public static MvcHtmlString ReadOnlyEditor ( this HtmlHelper html, string expression, string templateName, string htmlFieldName, object additionalViewData = null )
        {
            var mvcHtmlString = html.Editor ( expression, templateName, htmlFieldName, additionalViewData );
            var htmlString = mvcHtmlString.ToHtmlString ();
            if ( htmlString.Contains ( "<input " ) || htmlString.Contains ( "<select " ) )
            {
                return new MvcHtmlString (
                    htmlString.Replace ( "<input ", "<input disabled=\"disabled\"" )
                        .Replace ( "<select ", "<select disabled=\"disabled\"" ) );
            }
            return mvcHtmlString;
        }

        /// <summary>
        ///     Secures the action link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>An empty string if user does not have access, otherwise action link string.</returns>
        public static MvcHtmlString SecureActionLink (
            this HtmlHelper htmlHelper,
            string linkText,
            string actionName,
            string controllerName,
            object routeValues,
            object htmlAttributes )
        {
            return SecureActionLink ( htmlHelper, linkText, actionName, controllerName, new RouteValueDictionary ( routeValues ), htmlAttributes );
        }

        /// <summary>
        ///     Secures the action link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValueDictionary">The route value dictionary.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>An empty string if user does not have access, otherwise action link string.</returns>
        public static MvcHtmlString SecureActionLink (
            this HtmlHelper htmlHelper,
            string linkText,
            string actionName,
            string controllerName,
            RouteValueDictionary routeValueDictionary,
            object htmlAttributes )
        {
            MvcHtmlString result;
            if ( htmlHelper.CanAccess ( controllerName, actionName ) )
            {
                // HttpMothed always be get
                result = htmlHelper.ActionLink (
                    linkText,
                    actionName,
                    controllerName,
                    routeValueDictionary,
                    HtmlHelper.AnonymousObjectToHtmlAttributes ( htmlAttributes ) );
            }
            else
            {
                result = new MvcHtmlString ( string.Empty );
            }
            return result;
        }

        /// <summary>
        ///     Secures the web API action link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>An empty string if user does not have access, otherwise action link string.</returns>
        public static MvcHtmlString SecureWebApiActionLink (
            this HtmlHelper htmlHelper,
            string linkText,
            string actionName,
            string controllerName,
            object routeValues,
            object htmlAttributes )
        {
            var routeValueDictionary = new RouteValueDictionary ( routeValues ) { { "httproute", true } };
            return SecureActionLink ( htmlHelper, linkText, actionName, controllerName, routeValueDictionary, htmlAttributes );
        }

        /// <summary>To the select list.</summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="enumObj">The enum object.</param>
        /// <returns>A <see cref="SelectList" />.</returns>
        public static SelectList ToSelectList<TEnum> ( this TEnum enumObj )
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            var values = from TEnum e in Enum.GetValues ( typeof(TEnum) )
                select new { Id = e, Name = e.ToString () };
            return new SelectList ( values, "Id", "Name", enumObj );
        }

        /// <summary>
        /// Converts the hyperlinks.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="textWithPotentialHyperlinks">The text with potential hyperlinks.</param>
        /// <returns>Returns an MvcHtmlString.</returns>
        public static MvcHtmlString ConvertHyperlinks( this HtmlHelper htmlHelper, string textWithPotentialHyperlinks)
        {
            const string UrlRegEx = @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)";
            var rx = new Regex(UrlRegEx);
            var result = rx.Replace(
                textWithPotentialHyperlinks, 
                delegate(Match match) 
            {
                var url = match.ToString();
                return string.Format("<a href=\"{0}\">{0}</a>", url);
            });
            return new MvcHtmlString ( result );
        }

        #endregion

        #region Methods

        private static string ReplaceAttribute ( string mvcHtmlString, string attribute, string attributeValue )
        {
            attribute = attribute + "=\"";
            var start = mvcHtmlString.IndexOf ( attribute, StringComparison.OrdinalIgnoreCase );
            if ( start > -1 )
            {
                var end = mvcHtmlString.IndexOf ( "\"", start + attribute.Length, StringComparison.OrdinalIgnoreCase );
                var oldForAttributePair = mvcHtmlString.Substring ( start, end - start + 1 );
                mvcHtmlString = mvcHtmlString.Replace ( oldForAttributePair, attribute + attributeValue + "\"" );
            }
            return mvcHtmlString;
        }

        #endregion
    }
}