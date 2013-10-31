namespace ProCenter.Domain.AssessmentModule
{
    using System;
    using CommonModule;

    public interface IReportEngine
    {
        IReport Generate(Guid key, string reportName);

        ReportModel GetCustomizationModel(Guid key, string reportName);

        void UpdateCustomizationModel(Guid key, string reportName, string name, bool? shouldShow, string text);
    }
}