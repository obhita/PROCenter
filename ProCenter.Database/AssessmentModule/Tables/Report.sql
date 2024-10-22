﻿CREATE TABLE [AssessmentModule].[Report]
(
	[ReportKey] UNIQUEIDENTIFIER NOT NULL,
	[SourceKey] UNIQUEIDENTIFIER NOT NULL,
	[CreatedTimestamp] datetime2 NOT NULL,
	[Name] nvarchar(100) NOT NULL,
	[NameFormat] nvarchar(2000) NULL,
	[CanCustomize] bit NOT NULL,
	[PatientKey] UNIQUEIDENTIFIER NULL,
	[ReportSeverity] int NULL,
	[ReportType] int NULL,
	[ReportStatus] nvarchar(100) NULL,
	[IsPatientViewable] bit NOT NULL,
    [OrganizationKey] UNIQUEIDENTIFIER NOT NULL, 
    PRIMARY KEY CLUSTERED ([ReportKey] ASC)
)
