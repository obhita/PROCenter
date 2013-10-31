namespace ProCenter.Domain.AssessmentModule
{
    #region

    using System;
    using System.Collections.Generic;

    #endregion

    public class WorkflowReportsAttribute : Attribute
    {
        public IEnumerable<string> AssessmentNames { get; set; }

        public WorkflowReportsAttribute(params string[] assessmentNames)
        {
            AssessmentNames = assessmentNames;
        }
    }
}