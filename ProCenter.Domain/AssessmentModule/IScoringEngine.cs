namespace ProCenter.Domain.AssessmentModule
{
    public interface IScoringEngine
    {
        string AssessmentName { get; }

        void CalculateScore(AssessmentInstance assessment);
    }
}