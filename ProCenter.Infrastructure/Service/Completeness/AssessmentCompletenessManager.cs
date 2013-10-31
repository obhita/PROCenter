#region Licence Header
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
    using System.Linq;
    using Pillar.FluentRuleEngine;
    using ProCenter.Domain.AssessmentModule;
    using ProCenter.Domain.AssessmentModule.Lookups;
    using ProCenter.Service.Message.Metadata;

    public class AssessmentCompletenessManager : IAssessmentCompletenessManager
    {
        #region Fields

        private readonly ICompletenessRuleCollectionFactory _completenessRuleCollectionFactory;
        private readonly IRuleEngineFactory _ruleEngineFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssessmentCompletenessManager" /> class.
        /// </summary>
        /// <param name="completenessRuleCollectionFactory">The completeness rule collection factory.</param>
        /// <param name="ruleEngineFactory">The rule engine factory.</param>
        public AssessmentCompletenessManager(ICompletenessRuleCollectionFactory completenessRuleCollectionFactory, IRuleEngineFactory ruleEngineFactory)
        {
            _completenessRuleCollectionFactory = completenessRuleCollectionFactory;
            _ruleEngineFactory = ruleEngineFactory;
        }

        #endregion

        #region Public Methods and Operators
        
        public CompletenessResults CalculateCompleteness<TAssessment>(string completenessCategory, TAssessment assessment, IContainItemDefinitions itemDefinitionContainer ) where TAssessment : AssessmentInstance
        {
            var completenessResults =  CalculateCompleteness(assessment, itemDefinitionContainer, completenessCategory);
            //todo: to better use completenessCategory
            var ruleCollection = _completenessRuleCollectionFactory.GetCompletenessRuleCollection<TAssessment>(assessment.AssessmentName);
            var ruleEngine = _ruleEngineFactory.CreateRuleEngine(assessment, ruleCollection);
            var skippedQuestionResults = ruleEngine.ExecuteRuleSet(assessment, "CompletenessRuleSet");

            completenessResults.UpdateTotal(completenessResults.Total - skippedQuestionResults.RuleViolations.Count());

            return completenessResults;
        }

        #endregion

        #region Methods

        private static CompletenessResults CalculateCompleteness(AssessmentInstance assessmentInstance, IContainItemDefinitions itemDefinitionContainer, string completenessCategory)
        {
            var total = 0;
            var numbercomplete = 0;
            foreach (var itemDefinition in itemDefinitionContainer.ItemDefinitions)
            {
                total = CalculatePercentComplete(assessmentInstance, itemDefinition, completenessCategory, total, ref numbercomplete);

                 if (itemDefinition.ItemType == ItemType.Group)
                 {
                     total = itemDefinition.ItemDefinitions.Aggregate(total,
                                                                      (current, childItemDefinition) =>
                                                                      CalculatePercentComplete(assessmentInstance, childItemDefinition, completenessCategory, current,
                                                                                                        ref numbercomplete));
                 }
            }
            
            return new CompletenessResults(completenessCategory, total, numbercomplete);
        }

        private static int CalculatePercentComplete(AssessmentInstance assessmentInstance, ItemDefinition itemDefinition, string completenessCategory, int total,
                                                    ref int numberComplete)
        {
            if (itemDefinition.ItemType == ItemType.Question)
            {
                if (itemDefinition.ItemMetadata.MetadataItemExists<RequiredForCompletenessMetadataItem>(m => m.CompletenessCategory == completenessCategory))
                {
                    total++;
                    var itemInstance = assessmentInstance.ItemInstances.FirstOrDefault(i => i.ItemDefinitionCode == itemDefinition.CodedConcept.Code);
                    if (itemInstance != null && itemInstance.Value != null)
                    {
                        numberComplete++;
                    }
                }
            }
            return total;
        }

        #endregion
    }
}