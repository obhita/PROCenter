﻿#region License Header

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

namespace ProCenter.Infrastructure.Service.Completeness
{
    #region Using Statements

    using System.Collections.Generic;

    using Pillar.Common.InversionOfControl;
    using Pillar.FluentRuleEngine;

    using ProCenter.Domain.AssessmentModule;

    #endregion

    /// <summary>The completeness rule collection factory class.</summary>
    public class CompletenessRuleCollectionFactory : ICompletenessRuleCollectionFactory
    {
        #region Fields

        private readonly IContainer _container;

        private readonly IRuleCollectionFactory _ruleCollectionFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompletenessRuleCollectionFactory"/> class.
        /// </summary>
        /// <param name="ruleCollectionFactory">The rule collection factory.</param>
        /// <param name="container">The container.</param>
        public CompletenessRuleCollectionFactory ( IRuleCollectionFactory ruleCollectionFactory, IContainer container )
        {
            _ruleCollectionFactory = ruleCollectionFactory;
            _container = container;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Gets the completeness rule collection.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="completenessCategory">The completeness category.</param>
        /// <returns>A <see cref="ICompletenessRuleCollection{TEntity}"/>.</returns>
        public ICompletenessRuleCollection<TEntity> GetCompletenessRuleCollection<TEntity> ( string completenessCategory )
        {
            var completenessRuleCollection = (ICompletenessRuleCollection<TEntity>)_container.TryResolve ( typeof(ICompletenessRuleCollection<TEntity>), completenessCategory );
            _ruleCollectionFactory.CustomizeRuleCollection ( completenessRuleCollection );
            return completenessRuleCollection;
        }

        /// <summary>Gets the completeness rule collections.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>A collection of <see cref="ICompletenessRuleCollection{TEntity}"/>.</returns>
        public IEnumerable<ICompletenessRuleCollection<TEntity>> GetCompletenessRuleCollections<TEntity> ()
        {
            var completenessRuleCollections = _container.ResolveAll<ICompletenessRuleCollection<TEntity>> ();
            foreach ( var completenessRuleCollection in completenessRuleCollections )
            {
                _ruleCollectionFactory.CustomizeRuleCollection ( completenessRuleCollection );
                yield return completenessRuleCollection;
            }
        }

        #endregion
    }
}