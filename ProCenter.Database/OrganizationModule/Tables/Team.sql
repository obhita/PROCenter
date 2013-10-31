CREATE TABLE [OrganizationModule].[Team](
    [TeamKey]           UNIQUEIDENTIFIER             NOT NULL,
	[OrganizationKey]	 UNIQUEIDENTIFIER NOT NULL,
    [Name]        NVARCHAR (500)     NOT NULL,
    PRIMARY KEY CLUSTERED ([TeamKey] ASC), 
);
GO

CREATE NONCLUSTERED INDEX [Team_Organization_IDX]
    ON [OrganizationModule].[Team]([OrganizationKey] ASC);