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
    using System.Linq;
    using System.Linq.Expressions;

    using Pillar.Common.Extension;
    using Pillar.Common.Utility;
    using Pillar.FluentRuleEngine;
    using Pillar.FluentRuleEngine.Constraints;
    using Pillar.FluentRuleEngine.Rules;

    #endregion

    /// <summary>Rule used to skip items.</summary>
    /// <typeparam name="TValueType">The type of the value.</typeparam>
    public class ItemSkippingRule<TValueType> : Rule, IItemSkippingRule
    {
        #region Fields

        private readonly IList<IConstraint> _constraints;

        private readonly string _itemDefinitionCode;

        private readonly List<ItemDefinition> _skippedItemDefinitions;

        private IEnumerable<string> _validNonResponseLookups;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="ItemSkippingRule{TValueType}" /> class.</summary>
        /// <param name="itemDefinitionCode">The item definition code.</param>
        /// <param name="name">The name.</param>
        protected internal ItemSkippingRule ( string itemDefinitionCode, string name )
            : base(name)
        {
            Check.IsNotNullOrWhitespace ( itemDefinitionCode, "itemDefinition code is required." );

            _itemDefinitionCode = itemDefinitionCode;
            _skippedItemDefinitions = new List<ItemDefinition> ();
            _constraints = new List<IConstraint> ();
            WhenClause = ExecuteWhenClause;
            AddThenClause ( ExecuteThenClause );
            AddElseThenClause ( ExecuteElseThenClause );
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the <see cref="T:System.Collections.Generic.IEnumerable`1">constraints</see> of the rule.
        /// </summary>
        public IEnumerable<IConstraint> Constraints
        {
            get { return _constraints; }
        }

        /// <summary>Gets the item definitions.</summary>
        /// <value>The item definitions.</value>
        public IEnumerable<ItemDefinition> SkippedItemDefinitions
        {
            get { return _skippedItemDefinitions; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Adds Constraint to rule.
        /// </summary>
        /// <param name="constraint">
        ///     <see cref="T:Pillar.FluentRuleEngine.Constraints.IConstraint" /> to add to rule.
        /// </param>
        protected internal virtual void AddConstraint ( IConstraint constraint )
        {
            Check.IsNotNull ( constraint, "constraint is required." );
            _constraints.Add ( constraint );
        }

        /// <summary>Adds the item definition to skip.</summary>
        /// <param name="itemDefinition">The item definition.</param>
        protected internal virtual void AddItemDefinitionToSkip ( ItemDefinition itemDefinition )
        {
            Check.IsNotNull ( itemDefinition, "Item definition is required." );
            _skippedItemDefinitions.Add ( itemDefinition );
        }

        /// <summary>Checks the non response.</summary>
        /// <param name="validNonResponseLookups">The valid non response lookups.</param>
        protected internal virtual void CheckNonResponse ( IEnumerable<string> validNonResponseLookups )
        {
            Check.IsNotNull ( validNonResponseLookups, "Valid non response lookups cannot be null." );
            _validNonResponseLookups = validNonResponseLookups;
        }

        private void ExecuteThenClause ( IRuleEngineContext context )
        {
            var skippingContext = context.WorkingMemory.GetContextObject<SkippingContext>();

            Check.IsNotNull ( skippingContext, "There was no skipping context in the working memeory" );

            SkippedItemDefinitions.ForEach ( skippingContext.SkippedItemDefinitions.Add );
        }

        private void ExecuteElseThenClause(IRuleEngineContext context)
        {
            var skippingContext = context.WorkingMemory.GetContextObject<SkippingContext>();

            Check.IsNotNull(skippingContext, "There was no skipping context in the working memeory");

            SkippedItemDefinitions.ForEach(skippingContext.UnSkippedItemDefinitions.Add);
        }

        private bool ExecuteWhenClause ( IRuleEngineContext context )
        {
            var flag = true;
            var itemInstance = context.WorkingMemory.GetContextObject<ItemInstance> ( _itemDefinitionCode );
            var contextObject = new List<IConstraint> ();
            if (_validNonResponseLookups != null)
            {
                var nonResponseValue = itemInstance.Value == null ? null : itemInstance.Value.ToString ();
                if ( nonResponseValue != null )
                {
                    flag = _validNonResponseLookups.Contains ( nonResponseValue );
                }
            }
            if (_validNonResponseLookups == null || !flag)
            {
                //i =>
                var propertyValue = itemInstance.Value == null || string.IsNullOrWhiteSpace(itemInstance.Value.ToString()) ?
                    (TValueType)typeof(TValueType).GetDefault() : 
                    itemInstance.Value is TValueType ? itemInstance.Value : 
                    (TValueType)Convert.ChangeType(itemInstance.Value, typeof(TValueType));
                foreach ( var constraint in Constraints )
                {
                    if ( !constraint.IsCompliant ( propertyValue, context ) )
                    {
                        flag = false;
                        contextObject.Add ( constraint );
                    }
                    else
                    {
                        flag = true;
                    }
                }
            }
            context.WorkingMemory.AddContextObject ( contextObject, Name );
            return flag;
        }

        #endregion
    }
}