namespace ProCenter.Domain.Nida
{
    using System.Linq;
    using AssessmentModule;
    using CommonModule;

    public  class NidaSingleQuestionScreenerScoringEngine: IScoringEngine
    {
        public string AssessmentName { get { return NidaSingleQuestionScreener.AssessmentCodedConcept.Name; } }

        public void CalculateScore(AssessmentInstance assessment)
        {
            var value = int.Parse(assessment.ItemInstances.First().Value.ToString());
            var guidance = value > 0
                               ? new CodedConcept(CodeSystems.Obhita, "guidance_1_and_up", "guidance_1_and_up")
                               : new CodedConcept(CodeSystems.Obhita, "guidance_0", "guidance_0");
            assessment.ScoreComplete(new CodedConcept(CodeSystems.Obhita,"",""), value , guidance );
        }
    }
}