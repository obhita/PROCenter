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

    using AssessmentModule;
    using MessageModule;
    using Pillar.Common.InversionOfControl;
    using Pillar.FluentRuleEngine;
    using Pillar.FluentRuleEngine.RuleSelectors;

    #endregion

    [WorkflowAssessments("NidaSingleQuestionScreener", "DrugAbuseScreeningTest", "NidaAssessFurther")]
    public class NidaWorkflowEngine : IWorkflowEngine
    {
        private readonly IMessageCollector _messageCollector;
        private readonly IRuleCollectionFactory _ruleCollectionFactory;
        private readonly IRuleEngineFactory _ruleEngineFactory;

        public NidaWorkflowEngine(IRuleCollectionFactory ruleCollectionFactory, IRuleEngineFactory ruleEngineFactory,
                                  IMessageCollector messageCollector)
        {
            _ruleCollectionFactory = ruleCollectionFactory;
            _ruleEngineFactory = ruleEngineFactory;
            _messageCollector = messageCollector;
        }

        public void Run(AssessmentInstance assessmentInstance)
        {
            //TODO:If Required
            //Need to update pillar to allow for named rule collections _ruleCollectionFactory.CreateRuleCollection<AssessmentInstance>("NidaWorkflow");
            var ruleCollection = IoC.CurrentContainer.Resolve<NidaWorkflowRuleCollection>();
            var ruleEngine = _ruleEngineFactory.CreateRuleEngine(assessmentInstance, ruleCollection);
            var ruleEngineContext = new RuleEngineContext<AssessmentInstance>(assessmentInstance, new SelectAllRulesInRuleSetSelector(assessmentInstance.AssessmentName + "RuleSet"));
            ruleEngineContext.WorkingMemory.AddContextObject(_messageCollector, "MessageCollector");
            ruleEngine.ExecuteRules(ruleEngineContext);
        }
    }
}