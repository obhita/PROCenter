CREATE TABLE [AssessmentModule].[AssessmentInstanceResponse] (
    [AssessmentInstanceKey]           UNIQUEIDENTIFIER             NOT NULL,
	[PatientKey]           UNIQUEIDENTIFIER             NOT NULL,
	[ItemDefinitionCode] NVARCHAR(50)	NOT NULL,
	[AssessmentName]	 NVARCHAR (500)     NOT NULL,
	[AssessmentCode]	 NVARCHAR (50)     NOT NULL,
	[AssessmentDefinitionKey]	UNIQUEIDENTIFIER NOT NULL,
	[OrganizationKey]           UNIQUEIDENTIFIER             NOT NULL,
	[ResponseType]	NVARCHAR(500) NOT NULL,
	[ResponseValue] NVARCHAR(500)
    ,  
	[IsCode] BIT NOT NULL, 
    [CodeValue] INT NULL, 
    CONSTRAINT [AssessmentIntanceResponse_Patient_FK] FOREIGN KEY ([PatientKey]) REFERENCES [PatientModule].[Patient] ([PatientKey])
);
GO

CREATE NONCLUSTERED INDEX [AssessmentInstanceResponse_Patient_FK_IDX]
    ON [AssessmentModule].[AssessmentInstanceResponse]([PatientKey] ASC);
	GO
	CREATE NONCLUSTERED INDEX [AssessmentInstanceResponse_Organization_FK_IDX]
    ON [AssessmentModule].[AssessmentInstanceResponse]([OrganizationKey] ASC);

