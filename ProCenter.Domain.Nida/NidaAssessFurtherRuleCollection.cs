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

namespace ProCenter.Domain.Nida
{
    #region Using Statements

    using System;
    using System.Linq;

    using Pillar.FluentRuleEngine;

    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Event;
    using ProCenter.Domain.AssessmentModule.Rules;

    #endregion

    /// <summary>The nida assess further rule collection class.</summary>
    public class NidaAssessFurtherRuleCollection : AbstractAssessmentRuleCollection
    {
        private readonly IAssessmentDefinitionRepository _assessmentDefinitionRepository;

        private static Guid? _nidaAssessmentDefinitionKey;
        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="NidaAssessFurtherRuleCollection" /> class.</summary>
        /// <param name="assessmentDefinitionRepository">The assessment definition repository.</param>
        public NidaAssessFurtherRuleCollection ( IAssessmentDefinitionRepository assessmentDefinitionRepository)
        {
            _assessmentDefinitionRepository = assessmentDefinitionRepository;

            NewItemSkippingRule ( () => SkipItem3269984 )
                .ForItemInstance<string> ( "3269985" )
                .Null ()
                .SkipItem ( GetItemDefinition ( "3269984" ) );

            NewRuleSet(() => ItemUpdatedRuleSet3269985, SkipItem3269984);

            NewItemSkippingRule ( () => SkipItem3269986 )
                .ForItemInstance<bool> ( "3269978" )
                .EqualTo ( false )
                .SkipItem ( GetItemDefinition ( "3269986" ) );

            NewRuleSet(() => ItemUpdatedRuleSet3269978, SkipItem3269986);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the rule set3269985.
        /// </summary>
        /// <value>
        /// The rule set3269985.
        /// </value>
        public IRuleSet ItemUpdatedRuleSet3269985 { get; set; }

        /// <summary>
        /// Gets or sets the should clear3269984.
        /// </summary>
        /// <value>
        /// The should clear3269984.
        /// </value>
        public IItemSkippingRule SkipItem3269984 { get; set; }

        /// <summary>Gets or sets the rule set3269978.</summary>
        /// <value>The rule set3269978.</value>
        public IRuleSet ItemUpdatedRuleSet3269978 { get; set; }

        /// <summary>Gets or sets the should clear3269986.</summary>
        /// <value>The should clear3269986.</value>
        public IItemSkippingRule SkipItem3269986 { get; set; }

        #endregion

        private ItemDefinition GetItemDefinition(string itemDefinitionCode)
        {
            if (!_nidaAssessmentDefinitionKey.HasValue)
            {
                _nidaAssessmentDefinitionKey = _assessmentDefinitionRepository.GetKeyByCode(NidaAssessFurther.AssessmentCodedConcept.Code);
            }

            var assessmentDefinition = _assessmentDefinitionRepository.GetByKey(_nidaAssessmentDefinitionKey.Value);
            return assessmentDefinition.GetItemDefinitionByCode(itemDefinitionCode);
        }

        private ItemDefinition[] GetItemDefinition(params string[] itemDefinitionCode)
        {
            return itemDefinitionCode.Select(GetItemDefinition).ToArray();
        }
    }
}
