using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCenter.Domain
{
    using System.Linq.Expressions;
    using Pillar.Common.Utility;
    using Pillar.FluentRuleEngine;
    using Pillar.FluentRuleEngine.Constraints;
    using Pillar.FluentRuleEngine.Resources;
    using Pillar.FluentRuleEngine.Rules;

    public static class ContextObjectProviderRuleBuilderExtensions
    {
        public static IContextObjectProviderRuleBuilder<TContext, TSubject, TContextObject> UseSubjectForRuleViolation<TContext, TSubject, TContextObject> (
            this IContextObjectProviderRuleBuilder<TContext, TSubject, TContextObject> ruleBuilder, Expression<Func<TSubject, object>> propertyExpression)
            where TContext : RuleEngineContext<TSubject>
        {
            ruleBuilder.ElseThen(
                        (s, ctx) =>
                        {
                            var failedConstraints = ctx.WorkingMemory.GetContextObject<List<IConstraint>>(ruleBuilder.Rule.Name);
                            foreach (var constraint in failedConstraints)
                            {
                                if (!(constraint is IHandleAddingRuleViolations))
                                {
                                    var propertyName = ctx.NameProvider.GetName(s, propertyExpression);

                                    var formatedMessage = constraint.Message.FormatRuleEngineMessage(propertyName);

                                    var ruleViolation = new RuleViolation(
                                            ruleBuilder.Rule, s, formatedMessage, PropertyUtil.ExtractPropertyName ( propertyExpression ));
                                    ctx.RuleViolationReporter.Report(ruleViolation);
                                }
                            }
                            failedConstraints.Clear ();
                        });
            return ruleBuilder;
        }
    }
}
