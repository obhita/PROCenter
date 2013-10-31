CREATE TABLE [OrganizationModule].[OrganizationAssessmentDefinition]
(
	[OrganizationKey] UNIQUEIDENTIFIER NOT NULL,
	[AssessmentDefinitionKey] UNIQUEIDENTIFIER NOT NULL
	PRIMARY KEY CLUSTERED ([OrganizationKey], [AssessmentDefinitionKey]  ASC), 
    [AssessmentName] NVARCHAR(500) NOT NULL, 
    [AssessmentCode] NVARCHAR(50) NOT NULL,
);
GO
CREATE NONCLUSTERED INDEX [OrganizationAssessmentDefinition_Organization_IDX]
	ON [OrganizationModule].[OrganizationAssessmentDefinition]([OrganizationKEY] ASC);
GO
CREATE NONCLUSTERED INDEX [OrganizationAssessmentDefinition_Assessment_IDX]
	ON [OrganizationModule].[OrganizationAssessmentDefinition]([AssessmentDefinitionKEY] ASC);
GO