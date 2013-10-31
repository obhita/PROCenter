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
