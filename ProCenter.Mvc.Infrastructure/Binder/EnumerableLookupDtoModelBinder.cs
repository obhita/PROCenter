namespace ProCenter.Mvc.Infrastructure.Binder
{
    #region Using Statements

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ProCenter.Service.Message.Common.Lookups;

    #endregion

    public class EnumerableLookupDtoModelBinder : IModelBinder
    {
        /// <summary>
        ///     Binds the model to a value by using the specified controller context and binding context.
        /// </summary>
        /// <returns>
        ///     The bound value.
        /// </returns>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var codes = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).RawValue as IEnumerable<string>;
            return codes == null
                       ? null
                       : codes.Where(c => !string.IsNullOrEmpty(c)).Select(c => new LookupDto {Code = c});
        }
    }
}