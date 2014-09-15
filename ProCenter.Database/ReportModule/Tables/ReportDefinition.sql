CREATE TABLE [ReportModule].[ReportDefinition]
(
	[ReportDefinitionKey] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [ReportName] VARCHAR(150) NOT NULL, 
    [DisplayName] VARCHAR(250) NOT NULL, 
    [IsPatientCentric] BIT NOT NULL
)
