namespace ProCenter.Mvc.Infrastructure.Binder
{
    #region Using Statements

    using System;
    using System.Globalization;
    using System.Web.Mvc;

    #endregion

    public class NullableHandlingValueProviderResult : ValueProviderResult
    {
        #region Constructors and Destructors

        public NullableHandlingValueProviderResult(ValueProviderResult valueProviderResult)
            : base(valueProviderResult.RawValue, valueProviderResult.AttemptedValue, valueProviderResult.Culture)
        {
        }

        #endregion

        #region Public Methods and Operators

        public override object ConvertTo(Type type, CultureInfo culture)
        {
            var value = RawValue;
            if (RawValue == null || type.IsInstanceOfType(value))
            {
                return RawValue;
            }
            var nullableType = Nullable.GetUnderlyingType(type);
            return base.ConvertTo(nullableType ?? type, culture);
        }

        #endregion
    }
}