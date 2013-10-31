#region Licence Header
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
namespace ProCenter.Mvc.Infrastructure.Binder
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Primitive;
    using ProCenter.Infrastructure.Extensions;

    public class ProCenterModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (!(bindingContext.ValueProvider is INullableHandling))
            {
                bindingContext.ValueProvider = new NullableHandlingValueProviderWrapper(bindingContext.ValueProvider);
            }
            return base.BindModel(controllerContext, bindingContext);
        }

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (typeof (IPrimitive).IsAssignableFrom(modelType))
            {
                var modelTypeIsNullable = bindingContext.ModelType.IsNullable();
                var constructor = modelType.GetConstructors().OrderByDescending(c => c.GetParameters().Count()).FirstOrDefault();
                var allParamsNull = true;
                var parameters = constructor.GetParameters().Select(p =>
                    {
                        var propertyName = p.Name.ToFirstLetterUpper();
                        var propertyPath = (bindingContext.Model == null ? bindingContext.ModelName : bindingContext.ModelMetadata.PropertyName) + "." + propertyName;
                        var valueResult = bindingContext.ValueProvider.GetValue(propertyPath);
                        if (modelTypeIsNullable && (valueResult == null || string.IsNullOrWhiteSpace(valueResult.AttemptedValue)))
                        {
                            return null;
                        }
                        allParamsNull = false;
                        var propertyType = bindingContext.PropertyMetadata[propertyName].ModelType;
                        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof (Nullable<>))
                        {
                            propertyType = Nullable.GetUnderlyingType(propertyType);
                        }
                        else if (propertyType.IsEnum)
                        {
                            return Enum.Parse(propertyType, valueResult.AttemptedValue.Replace("(", "").Replace(")", ""));
                        }
                        return (valueResult.AttemptedValue as IConvertible).ToType(propertyType, valueResult.Culture);
                    }).ToArray();
                if (modelTypeIsNullable && allParamsNull)
                {
                    return null;
                }
                var model = Activator.CreateInstance(modelType, parameters);
                return model;
            }
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }

        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.Model == null)
            {
                return;
            }
            base.OnModelUpdated(controllerContext, bindingContext);
        }
    }
}
