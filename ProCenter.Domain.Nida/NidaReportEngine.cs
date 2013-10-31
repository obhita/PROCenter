namespace ProCenter.Domain.Nida
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AssessmentModule;
    using Common.Report;
    using CommonModule;
    using MessageModule;

    #endregion

    [WorkflowReports(ReportNames.NidaPatientSummaryReport)]
    public class NidaReportEngine : IReportEngine
    {
        private readonly IWorkflowMessageRepository _workflowMessageRepository;

        public NidaReportEngine (IWorkflowMessageRepository workflowMessageRepository)
        {
            _workflowMessageRepository = workflowMessageRepository;
        }

        public IReport Generate(Guid workflowKey, string reportName)
        {
            var workflow = _workflowMessageRepository.GetByKey ( workflowKey );
            var reportModel = workflow.WorkflowReports.FirstOrDefault(r => r.Name == reportName);
            if (reportModel != null)
            {
                NidaPatientSummaryReportModelBuilder.FillDefaults(reportModel);
            }
            //todo: get report data from event store
            var nidaReportDataCollection = new NidaReportDataCollection
                {
                    reportModel == null ? new NidaReportData() : new NidaReportData (reportModel)
                };
            var report = new NidaReport
                {
                    DataSource = nidaReportDataCollection,
                };
            return report;
        }

        public ReportModel GetCustomizationModel(Guid workflowKey, string reportName)
        {
            var reportModel = _workflowMessageRepository.GetByKey ( workflowKey ).WorkflowReports.FirstOrDefault( r => r.Name == reportName);
            if ( reportModel != null )
            {
                NidaPatientSummaryReportModelBuilder.FillDefaults ( reportModel );
            }
            return reportModel;
        }

        public void UpdateCustomizationModel(Guid workflowKey, string reportName, string name, bool? shouldShow, string text)
        {
            var workflowMessage = _workflowMessageRepository.GetByKey ( workflowKey );
            var reportItem = workflowMessage.WorkflowReports.First ( r => r.Name == reportName ).FindReportItem ( name );
            var defaultText = (reportItem.FormatParameters.Any ()
                                  ? string.Format ( NidaWorkflowPatientSummaryReport.ResourceManager.GetString ( reportItem.Name ) ?? string.Empty,
                                                    reportItem.FormatParameters.ToArray () )
                                  : NidaWorkflowPatientSummaryReport.ResourceManager.GetString ( reportItem.Name )) ?? string.Empty;
            if ( defaultText.Equals ( text ) )
            {
                text = null;
            }
            workflowMessage.UpdateReportItem ( reportName, name, shouldShow, text );
        }
    }
}