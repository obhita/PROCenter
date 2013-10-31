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
