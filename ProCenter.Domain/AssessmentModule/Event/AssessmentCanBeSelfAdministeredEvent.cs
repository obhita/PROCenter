namespace ProCenter.Domain.AssessmentModule.Event
{
    using System;
    using CommonModule;

    /// <summary>
    /// Assessment can be self administered event.
    /// </summary>
    public class AssessmentCanBeSelfAdministeredEvent : CommitEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentCanBeSelfAdministeredEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        public AssessmentCanBeSelfAdministeredEvent ( Guid key, int version )
            : base ( key, version )
        {
        }
    }
}
