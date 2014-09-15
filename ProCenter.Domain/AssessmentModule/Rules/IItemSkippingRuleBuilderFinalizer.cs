namespace ProCenter.Domain.AssessmentModule.Rules
{
    using Pillar.FluentRuleEngine;

    /// <summary>Interface for item skipping rule builder finalizer.</summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    public interface IItemSkippingRuleBuilderFinalizer<TContext, out TProperty>
        where TContext : RuleEngineContext<AssessmentInstance>
    {
        #region Public Methods and Operators

        /// <summary>Skips the item.</summary>
        /// <param name="itemDefinition">The item definition.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" />.</returns>
        IItemSkippingRuleBuilderFinalizer<TContext, TProperty> SkipItem(ItemDefinition itemDefinition);

        #endregion
    }
}