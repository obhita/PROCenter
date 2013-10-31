namespace ProCenter.Domain.AssessmentModule
{
    public interface IWorkflowEngine
    {
        void Run(AssessmentInstance assessmentInstance);
    }
}