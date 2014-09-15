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

namespace ProCenter.Domain.GainShortScreener
{
    #region Using Statements

    using System;
    using Pillar.FluentRuleEngine;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Rules;

    #endregion

    /// <summary>The gain short screener rule collection class.</summary>
    public class GainShortScreenerRuleCollection : AbstractAssessmentRuleCollection
    {
        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;

        private static Guid? _gainShortScreenerAssessmentDefinitionKey;
        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="GainShortScreenerRuleCollection" /> class.</summary>
        /// <param name="assessmentDefinitionRepository">The assessment definition repository.</param>
        public GainShortScreenerRuleCollection(IAssessmentDefinitionRepository assessmentDefinitionRepository)
        {
            _assessmentDefinitionRepository = assessmentDefinitionRepository;

            NewItemSkippingRule(() => SkipItem6125031)
              .ForItemInstance<bool>("6125030")
              .EqualTo(false)
              .SkipItem(GetItemDefinition("6125031"));

            NewRuleSet(() => ItemUpdatedRuleSet6125030, SkipItem6125031);

            NewItemSkippingRule(() => SkipItem6125033)
              .ForItemInstance<GainShortScreenerGender>("6125032")
              .NotEqualTo(GainShortScreenerGender.Other)
              .SkipItem(GetItemDefinition("6125033"));

            NewRuleSet(() => ItemUpdatedRuleSet6125032, SkipItem6125033);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the should clear6125031.
        /// </summary>
        /// <value>
        /// The should clear6125031.
        /// </value>
        public IItemSkippingRule SkipItem6125031 { get; set; }

        /// <summary>Gets or sets the rule set6125030.</summary>
        /// <value>The rule set6125030.</value>
        public IRuleSet ItemUpdatedRuleSet6125030 { get; set; }

        /// <summary>
        /// Gets or sets the skip item6125033.
        /// </summary>
        /// <value>
        /// The skip item6125033.
        /// </value>
        public IItemSkippingRule SkipItem6125033 { get; set; }

        /// <summary>
        /// Gets or sets the item updated rule set6125032.
        /// </summary>
        /// <value>
        /// The item updated rule set6125032.
        /// </value>
        public IRuleSet ItemUpdatedRuleSet6125032 { get; set; }

        #endregion

        /// <summary>
        /// Gets the item definition.
        /// </summary>
        /// <param name="itemDefinitionCode">The item definition code.</param>
        /// <returns>Returns and ItemDefinition for the itemDefinitionCode if found by key.</returns>
        private ItemDefinition GetItemDefinition(string itemDefinitionCode)
        {
            if (!_gainShortScreenerAssessmentDefinitionKey.HasValue)
            {
                _gainShortScreenerAssessmentDefinitionKey = _assessmentDefinitionRepository.GetKeyByCode(GainShortScreener.AssessmentCodedConcept.Code);
            }

            var assessmentDefinition = _assessmentDefinitionRepository.GetByKey(_gainShortScreenerAssessmentDefinitionKey.Value);
            return assessmentDefinition.GetItemDefinitionByCode(itemDefinitionCode);
        }
    }
}
