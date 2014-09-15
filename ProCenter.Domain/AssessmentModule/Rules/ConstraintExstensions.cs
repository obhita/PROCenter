#region License Header

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

namespace ProCenter.Domain.AssessmentModule.Rules
{
    #region Using Statements

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using DevExpress.Data.PLinq.Helpers;

    using Pillar.Common.Extension;
    using Pillar.Common.Specification;
    using Pillar.Common.Utility;
    using Pillar.FluentRuleEngine;
    using Pillar.FluentRuleEngine.Constraints;
    using Pillar.FluentRuleEngine.Resources;

    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>The constraint exstensions class.</summary>
    public static class ConstraintExstensions
    {
        #region Public Methods and Operators

        /// <summary>Adds an <see cref="InlineConstraint{TProperty}" /> of EqualTo to the Rule.</summary>
        /// <typeparam name="TContext">Type of <see cref="IRuleEngineContext" /> of the rule.</typeparam>
        /// <typeparam name="TProperty">Type of property of the subject of the rule.</typeparam>
        /// <param name="itemSkippingRuleBuilder">
        ///     <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /> currently configuring
        ///     the rule.
        /// </param>
        /// <param name="compareValue">Value to compare to value of property.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /></returns>
        public static IItemSkippingRuleBuilder<TContext, TProperty> EqualTo<TContext, TProperty> (
            this IItemSkippingRuleBuilder<TContext, TProperty> itemSkippingRuleBuilder,
            TProperty compareValue ) where TContext : RuleEngineContext<AssessmentInstance>
        {
            var message = Messages.Constraints_Comparison_Message.FormatCompareRuleEngineMessage ( compareValue, "=" );
            itemSkippingRuleBuilder.Constrain ( new InlineConstraint<TProperty> ( lhs => Comparer<TProperty>.Default.Compare ( compareValue, lhs ) == 0, message ) );
            return itemSkippingRuleBuilder;
        }

        /// <summary>Adds an <see cref="InlineConstraint{TProperty}" /> of ExclusiveBetween to the Rule.</summary>
        /// <typeparam name="TContext">Type of <see cref="IRuleEngineContext" /> of the rule.</typeparam>
        /// <typeparam name="TProperty">Type of property of the subject of the rule.</typeparam>
        /// <param name="itemSkippingRuleBuilder">
        ///     <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /> currently configuring
        ///     the rule.
        /// </param>
        /// <param name="startValue">Start Value to use in comparison to property value.</param>
        /// <param name="endValue">End Value to use in comparison to property value.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /></returns>
        public static IItemSkippingRuleBuilder<TContext, TProperty> ExclusiveBetween<TContext, TProperty> (
            this IItemSkippingRuleBuilder<TContext, TProperty> itemSkippingRuleBuilder,
            TProperty startValue,
            TProperty endValue )
            where TContext : RuleEngineContext<AssessmentInstance>
        {
            var message =
                Messages.Constraints_ExclusiveBetween_Message.FormatRuleEngineMessage (
                                                                                       new Dictionary<string, string>
                                                                                       {
                                                                                           { "StartValue", startValue.ToString () },
                                                                                           { "EndValue", startValue.ToString () }
                                                                                       } );
            itemSkippingRuleBuilder.Constrain (
                                               new InlineConstraint<TProperty> (
                                                   lhs => Comparer<TProperty>.Default.Compare ( startValue, lhs ) < 0 && Comparer<TProperty>.Default.Compare ( endValue, lhs ) > 0,
                                                   message ) );
            return itemSkippingRuleBuilder;
        }

        /// <summary>Adds an <see cref="InlineConstraint{TProperty}" /> of GreaterThan to the Rule.</summary>
        /// <typeparam name="TContext">Type of <see cref="IRuleEngineContext" /> of the rule.</typeparam>
        /// <typeparam name="TProperty">Type of property of the subject of the rule.</typeparam>
        /// <param name="itemSkippingRuleBuilder">
        ///     <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /> currently configuring
        ///     the rule.
        /// </param>
        /// <param name="compareValue">Value to compare to value of property.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /></returns>
        public static IItemSkippingRuleBuilder<TContext, TProperty> GreaterThen<TContext, TProperty> (
            this IItemSkippingRuleBuilder<TContext, TProperty> itemSkippingRuleBuilder,
            TProperty compareValue ) where TContext : RuleEngineContext<AssessmentInstance>
        {
            var message = Messages.Constraints_Comparison_Message.FormatCompareRuleEngineMessage ( compareValue, ">" );
            itemSkippingRuleBuilder.Constrain ( new InlineConstraint<TProperty> ( lhs => Comparer<TProperty>.Default.Compare ( compareValue, lhs ) < 0, message ) );
            return itemSkippingRuleBuilder;
        }

        /// <summary>Adds an <see cref="InlineConstraint{TProperty}" /> of GreaterThanOrEqualTo to the Rule.</summary>
        /// <typeparam name="TContext">Type of <see cref="IRuleEngineContext" /> of the rule.</typeparam>
        /// <typeparam name="TProperty">Type of property of the subject of the rule.</typeparam>
        /// <param name="itemSkippingRuleBuilder">
        ///     <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /> currently configuring
        ///     the rule.
        /// </param>
        /// <param name="compareValue">Value to compare to value of property.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /></returns>
        public static IItemSkippingRuleBuilder<TContext, TProperty> GreaterThenOrEqualTo<TContext, TProperty> (
            this IItemSkippingRuleBuilder<TContext, TProperty> itemSkippingRuleBuilder,
            TProperty compareValue ) where TContext : RuleEngineContext<AssessmentInstance>
        {
            var message = Messages.Constraints_Comparison_Message.FormatCompareRuleEngineMessage ( compareValue, ">=" );
            itemSkippingRuleBuilder.Constrain ( new InlineConstraint<TProperty> ( lhs => Comparer<TProperty>.Default.Compare ( compareValue, lhs ) <= 0, message ) );
            return itemSkippingRuleBuilder;
        }

        /// <summary>Adds an <see cref="InlineConstraint{TProperty}" /> of InList to the Rule.</summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="itemSkippingRuleBuilder">The property rule builder.</param>
        /// <param name="list">The list to check.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /></returns>
        public static IItemSkippingRuleBuilder<TContext, TProperty> InList<TContext, TProperty> (
            this IItemSkippingRuleBuilder<TContext, TProperty> itemSkippingRuleBuilder,
            params TProperty[] list ) where TContext : RuleEngineContext<AssessmentInstance>
        {
            Check.IsNotNull ( list, "list is required." );
            var message = Messages.Constraints_InList_Message.FormatRuleEngineMessage ( new Dictionary<string, string> { { "ListString", string.Join ( ", ", list ) } } );
            itemSkippingRuleBuilder.Constrain ( new InlineConstraint<TProperty> ( list.Contains, message ) );
            return itemSkippingRuleBuilder;
        }

        /// <summary>Adds an <see cref="InlineConstraint{TProperty}" /> of InclusiveBetween to the Rule.</summary>
        /// <typeparam name="TContext">Type of <see cref="IRuleEngineContext" /> of the rule.</typeparam>
        /// <typeparam name="TProperty">Type of property of the subject of the rule.</typeparam>
        /// <param name="itemSkippingRuleBuilder">
        ///     <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /> currently configuring
        ///     the rule.
        /// </param>
        /// <param name="startValue">Start Value to use in comparison to property value.</param>
        /// <param name="endValue">End Value to use in comparison to property value.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /></returns>
        public static IItemSkippingRuleBuilder<TContext, TProperty> InclusiveBetween<TContext, TProperty> (
            this IItemSkippingRuleBuilder<TContext, TProperty> itemSkippingRuleBuilder,
            IComparable startValue,
            IComparable endValue )
            where TContext : RuleEngineContext<AssessmentInstance>
        {
            var message =
                Messages.Constraints_InclusiveBetween_Message.FormatRuleEngineMessage (
                                                                                       new Dictionary<string, string>
                                                                                       {
                                                                                           { "StartValue", startValue.ToString () },
                                                                                           { "EndValue", startValue.ToString () }
                                                                                       } );
            itemSkippingRuleBuilder.Constrain ( new InlineConstraint<TProperty> ( lhs => startValue.CompareTo ( lhs ) <= 0 && endValue.CompareTo ( lhs ) >= 0, message ) );
            return itemSkippingRuleBuilder;
        }

        /// <summary>Adds an <see cref="InlineConstraint{TProperty}" /> of InclusiveBetween or null to the Rule.</summary>
        /// <typeparam name="TContext">Type of <see cref="IRuleEngineContext" /> of the rule.</typeparam>
        /// <typeparam name="TProperty">Type of property of the subject of the rule.</typeparam>
        /// <param name="itemSkippingRuleBuilder">
        ///     <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /> currently configuring
        ///     the rule.
        /// </param>
        /// <param name="startValue">Start Value to use in comparison to property value.</param>
        /// <param name="endValue">End Value to use in comparison to property value.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /></returns>
        public static IItemSkippingRuleBuilder<TContext, TProperty> InclusiveBetweenOrNull<TContext, TProperty> (
            this IItemSkippingRuleBuilder<TContext, TProperty> itemSkippingRuleBuilder,
            IComparable startValue,
            IComparable endValue )
            where TContext : RuleEngineContext<AssessmentInstance>
        {
            var message =
                Messages.Constraints_InclusiveBetween_Message.FormatRuleEngineMessage (
                                                                                       new Dictionary<string, string>
                                                                                       {
                                                                                           { "StartValue", startValue.ToString () },
                                                                                           { "EndValue", startValue.ToString () }
                                                                                       } );
            itemSkippingRuleBuilder.Constrain (
                                               new InlineConstraint<TProperty> (
                                                   lhs =>
                                                       ( typeof(TProperty).IsNullable () && lhs == null )
                                                       || ( startValue.CompareTo ( lhs ) <= 0 && endValue.CompareTo ( lhs ) >= 0 ),
                                                   message ) );
            return itemSkippingRuleBuilder;
        }

        /// <summary>Adds an <see cref="InlineConstraint{TProperty}" /> of LessThen to the Rule.</summary>
        /// <typeparam name="TContext">Type of <see cref="IRuleEngineContext" /> of the rule.</typeparam>
        /// <typeparam name="TProperty">Type of property of the subject of the rule.</typeparam>
        /// <param name="itemSkippingRuleBuilder">
        ///     <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /> currently configuring
        ///     the rule.
        /// </param>
        /// <param name="compareValue">Value to compare to value of property.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /></returns>
        public static IItemSkippingRuleBuilder<TContext, TProperty> LessThen<TContext, TProperty> (
            this IItemSkippingRuleBuilder<TContext, TProperty> itemSkippingRuleBuilder,
            TProperty compareValue ) where TContext : RuleEngineContext<AssessmentInstance>
        {
            var message = Messages.Constraints_Comparison_Message.FormatCompareRuleEngineMessage ( compareValue, "<" );
            itemSkippingRuleBuilder.Constrain ( new InlineConstraint<TProperty> ( lhs => Comparer<TProperty>.Default.Compare ( compareValue, lhs ) > 0, message ) );
            return itemSkippingRuleBuilder;
        }

        /// <summary>Adds an <see cref="InlineConstraint{TProperty}" /> of LessThenOrEqualTo to the Rule.</summary>
        /// <typeparam name="TContext">Type of <see cref="IRuleEngineContext" /> of the rule.</typeparam>
        /// <typeparam name="TProperty">Type of property of the subject of the rule.</typeparam>
        /// <param name="itemSkippingRuleBuilder">
        ///     <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /> currently configuring
        ///     the rule.
        /// </param>
        /// <param name="compareValue">Value to compare to value of property.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /></returns>
        public static IItemSkippingRuleBuilder<TContext, TProperty> LessThenOrEqualTo<TContext, TProperty> (
            this IItemSkippingRuleBuilder<TContext, TProperty> itemSkippingRuleBuilder,
            TProperty compareValue ) where TContext : RuleEngineContext<AssessmentInstance>
        {
            var message = Messages.Constraints_Comparison_Message.FormatCompareRuleEngineMessage ( compareValue, "<=" );
            itemSkippingRuleBuilder.Constrain ( new InlineConstraint<TProperty> ( lhs => Comparer<TProperty>.Default.Compare ( compareValue, lhs ) >= 0, message ) );
            return itemSkippingRuleBuilder;
        }

        /// <summary>Adds an <see cref="InlineConstraint{TProperty}" /> of Regex Match to the Rule.</summary>
        /// <typeparam name="TContext">Type of <see cref="IRuleEngineContext" /> of the rule.</typeparam>
        /// <typeparam name="TProperty">Type of property of the subject of the rule.</typeparam>
        /// <param name="itemSkippingRuleBuilder">
        ///     <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /> currently configuring
        ///     the rule.
        /// </param>
        /// <param name="regexString">Regex string to check match on property value.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /></returns>
        public static IItemSkippingRuleBuilder<TContext, TProperty> MatchesRegex<TContext, TProperty> (
            this IItemSkippingRuleBuilder<TContext, TProperty> itemSkippingRuleBuilder,
            string regexString ) where TContext : RuleEngineContext<AssessmentInstance>
        {
            var message = Messages.Constraints_Regex_Message.FormatRuleEngineMessage ( new Dictionary<string, string> { { "RegexString", regexString } } );
            itemSkippingRuleBuilder.Constrain ( new InlineConstraint<TProperty> ( lhs => Regex.IsMatch ( lhs.ToString (), regexString ), message ) );
            return itemSkippingRuleBuilder;
        }

        /// <summary>Adds an <see cref="InlineConstraint{TProperty}" /> of Specification to the Rule.</summary>
        /// <typeparam name="TContext">Type of <see cref="IRuleEngineContext" /> of the rule.</typeparam>
        /// <typeparam name="TProperty">Type of property of the subject of the rule.</typeparam>
        /// <param name="itemSkippingRuleBuilder">
        ///     <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /> currently configuring
        ///     the rule.
        /// </param>
        /// <param name="specification"><see cref="ISpecification{TEntity}" /> to use in Constraint.</param>
        /// <param name="violationMessage">Violation message to use in Constraint.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /></returns>
        public static IItemSkippingRuleBuilder<TContext, TProperty> MeetsSpecification<TContext, TProperty> (
            this IItemSkippingRuleBuilder<TContext, TProperty> itemSkippingRuleBuilder,
            ISpecification<TProperty> specification,
            string violationMessage = null )
            where TContext : RuleEngineContext<AssessmentInstance>
        {
            var message = violationMessage ??
                          Messages.Constraint_Specification_Message.FormatRuleEngineMessage ( new Dictionary<string, string> { { "Specification", specification.ToString () } } );
            itemSkippingRuleBuilder.Constrain ( new InlineConstraint<TProperty> ( specification.IsSatisfiedBy, message ) );
            return itemSkippingRuleBuilder;
        }

        /// <summary>Adds a <see cref="NotEmptyConstraint" /> to the Rule.</summary>
        /// <typeparam name="TContext">Type of <see cref="IRuleEngineContext" /> of the rule.</typeparam>
        /// <typeparam name="TProperty">Type of property of the subject of the rule.</typeparam>
        /// <param name="itemSkippingRuleBuilder">
        ///     <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /> currently configuring
        ///     the rule.
        /// </param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /></returns>
        public static IItemSkippingRuleBuilder<TContext, TProperty> NotEmpty<TContext, TProperty> (
            this IItemSkippingRuleBuilder<TContext, TProperty> itemSkippingRuleBuilder ) where TContext : RuleEngineContext<AssessmentInstance>
        {
            itemSkippingRuleBuilder.Constrain ( new NotEmptyConstraint () );
            return itemSkippingRuleBuilder;
        }

        /// <summary>Adds an <see cref="InlineConstraint{TProperty}" /> of NotEqualTo to the Rule.</summary>
        /// <typeparam name="TContext">Type of <see cref="IRuleEngineContext" /> of the rule.</typeparam>
        /// <typeparam name="TProperty">Type of property of the subject of the rule.</typeparam>
        /// <param name="itemSkippingRuleBuilder">
        ///     <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /> currently configuring
        ///     the rule.
        /// </param>
        /// <param name="compareValue">Value to compare to value of property.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /></returns>
        public static IItemSkippingRuleBuilder<TContext, TProperty> NotEqualTo<TContext, TProperty> (
            this IItemSkippingRuleBuilder<TContext, TProperty> itemSkippingRuleBuilder,
            TProperty compareValue ) where TContext : RuleEngineContext<AssessmentInstance>
        {
            var message = Messages.Constraints_Comparison_Message.FormatCompareRuleEngineMessage ( compareValue, "!=" );
            itemSkippingRuleBuilder.Constrain ( new InlineConstraint<TProperty> ( lhs => Comparer<TProperty>.Default.Compare ( compareValue, lhs ) != 0, message ) );
            return itemSkippingRuleBuilder;
        }

        /// <summary>Adds a <see cref="NotNullConstraint" /> to the Rule.</summary>
        /// <typeparam name="TContext">Type of <see cref="IRuleEngineContext" /> of the rule.</typeparam>
        /// <typeparam name="TProperty">Type of property of the subject of the rule.</typeparam>
        /// <param name="itemSkippingRuleBuilder">
        ///     <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /> currently configuring
        ///     the rule.
        /// </param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /></returns>
        public static IItemSkippingRuleBuilder<TContext, TProperty> NotNull<TContext, TProperty> (
            this IItemSkippingRuleBuilder<TContext, TProperty> itemSkippingRuleBuilder ) where TContext : RuleEngineContext<AssessmentInstance>
        {
            itemSkippingRuleBuilder.Constrain ( new NotNullConstraint () );
            return itemSkippingRuleBuilder;
        }

        /// <summary>Adds a <see cref="NullConstraint" /> to the Rule.</summary>
        /// <typeparam name="TContext">Type of <see cref="IRuleEngineContext" /> of the rule.</typeparam>
        /// <typeparam name="TProperty">Type of property of the subject of the rule.</typeparam>
        /// <param name="itemSkippingRuleBuilder">
        ///     <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /> currently configuring
        ///     the rule.
        /// </param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" />.</returns>
        public static IItemSkippingRuleBuilder<TContext, TProperty> Null<TContext, TProperty> (
            this IItemSkippingRuleBuilder<TContext, TProperty> itemSkippingRuleBuilder ) where TContext : RuleEngineContext<AssessmentInstance>
        {
            itemSkippingRuleBuilder.Constrain ( new NullConstraint () );
            return itemSkippingRuleBuilder;
        }

        /// <summary>
        /// Determines whether does not contain the specified compare value.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="itemSkippingRuleBuilder">The item skipping rule builder.</param>
        /// <param name="compareValue">The compare value.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" />.</returns>
        public static IItemSkippingRuleBuilder<TContext, TProperty> DoesNotContain<TContext, TProperty>(
            this IItemSkippingRuleBuilder<TContext, TProperty> itemSkippingRuleBuilder,
            Lookup compareValue) 
            where TContext : RuleEngineContext<AssessmentInstance>
            where TProperty : IEnumerable<Lookup> 
        {
            var message = Messages.Constraints_Comparison_Message.FormatCompareRuleEngineMessage(compareValue, "!=");
            itemSkippingRuleBuilder.Constrain(new InlineConstraint<TProperty>(lhs => lhs != null && !lhs.Contains ( compareValue ), message));
            return itemSkippingRuleBuilder;
        }

        /// <summary>
        /// Determines whether contains the specified compare value.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="itemSkippingRuleBuilder">The item skipping rule builder.</param>
        /// <param name="compareValue">The compare value.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" /></returns>
        public static IItemSkippingRuleBuilder<TContext, TProperty> Contains<TContext, TProperty>(
            this IItemSkippingRuleBuilder<TContext, TProperty> itemSkippingRuleBuilder,
            Lookup compareValue)
            where TContext : RuleEngineContext<AssessmentInstance>
            where TProperty : IEnumerable<Lookup>
        {
            var message = Messages.Constraints_Comparison_Message.FormatCompareRuleEngineMessage(compareValue, "!=");
            itemSkippingRuleBuilder.Constrain(new InlineConstraint<TProperty>(lhs => lhs != null && lhs.Contains(compareValue), message));
            return itemSkippingRuleBuilder;
        }

        #endregion
    }
}