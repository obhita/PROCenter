namespace ProCenter.Infrastructure.Service.ReadSideService
{
    #region

    using System;
    using Dapper;
    using Pillar.Common.Metadata;
    using Pillar.Common.Utility;
    using ProCenter.Domain.MessageModule.Event;

    #endregion

    public class WorkflowReportUpdater : IHandleMessages<WorkflowMessageReportReadyEvent>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public WorkflowReportUpdater(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Handle(WorkflowMessageReportReadyEvent message)
        {
            if (message.WorkflowReport != null)
            {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    connection.Execute(
                        "insert into AssessmentModule.Report values(@ReportKey, @SourceKey, @CreatedTimestamp, @Name, @CanCustomize, @PatientKey, @ReportSeverity, @ReportStatus, @IsPatientViewable, @OrganizationKey)",
                        new
                            {
                                ReportKey = CombGuid.NewCombGuid(),
                                SourceKey = message.Key,
                                CreatedTimestamp = DateTime.Now,
                                message.WorkflowReport.Name,
                                CanCustomize = message.WorkflowReport.IsCustomizable,
                                message.PatientKey,
                                message.WorkflowReport.ReportSeverity,
                                message.WorkflowReport.ReportStatus,
                                message.WorkflowReport.IsPatientViewable,
                                message.OrganizationKey
                            });
                }
            }
        }
    }
}