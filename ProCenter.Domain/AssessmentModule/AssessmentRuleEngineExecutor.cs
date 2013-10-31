using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCenter.Domain.AssessmentModule
{
    using System.Linq.Expressions;
    using CommonModule;
    using Pillar.Common.InversionOfControl;
    using Pillar.FluentRuleEngine;

    public static class AssessmentRuleEngineExecutor 
    {
        public static RuleEngineExecutor<AssessmentInstance> CreateRuleEngineExecutor(AssessmentInstance assessmentInstance)
        {
            var ruleCollection = (IRuleCollection<AssessmentInstance>)IoC.CurrentContainer.TryResolve(typeof(IAssessmentRuleCollection), assessmentInstance.AssessmentName) ?? new EmptyRuleCollection<AssessmentInstance> ();
            var ruleEngineExecutor = new RuleEngineExecutor<AssessmentInstance> ( assessmentInstance, ruleCollection );
            return ruleEngineExecutor;
        }

        public static RuleEngineExecutor<AssessmentInstance> ForItemDefinitionCode ( this RuleEngineExecutor<AssessmentInstance> ruleEngineExecutor, string itemDefinitionCode )
        {
            return ruleEngineExecutor.ForRuleSet ( "RuleSet" + itemDefinitionCode );
        }
   } 
}
