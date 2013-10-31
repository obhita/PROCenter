CREATE TABLE [MessageModule].[AssessmentReminder]
(
	[AssessmentReminderKey] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [OrganizationKey] UNIQUEIDENTIFIER NOT NULL,
	[PatientKey] UNIQUEIDENTIFIER NOT NULL, 
    [PatientFirstName] NVARCHAR(50) NOT NULL, 
    [PatientLastName] NVARCHAR(50) NOT NULL, 
    [CreatedByStaffKey] UNIQUEIDENTIFIER NOT NULL, 
    [AssessmentDefinitionKey] UNIQUEIDENTIFIER NOT NULL, 
	[AssessmentName] NVARCHAR(100) NOT NULL, 
    [AssessmentCode] NVARCHAR(50) NOT NULL, 
    [Title] NVARCHAR(500) NULL, 
    [Start] DATETIME NOT NULL, 
	[End] DATETIME NULL, 
    [Status] NVARCHAR(50) NOT NULL, 
    [ReminderDays] float NOT NULL, 
	[SendToEmail] NVARCHAR(200) NULL, 
    [AlertSentDate] DATETIME NULL,
	[ForSelfAdministration] bit NULL
);
GO

CREATE NONClustered INDEX [AssessmentReminder_Patient_FK_IDX] 
ON [MessageModule].[AssessmentReminder] ([PatientKey] ASC);
GO

CREATE NONClustered INDEX [AssessmentReminder_Staff_FK_IDX] 
ON [MessageModule].[AssessmentReminder] ([CreatedByStaffKey] ASC);
GO

CREATE NONClustered INDEX [AssessmentReminder_AssessmentDefinition_FK_IDX] 
ON [MessageModule].[AssessmentReminder] ([AssessmentDefinitionKey] ASC);
GO

CREATE NONClustered INDEX [AssessmentReminder_Organization_FK_IDX] 
ON [MessageModule].[AssessmentReminder] ([OrganizationKey] ASC);
GO