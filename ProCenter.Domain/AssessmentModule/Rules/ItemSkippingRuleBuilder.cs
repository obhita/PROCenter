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

    using System.Collections.Generic;

    using Pillar.FluentRuleEngine;
    using Pillar.FluentRuleEngine.Constraints;

    #endregion

    /// <summary>The item skipping rule builder class.</summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    public class ItemSkippingRuleBuilder<TContext, TProperty> : IItemSkippingRuleBuilder<TContext, TProperty>, IItemSkippingRuleBuilderFinalizer<TContext, TProperty>
        where TContext : RuleEngineContext<AssessmentInstance>
    {
        #region Fields

        private readonly ItemSkippingRule<TProperty> _skippingRule;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="ItemSkippingRuleBuilder{TContext, TProperty}" /> class.</summary>
        /// <param name="skippingRule">The skipping rule.</param>
        public ItemSkippingRuleBuilder(ItemSkippingRule<TProperty> skippingRule)
        {
            _skippingRule = skippingRule;
        }

        #endregion

        #region Public Properties

        /// <summary>Gets the skipping rule.</summary>
        /// <value>The skipping rule.</value>
        public IItemSkippingRule SkippingRule
        {
            get { return _skippingRule; }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Adds a Constraint to the rule.</summary>
        /// <param name="constraint"><see cref="T:Pillar.FluentRuleEngine.Constraints.IConstraint">Constraint</see> to add to rule.</param>
        /// <returns>An <see cref="IItemSkippingRuleBuilder{TContext, TProperty}" /></returns>
        public IItemSkippingRuleBuilder<TContext, TProperty> Constrain(IConstraint constraint)
        {
            _skippingRule.AddConstraint ( constraint );
            return this;
        }

        /// <summary>Skips the item.</summary>
        /// <param name="itemDefinition">The item definition.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" />.</returns>
        public IItemSkippingRuleBuilderFinalizer<TContext, TProperty> SkipItem ( ItemDefinition itemDefinition )
        {
            _skippingRule.AddItemDefinitionToSkip ( itemDefinition );
            return this;
        }

        /// <summary>Ors the non response.</summary>
        /// <param name="nonResponseLookups">The non response lookups.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilderFinalizer{TContext, TProperty}"/>.</returns>
        public IItemSkippingRuleBuilderFinalizer<TContext, TProperty> OrNonResponse(IEnumerable<string> nonResponseLookups )
        {
            _skippingRule.CheckNonResponse ( nonResponseLookups );
            return this;
        }

        #endregion
    }
}