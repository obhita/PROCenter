namespace ProCenter.Domain.Nida
{
    using AssessmentModule;
    using System.Linq;
    using CommonModule;

    public class DrugAbuseScreeningTestScoringEngine : IScoringEngine
    {
        public string AssessmentName { get { return DrugAbuseScreeningTest.AssessmentCodedConcept.Name; } }

        public void CalculateScore(AssessmentInstance assessment)
        {
            var value = assessment.ItemInstances.Count(i => (bool.Parse(i.Value.ToString())));

            var guidance = value <= 2
                               ? new CodedConcept(CodeSystems.Obhita, "guidance_0_to_2", "guidance_0_to_2")
                               : new CodedConcept(CodeSystems.Obhita, "guidance_3_and_up", "guidance_3_and_up");

            assessment.ScoreComplete(new CodedConcept(CodeSystems.Obhita, "", ""), value, guidance);
        }
    }
}