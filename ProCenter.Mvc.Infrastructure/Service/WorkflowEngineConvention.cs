namespace ProCenter.Mvc.Infrastructure.Service
{
    #region Using Statements

    using System;
    using System.Linq;
    using Domain.AssessmentModule;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    #endregion

    public class WorkflowEngineConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (typeof (IWorkflowEngine).IsAssignableFrom(type))
            {
                var assessmentNames =
                    type.GetCustomAttributes(typeof (WorkflowAssessmentsAttribute), true)
                        .SelectMany(attr => ((WorkflowAssessmentsAttribute) attr).AssessmentNames);
                foreach (var assessmentName in assessmentNames)
                {
                    registry.For(typeof (IWorkflowEngine)).Add(type).Named(assessmentName);
                }
            }
        }
    }
}