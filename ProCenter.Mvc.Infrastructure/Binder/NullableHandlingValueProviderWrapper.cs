namespace ProCenter.Mvc.Infrastructure.Binder
{
    #region Using Statements

    using System.Web.Mvc;

    #endregion

    public class NullableHandlingValueProviderWrapper : IValueProvider, INullableHandling
    {
        #region Fields

        private readonly IValueProvider _backingProvider;

        #endregion

        #region Constructors and Destructors

        public NullableHandlingValueProviderWrapper(IValueProvider backingProvider)
        {
            this._backingProvider = backingProvider;
        }

        #endregion

        #region Public Methods and Operators

        public bool ContainsPrefix(string prefix)
        {
            return this._backingProvider.ContainsPrefix(prefix);
        }

        public ValueProviderResult GetValue(string key)
        {
            var result = this._backingProvider.GetValue(key);
            return result == null ? null : new NullableHandlingValueProviderResult(result);
        }

        #endregion
    }

    public interface INullableHandling
    {
    }
}