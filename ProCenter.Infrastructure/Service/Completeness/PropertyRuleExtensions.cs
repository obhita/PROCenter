namespace ProCenter.Infrastructure.Service.Completeness
{
    #region

    using System;
    using System.Linq.Expressions;
    using Pillar.Common.Utility;
    using Pillar.FluentRuleEngine;

    #endregion

    public static class PropertyRuleExtensions
    {
        public static bool ContainProperty<TEntity>(this IPropertyRule propertyRule, Expression<Func<TEntity, object>> propertyExpression)
        {
            return propertyRule.PropertyChain.Contains(PropertyUtil.ExtractPropertyName(propertyExpression));
        }
    }
}