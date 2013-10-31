CREATE TABLE [OrganizationModule].[Staff](
    [StaffKey]           UNIQUEIDENTIFIER             NOT NULL,
	[OrganizationKey]	 UNIQUEIDENTIFIER NOT NULL,
    [FirstName]        NVARCHAR (500)     NOT NULL,
    [LastName]         NVARCHAR (500)     NOT NULL,
	[Email]            NVARCHAR(100),
	[Location]	NVARCHAR(100),
	[NPI] NVARCHAR(50),
    PRIMARY KEY CLUSTERED ([StaffKey] ASC), 
);
GO

CREATE NONCLUSTERED INDEX [Staff_Organization_FK_IDX]
    ON [OrganizationModule].[Staff]([OrganizationKey] ASC);

