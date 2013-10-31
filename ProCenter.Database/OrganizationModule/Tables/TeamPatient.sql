CREATE TABLE [OrganizationModule].[TeamPatient](
    [TeamPatientKey]           UNIQUEIDENTIFIER             NOT NULL,
    [TeamKey]           UNIQUEIDENTIFIER             NOT NULL,
    [PatientKey]           UNIQUEIDENTIFIER             NOT NULL,
    [FirstName]        NVARCHAR (500)     NOT NULL,
    [LastName]         NVARCHAR (500)     NOT NULL,
    [OrganizationKey] UNIQUEIDENTIFIER NOT NULL, 
    PRIMARY KEY CLUSTERED ([TeamPatientKey] ASC), 
);
GO

CREATE NONCLUSTERED INDEX [TeamPatient_Team_IDX]
    ON [OrganizationModule].[TeamPatient]([TeamKey] ASC);
GO

CREATE NONCLUSTERED INDEX [TeamPatient_Staff_IDX]
    ON [OrganizationModule].[TeamPatient]([PatientKey] ASC);