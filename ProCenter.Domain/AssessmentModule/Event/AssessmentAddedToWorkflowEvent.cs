namespace ProCenter.Domain.AssessmentModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     Assessment added to workflow event.
    /// </summary>
    public class AssessmentAddedToWorkflowEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssessmentAddedToWorkflowEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="workflowKey">The workflow key.</param>
        public AssessmentAddedToWorkflowEvent ( Guid key, int version, Guid workflowKey )
            : base ( key, version )
        {
            WorkflowKey = workflowKey;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the workflow key.
        /// </summary>
        /// <value>
        ///     The workflow key.
        /// </value>
        public Guid WorkflowKey { get; private set; }

        #endregion
    }
}