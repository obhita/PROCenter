CREATE TABLE [PatientModule].[Patient] (
    [PatientKey]           UNIQUEIDENTIFIER             NOT NULL,
	[OrganizationKey]	UNIQUEIDENTIFIER not null,
    [GenderCode]            NVARCHAR(50)             NOT NULL,
    [FirstName]        NVARCHAR (500)     NOT NULL,
    [LastName]         NVARCHAR (500)     NOT NULL,
    [UniqueIdentifier] NVARCHAR(50) NOT NULL, 
    [DateOfBirth] DATETIME NOT NULL, 
    PRIMARY KEY CLUSTERED ([PatientKey] ASC),  
);
GO
CREATE NONCLUSTERED INDEX [Patient_Organization_FK_IDX]
ON [PatientModule].[Patient]([OrganizationKey] ASC);
GO
