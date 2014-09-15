CREATE TABLE [AssessmentModule].[AssessmentDefinition] (
    [AssessmentDefinitionKey]           UNIQUEIDENTIFIER             NOT NULL,
	[AssessmentName]	 NVARCHAR (500)     NOT NULL,
	[AssessmentCode]	 NVARCHAR (50)     NOT NULL
    PRIMARY KEY CLUSTERED ([AssessmentDefinitionKey] ASC), 
    [ScoreType] NVARCHAR(50) NOT NULL,  
);
GO
