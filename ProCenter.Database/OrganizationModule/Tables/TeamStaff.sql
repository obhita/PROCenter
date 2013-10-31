CREATE TABLE [OrganizationModule].[TeamStaff](
    [TeamStaffKey]           UNIQUEIDENTIFIER             NOT NULL,
    [TeamKey]           UNIQUEIDENTIFIER             NOT NULL,
    [StaffKey]           UNIQUEIDENTIFIER             NOT NULL,
    [FirstName]        NVARCHAR (500)     NOT NULL,
    [LastName]         NVARCHAR (500)     NOT NULL,
    [OrganizationKey] UNIQUEIDENTIFIER NOT NULL, 
    PRIMARY KEY CLUSTERED ([TeamStaffKey] ASC), 
);
GO

CREATE NONCLUSTERED INDEX [TeamStaff_Team_IDX]
    ON [OrganizationModule].[TeamStaff]([TeamKey] ASC);
GO

CREATE NONCLUSTERED INDEX [TeamStaff_Staff_IDX]
    ON [OrganizationModule].[TeamStaff]([StaffKey] ASC);