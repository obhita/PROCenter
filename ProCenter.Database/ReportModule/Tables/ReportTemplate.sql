CREATE TABLE [ReportModule].[ReportTemplate] (
    [ReportTemplateKey]  UNIQUEIDENTIFIER             NOT NULL,
	[SystemAccountKey] UNIQUEIDENTIFIER             NOT NULL,
	[Name]	NVARCHAR(500) NOT NULL,
    [ReportType]            NVARCHAR(50)             NOT NULL,
	[Parameters]	NVARCHAR(2000) NOT NULL,
	[ReportStateCode]         NVARCHAR(50)    NOT NULL,
    [PatientKey] UNIQUEIDENTIFIER NULL, 
    [OrganizationKey] UNIQUEIDENTIFIER NOT NULL, 
    PRIMARY KEY CLUSTERED ([ReportTemplateKey] ASC),  
);
GO
CREATE NONCLUSTERED INDEX [ReportTemplate_Patient_FK_IDX]
ON [PatientModule].[Patient]([PatientKey] ASC);
GO