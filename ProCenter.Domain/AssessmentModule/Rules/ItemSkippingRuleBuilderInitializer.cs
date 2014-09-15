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

    using Pillar.Common.Extension;
    using Pillar.FluentRuleEngine;

    #endregion

    /// <summary>The item skipping rule builder initializer class.</summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    public class ItemSkippingRuleBuilderInitializer<TContext> : IItemSkippingRuleBuilderInitializer<TContext>
        where TContext : RuleEngineContext<AssessmentInstance>
    {
        #region Fields

        private readonly Action<IItemSkippingRule> _addRuleCallBack;

        private readonly string _name;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="ItemSkippingRuleBuilderInitializer{TContext}" /> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="addRuleCallBack">The add rule call back.</param>
        internal ItemSkippingRuleBuilderInitializer ( string name, Action<IItemSkippingRule> addRuleCallBack )
        {
            _name = name;
            _addRuleCallBack = addRuleCallBack;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Fors the item instance.</summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="itemDefinitionCode">The item definition code.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilder{TContext,TProperty}" />.</returns>
        public IItemSkippingRuleBuilder<TContext, TProperty> ForItemInstance<TProperty>(string itemDefinitionCode)
        {
            var rule = new ItemSkippingRule<TProperty> ( itemDefinitionCode, _name );
            _addRuleCallBack ( rule );
            return new ItemSkippingRuleBuilder<TContext, TProperty> ( rule );
        }

        #endregion
    }
}