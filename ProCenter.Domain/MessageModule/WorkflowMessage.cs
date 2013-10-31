namespace ProCenter.Domain.MessageModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AssessmentModule;
    using CommonModule;
    using Event;
    using Pillar.Common.Utility;

    #endregion

    public class WorkflowMessage : AggregateRootBase, IMessage
    {
        private Dictionary<string,Guid> _workflowAssessments = new Dictionary<string, Guid> ();

        public WorkflowMessage()
        {
            WorkflowReports = new List<ReportModel>();
        }

        public WorkflowMessage(Guid patientKey,
                               Guid initiatingAssessmentKey,
                               string initiatingAssessmentCode,
                               Guid recommendedAssessmentDefinitionKey,
                               string recommendedAssessmentDefinitionCode,
                               Score initiatingAssessmentScore)
        {
            Key = CombGuid.NewCombGuid();
            RaiseEvent(new WorkflowMessageCreatedEvent(Key,
                                                       MessageType,
                                                       patientKey,
                                                       WorkflowMessageStatus.WaitingForResponse, 
                                                       initiatingAssessmentKey,
                                                       initiatingAssessmentCode,
                                                       recommendedAssessmentDefinitionKey,
                                                       recommendedAssessmentDefinitionCode,
                                                       initiatingAssessmentScore));
            WorkflowReports = new List<ReportModel> ();
        }

        public Guid InitiatingAssessmentKey { get; private set; }
        public string InitiatingAssessmentCode { get; private set; }
        public Guid RecommendedAssessmentDefinitionKey { get; private set; }
        public string RecommendedAssessmentDefinitionCode { get; private set; }
        public WorkflowMessageStatus Status { get; private set; }
        public Guid PatientKey { get; private set; }
        public Score InitiatingAssessmentScore { get; private set; }
        public IEnumerable<ReportModel> WorkflowReports { get; private set; }

        public bool ForSelfAdministration { get; private set; }

        public MessageType MessageType
        {
            get { return MessageType.RecommendAssessment; }
        }

        public void Reject()
        {
            if (Status == WorkflowMessageStatus.WaitingForResponse)
            {
                RaiseEvent(new WorkflowMessageStatusChangedEvent(Key, MessageType, WorkflowMessageStatus.Rejected));
            }
            else
            {
                //TODO: throw error
            }
        }

        public void AdministerAssessment()
        {
            if (Status == WorkflowMessageStatus.WaitingForResponse)
            {
                RaiseEvent(new WorkflowMessageStatusChangedEvent(Key, MessageType, WorkflowMessageStatus.InProgress));
            }
            else
            {
                //TODO: throw error
            }
        }

        public void Complete(params ReportModel[] workflowReports)
        {
            if (Status == WorkflowMessageStatus.InProgress)
            {
                RaiseEvent(new WorkflowMessageStatusChangedEvent(Key, MessageType, WorkflowMessageStatus.Complete));
                if ( workflowReports != null )
                {
                    foreach ( var workflowReport in workflowReports )
                    {
                        RaiseEvent(new WorkflowMessageReportReadyEvent(Key, MessageType, workflowReport, PatientKey));
                    }
                }
            }
        }

        public void Advance(
            Guid initiatingAssessmentKey,
            string initiatingAssessmentCode,
            Guid recommendedAssessmentDefinitionKey,
            string recommendedAssessmentDefinitionCode,
            Score initiatingAssessmentScore)
        {
            if (Status == WorkflowMessageStatus.InProgress)
            {
                RaiseEvent(new WorkflowMessageAdvancedEvent(Key,
                                                            MessageType, 
                                                            initiatingAssessmentKey,
                                                            initiatingAssessmentCode,
                                                            recommendedAssessmentDefinitionKey,
                                                            recommendedAssessmentDefinitionCode,
                                                            initiatingAssessmentScore));

                RaiseEvent(new WorkflowMessageStatusChangedEvent(Key, MessageType, WorkflowMessageStatus.WaitingForResponse));
            }
        }

        public Guid? GetAssessmentKeyforCodeInWorkflow ( string assessmentCode )
        {
            if ( _workflowAssessments.ContainsKey ( assessmentCode ) )
            {
                return _workflowAssessments[assessmentCode];
            }
            return null;
        }

        public void UpdateReportItem ( string reportName, string name, bool? shouldShow, string text )
        {
            RaiseEvent ( new WorkflowMessageReportItemUpdatedEvent ( Key, MessageType, reportName, name, shouldShow, text ) );
        }

        public void AllowSelfAdministration()
        {
            RaiseEvent(new MessageForSelfAdministrationEvent(Key, MessageType));
        }

        private void Apply ( MessageForSelfAdministrationEvent messageForSelfAdministrationEvent )
        {
            ForSelfAdministration = true;
        }

        private void Apply(WorkflowMessageReportItemUpdatedEvent workflowMessageReportItemUpdatedEvent)
        {
            var report = WorkflowReports.FirstOrDefault ( r => r.Name == workflowMessageReportItemUpdatedEvent.ReportName );
            if ( report != null )
            {
                report.UpdateReportItem ( workflowMessageReportItemUpdatedEvent.Name, workflowMessageReportItemUpdatedEvent.ShouldShow, workflowMessageReportItemUpdatedEvent.Text );
            }
        }

        private void Apply ( WorkflowMessageReportReadyEvent workflowMessageReportReadyEvent )
        {
            (WorkflowReports as IList<ReportModel> ).Add(workflowMessageReportReadyEvent.WorkflowReport);
        }

        private void Apply(WorkflowMessageStatusChangedEvent workflowMessageStatusChangedEvent)
        {
            Status = workflowMessageStatusChangedEvent.Status;
        }

        private void Apply(WorkflowMessageCreatedEvent workflowMessageCreatedEvent)
        {
            PatientKey = workflowMessageCreatedEvent.PatientKey;
            InitiatingAssessmentKey = workflowMessageCreatedEvent.InitiatingAssessmentKey;
            InitiatingAssessmentCode = workflowMessageCreatedEvent.InitiatingAssessmentCode;
            RecommendedAssessmentDefinitionKey = workflowMessageCreatedEvent.RecommendedAssessmentDefinitionKey;
            RecommendedAssessmentDefinitionCode = workflowMessageCreatedEvent.RecommendedAssessmentDefinitionCode;
            InitiatingAssessmentScore = workflowMessageCreatedEvent.InitiatingAssessmentScore;
            _workflowAssessments.Add ( InitiatingAssessmentCode, InitiatingAssessmentKey );
            if ( RecommendedAssessmentDefinitionCode == null )
            {
                Status = WorkflowMessageStatus.InProgress;
            }
        }

        private void Apply(WorkflowMessageAdvancedEvent workflowMessageAdvancedEvent)
        {
            InitiatingAssessmentKey = workflowMessageAdvancedEvent.InitiatingAssessmentKey;
            InitiatingAssessmentCode = workflowMessageAdvancedEvent.InitiatingAssessmentCode;
            RecommendedAssessmentDefinitionKey = workflowMessageAdvancedEvent.RecommendedAssessmentDefinitionKey;
            RecommendedAssessmentDefinitionCode = workflowMessageAdvancedEvent.RecommendedAssessmentDefinitionCode;
            InitiatingAssessmentScore = workflowMessageAdvancedEvent.InitiatingAssessmentScore;
            _workflowAssessments.Add(InitiatingAssessmentCode, InitiatingAssessmentKey);
        }
    }
}