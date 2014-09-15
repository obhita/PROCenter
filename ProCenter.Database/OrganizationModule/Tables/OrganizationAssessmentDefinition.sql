CREATE TABLE [OrganizationModule].[OrganizationAssessmentDefinition]
(
	[OrganizationKey] UNIQUEIDENTIFIER NOT NULL,
	[AssessmentDefinitionKey] UNIQUEIDENTIFIER NOT NULL
	PRIMARY KEY CLUSTERED ([OrganizationKey], [AssessmentDefinitionKey]  ASC), 
    [AssessmentName] NVARCHAR(500) NOT NULL, 
    [AssessmentCode] NVARCHAR(50) NOT NULL, 
    [ScoreType] NVARCHAR(50) NOT NULL,
);
GO
CREATE NONCLUSTERED INDEX [OrganizationAssessmentDefinition_Organization_IDX]
	ON [OrganizationModule].[OrganizationAssessmentDefinition]([OrganizationKey] ASC);
GO
CREATE NONCLUSTERED INDEX [OrganizationAssessmentDefinition_Assessment_IDX]
	ON [OrganizationModule].[OrganizationAssessmentDefinition]([AssessmentDefinitionKey] ASC);
GO