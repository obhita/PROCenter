namespace ProCenter.Domain.Nida
{
    #region

    using System;
    using AssessmentModule;
    using Common;
    using MessageModule;
    using Pillar.FluentRuleEngine;

    #endregion

    public class NidaWorkflowRuleCollection : AbstractRuleCollection<AssessmentInstance>
    {
        private readonly IResourcesManager _resourcesManager;

        public NidaWorkflowRuleCollection(IAssessmentDefinitionRepository assessmentDefinitionRepository, IWorkflowMessageRepository repository,
                                          IAssessmentInstanceRepository assessmentInstanceRepository, IResourcesManager resourcesManager = null)
        {
            _resourcesManager = resourcesManager;
            NewRule(() => ShouldRecommendDastRule).When(assessment => (int) assessment.Score.Value > 0)
                                                  .Then((assessment, ctx) =>
                                                      {
                                                          var messageReporter =
                                                              ctx.WorkingMemory.GetContextObject<IMessageCollector>("MessageCollector");
                                                          var assessmentDefinitionKey =
                                                              assessmentDefinitionRepository.GetKeyByCode(DrugAbuseScreeningTest.AssessmentCodedConcept.Code);
                                                          WorkflowMessage message = null;
                                                          if (assessment.WorkflowKey.HasValue)
                                                          {
                                                              message = repository.GetByKey(assessment.WorkflowKey.Value);
                                                          }
                                                          message = message ?? new WorkflowMessage(assessment.PatientKey,
                                                                                                   assessment.Key,
                                                                                                   NidaSingleQuestionScreener.AssessmentCodedConcept.Code,
                                                                                                   assessmentDefinitionKey,
                                                                                                   DrugAbuseScreeningTest.AssessmentCodedConcept.Code,
                                                                                                   assessment.Score);
                                                          if ( assessment.CanSelfAdminister )
                                                          {
                                                              message.AllowSelfAdministration ();
                                                          }
                                                          assessment.AddToWorkflow(message.Key);
                                                          messageReporter.AddMessage(message);
                                                      })
                                                  .ElseThen((assessment, ctx) =>
                                                      {
                                                          WorkflowMessage message = null;
                                                          if (assessment.WorkflowKey.HasValue)
                                                          {
                                                              message = repository.GetByKey(assessment.WorkflowKey.Value);
                                                          }
                                                          message = message ?? new WorkflowMessage(assessment.PatientKey,
                                                                                                   assessment.Key,
                                                                                                   NidaSingleQuestionScreener.AssessmentCodedConcept.Code,
                                                                                                   Guid.Empty,
                                                                                                   null,
                                                                                                   assessment.Score);

                                                          if (assessment.CanSelfAdminister)
                                                          {
                                                              message.AllowSelfAdministration();
                                                          }

                                                          message.Complete(NidaPatientSummaryReportModelBuilder.GetGreenReportModel());
                                                      });

            NewRuleSet(() => NidaSingleQuestionScreenerRuleSet, ShouldRecommendDastRule);


            NewRule(() => ShouldRecommendNidaAssessFurtherRule).When(assessment => assessment.WorkflowKey.HasValue)
                                                               .Then((assessment, ctx) =>
                                                                   {
                                                                       var messageReporter =
                                                                           ctx.WorkingMemory
                                                                              .GetContextObject<IMessageCollector>("MessageCollector");
                                                                       var assessmentDefinitionKey =
                                                                           assessmentDefinitionRepository.GetKeyByCode(NidaAssessFurther.AssessmentCodedConcept.Code);
                                                                       var message = repository.GetByKey(assessment.WorkflowKey.Value);
                                                                       message.Advance(
                                                                           assessment.Key,
                                                                           DrugAbuseScreeningTest.AssessmentCodedConcept.Code,
                                                                           assessmentDefinitionKey,
                                                                           NidaAssessFurther.AssessmentCodedConcept.Code,
                                                                           assessment.Score);
                                                                       messageReporter.AddMessage(message);
                                                                   });

            NewRuleSet(() => DrugAbuseScreeningTestRuleSet, ShouldRecommendNidaAssessFurtherRule);

            NewRule(() => ShouldCompleteWorkflowStatusRule).When(assessment => assessment.WorkflowKey.HasValue)
                                                           .Then((assessment, ctx) =>
                                                               {
                                                                   var message = repository.GetByKey(assessment.WorkflowKey.Value);
                                                                   var dastKey = message.GetAssessmentKeyforCodeInWorkflow(DrugAbuseScreeningTest.AssessmentCodedConcept.Code);
                                                                   var dastInstance = assessmentInstanceRepository.GetByKey(dastKey.Value);
                                                                   ReportModel reportModel;
                                                                   if (((long) dastInstance.Score.Value) <= 2 && !((bool) assessment.Score.Value))
                                                                   {
                                                                       reportModel = NidaPatientSummaryReportModelBuilder.GetAmberReportModel(_resourcesManager,
                                                                                                                                              dastInstance,
                                                                                                                                              assessment);
                                                                   }
                                                                   else
                                                                   {
                                                                       reportModel = NidaPatientSummaryReportModelBuilder.GetRedReportModel(_resourcesManager,
                                                                                                                                            dastInstance,
                                                                                                                                            assessment);
                                                                   }
                                                                   message.Complete(reportModel);
                                                               });
            NewRuleSet(() => NidaAssessFurtherRuleSet, ShouldCompleteWorkflowStatusRule);
        }

        public IRuleSet NidaSingleQuestionScreenerRuleSet { get; private set; }
        public IRuleSet DrugAbuseScreeningTestRuleSet { get; private set; }
        public IRuleSet NidaAssessFurtherRuleSet { get; private set; }

        public IRule ShouldRecommendDastRule { get; set; }
        public IRule ShouldRecommendNidaAssessFurtherRule { get; set; }
        public IRule ShouldCompleteWorkflowStatusRule { get; set; }
    }
}