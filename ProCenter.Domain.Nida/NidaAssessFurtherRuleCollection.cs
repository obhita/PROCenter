using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCenter.Domain.Nida
{
    using AssessmentModule;
    using AssessmentModule.Event;
    using Pillar.FluentRuleEngine;

    public class NidaAssessFurtherRuleCollection : AbstractRuleCollection<AssessmentInstance>, IAssessmentRuleCollection
    {
        public NidaAssessFurtherRuleCollection ()
        {
            NewRule ( () => ShouldClear3269984 ).OnContextObject<ItemInstance> ()
                                               .When ( (assessment,ctx) =>
                                                   {
                                                       var itemUpdatedEvent = ctx.WorkingMemory.GetContextObject<ItemUpdatedEvent> ();
                                                       var itemHasValue =
                                                           assessment.ItemInstances.Any (
                                                                                         i =>
                                                                                         i.ItemDefinitionCode == "3269984" &&
                                                                                         !( i.Value == null || string.IsNullOrWhiteSpace ( i.Value.ToString () ) ) );
                                                       if ( itemUpdatedEvent.Value == null )
                                                       {
                                                           return itemHasValue;
                                                       }
                                                       return itemHasValue && string.IsNullOrWhiteSpace ( itemUpdatedEvent.Value.ToString () );
                                                   } )
                                                   .Then ( assessment => assessment.UpdateItem("3269984", null) );

            NewRuleSet ( () => RuleSet3269985, ShouldClear3269984 );
        }

        public IRuleSet RuleSet3269985 { get; set; }

        public IRule ShouldClear3269984 { get; set; }
    }
}
