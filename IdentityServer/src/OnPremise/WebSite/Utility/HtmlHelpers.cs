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
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Thinktecture.IdentityServer.Web.Utility
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString ValidatorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var prop = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
            var name = ExpressionHelper.GetExpressionText(expression);
            return ValidatorInternal(html, name, prop.Description);
        }

        public static MvcHtmlString Validator(this HtmlHelper html, ModelMetadata prop)
        {
            var name = html.ViewData.TemplateInfo.GetFullHtmlFieldName(prop.PropertyName);
            return ValidatorInternal(html, name, prop.Description);
        }
        
        public static MvcHtmlString Validator(this HtmlHelper html, string name, string description = null)
        {
            return ValidatorInternal(html, name, description);
        }

        static MvcHtmlString ValidatorInternal(this HtmlHelper html, string name, string description)
        {
            if (html.ViewData.ModelState.IsValidField(name))
            {
                if (!String.IsNullOrWhiteSpace(description))
                {
                    var help = UrlHelper.GenerateContentUrl("~/Content/Images/help.png", html.ViewContext.HttpContext);
                    TagBuilder img = new TagBuilder("img");
                    img.Attributes.Add("src", help);
                    img.Attributes.Add("title", description);
                    return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
                }
            }
            else
            {
                var error = UrlHelper.GenerateContentUrl("~/Content/Images/error.png", html.ViewContext.HttpContext);
                TagBuilder img = new TagBuilder("img");
                img.AddCssClass("error");
                img.Attributes.Add("src", error);
                var title = html.ViewData.ModelState[name].Errors.First().ErrorMessage;
                if (!String.IsNullOrWhiteSpace(description)) title += "\n\n" + description;
                img.Attributes.Add("title", title);
                return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
            }

            return MvcHtmlString.Empty;
        }
    }
}