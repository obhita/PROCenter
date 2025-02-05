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

namespace ProCenter.Domain.Nih
{
    #region Using Statements

    using System;
    using System.Collections.Generic;

    using Pillar.FluentRuleEngine;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Rules;
    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>
    /// The NihRuleCollection class.
    /// </summary>
    public class NihHealthBehaviorsAssessmentRuleCollection : AbstractAssessmentRuleCollection
    {
        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;

        private static Guid? _nihAssessmentDefinitionKey;

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="NihHealthBehaviorsAssessmentRuleCollection" /> class.</summary>
        /// <param name="assessmentDefinitionRepository">The assessment definition repository.</param>
        public NihHealthBehaviorsAssessmentRuleCollection(IAssessmentDefinitionRepository assessmentDefinitionRepository)
        {
            _assessmentDefinitionRepository = assessmentDefinitionRepository;

            NewItemSkippingRule(() => SkipItem7125038)
              .ForItemInstance<IEnumerable<Lookup>>("7125031")
              .DoesNotContain(NihHealthBehaviorsAssessmentEmploymentStatus.Other)
              .SkipItem(GetItemDefinition("7125038"));

            NewRuleSet(() => ItemUpdatedRuleSet7125031, SkipItem7125038);

            NewItemSkippingRule(() => SkipItem7125041)
              .ForItemInstance<HealthCondition>("7125022")
              .NotEqualTo(HealthCondition.Poor)
              .SkipItem(GetItemDefinition("7125041"));

            NewRuleSet(() => ItemUpdatedRuleSet7125022, SkipItem7125041);

            NewItemSkippingRule(() => SkipItem7125010)
                .ForItemInstance<int>("7125009")
                .EqualTo(0)
                .SkipItem(GetItemDefinition("7125010"));

            NewRuleSet(() => ItemUpdatedRuleSet7125009, SkipItem7125010);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the skip item7125038.
        /// </summary>
        /// <value>
        /// The skip item7125038.
        /// </value>
        public IItemSkippingRule SkipItem7125038 { get; set; }

        /// <summary>
        /// Gets or sets the item updated rule set7125031.
        /// </summary>
        /// <value>
        /// The item updated rule set7125031.
        /// </value>
        public IRuleSet ItemUpdatedRuleSet7125031 { get; set; }

        /// <summary>
        /// Gets or sets the skip item7125041.
        /// </summary>
        /// <value>
        /// The skip item7125041.
        /// </value>
        public IItemSkippingRule SkipItem7125041 { get; set; }

        /// <summary>
        /// Gets or sets the item updated rule set7125022.
        /// </summary>
        /// <value>
        /// The item updated rule set7125022.
        /// </value>
        public IRuleSet ItemUpdatedRuleSet7125022 { get; set; }

        /// <summary>
        /// Gets or sets the skip item7125009.
        /// </summary>
        /// <value>
        /// The skip item7125009.
        /// </value>
        public IItemSkippingRule SkipItem7125010 { get; set; }

        /// <summary>
        /// Gets or sets the item updated rule set7125010.
        /// </summary>
        /// <value>
        /// The item updated rule set7125010.
        /// </value>
        public IRuleSet ItemUpdatedRuleSet7125009 { get; set; }

        #endregion

        /// <summary>
        /// Gets the item definition.
        /// </summary>
        /// <param name="itemDefinitionCode">The item definition code.</param>
        /// <returns>Returns and ItemDefinition for the itemDefinitionCode if found by key.</returns>
        private ItemDefinition GetItemDefinition(string itemDefinitionCode)
        {
            if (!_nihAssessmentDefinitionKey.HasValue)
            {
                _nihAssessmentDefinitionKey = _assessmentDefinitionRepository.GetKeyByCode(NihHealthBehaviorsAssessment.AssessmentCodedConcept.Code);
            }

            var assessmentDefinition = _assessmentDefinitionRepository.GetByKey(_nihAssessmentDefinitionKey.Value);
            return assessmentDefinition.GetItemDefinitionByCode(itemDefinitionCode);
        }
    }
}
