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