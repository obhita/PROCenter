CREATE TABLE [ReportModule].[PatientScoreRangeReport]
(
	[AssessmentInstanceKey] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [PatientKey] UNIQUEIDENTIFIER NOT NULL, 
    [AssessmentName] NVARCHAR(500) NOT NULL, 
    [AssessmentScore] NVARCHAR(1000) NOT NULL, 
    [ScoreDate] DATETIME NOT NULL, 
    [PatientBirthDate] DATETIME NOT NULL, 
    [PatientFirstName] NVARCHAR(500) NOT NULL, 
    [PatientLastName] NVARCHAR(500) NOT NULL, 
    [PatientGender] NVARCHAR(10) NOT NULL, 
    [ScoreChange] NVARCHAR(25) NOT NULL, 
    [AssessmentCode] NVARCHAR(50) NOT NULL 
)
