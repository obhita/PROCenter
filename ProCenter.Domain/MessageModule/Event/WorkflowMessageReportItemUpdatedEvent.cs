namespace ProCenter.Domain.MessageModule.Event
{
    #region Using Statements

    using System;
    using AssessmentModule;

    #endregion

    public class WorkflowMessageReportItemUpdatedEvent : MessageEventBase
    {

        #region Constructors and Destructors

        public WorkflowMessageReportItemUpdatedEvent(Guid key, MessageType messageType, string reportName, string name, bool? shouldShow, string text)
            : base ( key, messageType )
        {
            ReportName = reportName;
            Name = name;
            ShouldShow = shouldShow;
            Text = text;
        }

        #endregion

        #region Public Properties

        public string ReportName { get; private set; }
        public string Name { get; private set; }
        public bool? ShouldShow { get; private set; }
        public string Text { get; private set; }

        #endregion
    }
}