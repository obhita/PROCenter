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
namespace ProCenter.Domain.Nida
{
    using System.Linq;
    using AssessmentModule;
    using Pillar.FluentRuleEngine;

    public class NidaAssessFurtherReportCompletenessRuleCollection : AbstractRuleCollection<AssessmentInstance>, ICompletenessRuleCollection<AssessmentInstance>
    {
        public NidaAssessFurtherReportCompletenessRuleCollection()
        {
            AutoValidatePropertyRules = true;
            //NewRule(() => Rule3269984).When(a => string.IsNullOrWhiteSpace(a.ItemInstances.FirstOrDefault(ins => ins.ItemDefinitionCode == "3269985").Value.ToString()))
            //                          .Then((a, ctx) =>
            //                              {
            //                                  var completenessManager = ctx.WorkingMemory.GetContextObject<IAssessmentCompletenessManager>();
            //                                  completenessManager.DecreasTotalCount();
            //                              });
            
            NewRule(() => OtherDrugUseFrequencyRule3269984)
                .When((a, ctx) =>
                    {
                        var item = GetItemByCode(a, "3269985");
                        if (item == null || item.Value == null)
                        {
                            return true;
                        }
                        return string.IsNullOrWhiteSpace((string) (item.Value));
                    }).ThenReportRuleViolation((a, ctx) => "");
          
            NewRule(() => LastTimeInjectedRule3269986)
                .When((a, ctx) =>
                    {
                        var item = GetItemByCode(a, "3269978");
                        if (item == null || item.Value == null)
                        {
                            return true;
                        }
                        return !bool.Parse(item.Value.ToString());
                    }).ThenReportRuleViolation((a, ctx) => "");

            NewRuleSet(() => CompletenessRuleSet, new[] {OtherDrugUseFrequencyRule3269984, LastTimeInjectedRule3269986});
        }


        public IRule OtherDrugUseFrequencyRule3269984 { get; private set; }
        public IRule LastTimeInjectedRule3269986 { get; private set; }

        public IRuleSet CompletenessRuleSet { get; private set; }

        public string CompletenessCategory
        {
            get { return AssessmentModule.CompletenessCategory.Report; }
        }

        private static ItemInstance GetItemByCode(AssessmentInstance assessment, string itemDefinitionCode )
        {
            return assessment.ItemInstances.FirstOrDefault(i => i.ItemDefinitionCode == itemDefinitionCode);
        }
    }
}