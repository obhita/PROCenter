namespace ProCenter.Infrastructure.Extensions
{
    #region Using Statements

    using System;

    #endregion

    public static class TypeExtensions
    {
        public static bool IsNullable(this Type type)
        {
            if (!type.IsValueType)
            {
                return true; // ref-type
            }
            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}