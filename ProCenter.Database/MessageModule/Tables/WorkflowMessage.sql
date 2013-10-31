CREATE TABLE [MessageModule].[WorkflowMessage]
(
	[WorkflowMessageKey] UNIQUEIDENTIFIER NOT NULL,
	[MessageType] NVARCHAR(100) NOT NULL,
	[PatientKey] UNIQUEIDENTIFIER NOT NULL,
	[WorkflowMessageStatus] NVARCHAR(100) NULL,
	[InitiatingAssessmentDefinitionCode] NVARCHAR(100) NOT NULL,
	[InitiatingAssessmentDefinitionKey] UNIQUEIDENTIFIER NOT NULL,
	[InitiatingAssessmentDefinitionName] NVARCHAR(100) NULL,
	[RecommendedAssessmentDefinitionCode] NVARCHAR(100) NOT NULL,
	[RecommendedAssessmentDefinitionKey] UNIQUEIDENTIFIER NOT NULL,
	[RecommendedAssessmentDefinitionName] NVARCHAR(100) NOT NULL,
	[InitiatingAssessmentScore] NVARCHAR(100) NULL,
	[CreatedDate] DATE NOT NULL,
	[ForSelfAdministration] bit NULL,
	[OrganizationKey] UNIQUEIDENTIFIER NOT NULL, 
    PRIMARY KEY CLUSTERED([WorkflowMessageKey] ASC),
);
GO

CREATE NONCLUSTERED INDEX [WorkflowMessage_Patient_FK_IDX]
ON [MessageModule].[WorkflowMessage]([PatientKey] ASC);
GO

CREATE NONCLUSTERED INDEX [WorkflowMessage_InitiatingAssessmentDefinitionKey_FK_IDX]
ON [MessageModule].[WorkflowMessage]([InitiatingAssessmentDefinitionKey] ASC);
GO

CREATE NONCLUSTERED INDEX [WorkflowMessage_RecommendedAssessmentDefinitionKey_FK_IDX]
ON [MessageModule].[WorkflowMessage]([RecommendedAssessmentDefinitionKey] ASC);
