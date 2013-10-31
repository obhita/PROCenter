CREATE TABLE [SecurityModule].[Role]
(
	[RoleKey] UNIQUEIDENTIFIER NOT NULL,
	[OrganizationKey] UNIQUEIDENTIFIER NULL,
	[Name] NVARCHAR(100) NOT NULL,
	[RoleType] INT NOT NULL, 
    PRIMARY KEY CLUSTERED ([RoleKey] ASC) ,
);
GO

CREATE NONCLUSTERED INDEX [Role_Oragnization_FK_IDX]
	ON [SecurityModule].[Role]([OrganizationKey] ASC);
