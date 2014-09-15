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
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Pillar.Common.Utility;
    using Pillar.FluentRuleEngine;

    #endregion

    /// <summary>The abstract assessment rule collection class.</summary>
    public abstract class AbstractAssessmentRuleCollection : AbstractRuleCollection<AssessmentInstance>, IAssessmentRuleCollection
    {
        #region Fields

        private readonly List<IItemSkippingRule> _itemSkippingRules = new List<IItemSkippingRule> ();

        #endregion

        #region Public Properties

        /// <summary>Gets the item skipping rules.</summary>
        /// <value>The item skipping rules.</value>
        public IEnumerable<IItemSkippingRule> ItemSkippingRules
        {
            get { return _itemSkippingRules.AsReadOnly (); }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Adds a new skipping item rule.</summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>A <see cref="IItemSkippingRuleBuilderInitializer{RuleEngineContext}" />.</returns>
        public IItemSkippingRuleBuilderInitializer<RuleEngineContext<AssessmentInstance>> NewItemSkippingRule<TProperty> ( Expression<Func<TProperty>> propertyExpression )
            where TProperty : IItemSkippingRule
        {
            Check.IsNotNull ( propertyExpression, "propertyExpression is required." );
            return new ItemSkippingRuleBuilderInitializer<RuleEngineContext<AssessmentInstance>> (
                PropertyUtil.ExtractPropertyName ( propertyExpression ),
                ( rule =>
                {
                    _itemSkippingRules.Add ( rule );
                    AddRule ( rule );
                } ) );
        }

        #endregion
    }
}