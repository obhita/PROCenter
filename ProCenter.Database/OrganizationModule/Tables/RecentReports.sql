CREATE TABLE [OrganizationModule].[RecentReports]
(
	[ReportKey] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	[SystemAccountKey] UNIQUEIDENTIFIER NOT NULL,
    [Name] NVARCHAR(100) NOT NULL, 
    [Assessment] NVARCHAR(100) NULL, 
    [RunDate] DATETIME NOT NULL, 
    [PatientKey] UNIQUEIDENTIFIER NULL, 
    [OrganizationKey] UNIQUEIDENTIFIER NOT NULL
)
