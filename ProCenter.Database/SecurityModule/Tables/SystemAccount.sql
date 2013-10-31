CREATE TABLE [SecurityModule].[SystemAccount]
(
	[SystemAccountKey] UNIQUEIDENTIFIER NOT NULL,
	[OrganizationKey] UNIQUEIDENTIFIER NOT NULL,
	[StaffKey] UNIQUEIDENTIFIER NULL,
	[PatientKey] UNIQUEIDENTIFIER NULL,
	[Identifier] NVARCHAR(100) NOT NULL,
	[Email] NVARCHAR(100) NOT NULL,
	PRIMARY KEY CLUSTERED([SystemAccountKey] ASC),
);
GO
CREATE NONCLUSTERED INDEX [SystemAccount_OrganizationKey_IDX]
	ON [SecurityModule].[SystemAccount]([OrganizationKey] ASC);
