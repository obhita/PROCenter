namespace ProCenter.Common
{
    using System;
    using System.Linq;
    using System.Security.Claims;

    public static class ClaimPrincipalExtensions
    {
        public static T GetClaim<T>(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == claimType);
            if (claim != null)
            {
                var type = typeof (T);
                var baseType = Nullable.GetUnderlyingType(type) ?? type;
                object value = claim.Value;
                if (baseType == typeof (Guid))
                {
                    value = Guid.Parse(claim.Value);
                }
                return (T) Convert.ChangeType(value, baseType );
            }
            return default(T);
        }
    }
}