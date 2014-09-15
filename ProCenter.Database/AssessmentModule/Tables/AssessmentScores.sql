CREATE TABLE [AssessmentModule].[AssessmentScores] (
    [AssessmentScoresKey]           UNIQUEIDENTIFIER             NOT NULL,
	[AssessmentDefinitionCode]	 NVARCHAR (50)     NOT NULL,
	[AssessmentScore]	 NVARCHAR (1000)     NOT NULL,
	[ScoredDate] DATETIME2 NOT NULL,
	[PatientKey] UNIQUEIDENTIFIER NOT NULL
    PRIMARY KEY CLUSTERED ([AssessmentScoresKey] ASC),  
);
GO