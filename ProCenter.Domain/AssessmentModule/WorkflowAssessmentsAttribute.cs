namespace ProCenter.Domain.AssessmentModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;

    #endregion

    public class WorkflowAssessmentsAttribute : Attribute
    {
        public WorkflowAssessmentsAttribute(params string[] assessmentNames)
        {
            AssessmentNames = assessmentNames;
        }

        public IEnumerable<string> AssessmentNames { get; private set; }
    }
}