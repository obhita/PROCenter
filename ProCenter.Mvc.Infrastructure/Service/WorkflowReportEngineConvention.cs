namespace ProCenter.Mvc.Infrastructure.Service
{
    #region

    using System;
    using System.Linq;
    using Domain.AssessmentModule;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    #endregion

    public class WorkflowReportEngineConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (typeof (IReportEngine).IsAssignableFrom(type))
            {
                var assessmentNames = type.GetCustomAttributes(typeof (WorkflowReportsAttribute), true).SelectMany(attr => ((WorkflowReportsAttribute) attr).AssessmentNames);
                foreach (var assessmentName in assessmentNames)
                {
                    registry.For(typeof (IReportEngine)).Add(type).Named(assessmentName);
                }
            }
        }
    }
}