CREATE TABLE [AssessmentModule].[AssessmentInstance] (
    [AssessmentInstanceKey]           UNIQUEIDENTIFIER             NOT NULL,
	[AssessmentName]	 NVARCHAR (500)     NOT NULL,
	[AssessmentCode]	 NVARCHAR (50)     NOT NULL,
	[OrganizationKey]           UNIQUEIDENTIFIER             NOT NULL,
	[PatientKey]           UNIQUEIDENTIFIER             NOT NULL,
    [PercentComplete]            FLOAT             NULL,
	[CreatedTime] DATETIME NOT NULL,
	[LastModifiedTime] DATETIME NOT NULL,
	[IsSubmitted]	BIT NULL,
	[CanSelfAdminister] bit NULL,
    [EmailSentDate] DATETIME NULL, 
    [EmailFailedDate] DATETIME NULL, 
    PRIMARY KEY CLUSTERED ([AssessmentInstanceKey] ASC),  
	CONSTRAINT [AssessmentIntance_Patient_FK] FOREIGN KEY ([PatientKey]) REFERENCES [PatientModule].[Patient] ([PatientKey])
);
GO

CREATE NONCLUSTERED INDEX [AssessmentIntance_Patient_FK_IDX]
    ON [AssessmentModule].[AssessmentInstance]([PatientKey] ASC);
	GO
	CREATE NONCLUSTERED INDEX [AssessmentIntance_Organization_FK_IDX]
    ON [AssessmentModule].[AssessmentInstance]([OrganizationKey] ASC);

