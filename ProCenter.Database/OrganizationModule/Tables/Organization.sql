CREATE TABLE [OrganizationModule].[Organization](
	[OrganizationKey]	 UNIQUEIDENTIFIER   NOT NULL,
    [Name]               NVARCHAR (500)     NOT NULL,
    PRIMARY KEY CLUSTERED ([OrganizationKey] ASC), 
);