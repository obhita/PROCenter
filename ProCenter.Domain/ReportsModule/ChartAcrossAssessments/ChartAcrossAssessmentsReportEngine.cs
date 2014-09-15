// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/

namespace ProCenter.Domain.ReportsModule.ChartAcrossAssessments
{
	#region Using Statements

	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Dapper;

	using NLog;

	using Common;
	using AssessmentModule;
	using CommonModule;

	#endregion

	/// <summary>
	///     The ChartAcrossAssessmentsReportEngine class.
	/// </summary>
	[ReportEngine ( ReportNames.PatientsWithSpecificResponseAcrossAssessments )]
	public class ChartAcrossAssessmentsReportEngine : IReportEngine
	{
		#region Fields

        private const string Query = @"SELECT [AssessmentModule].[AssessmentInstanceResponse].[AssessmentInstanceKey]
									,[AssessmentModule].[AssessmentInstanceResponse].[PatientKey]
									,[ItemDefinitionCode]
									,[AssessmentModule].[AssessmentInstanceResponse].[AssessmentName]
									,[AssessmentModule].[AssessmentInstanceResponse].[AssessmentCode]
									,[AssessmentModule].[AssessmentInstanceResponse].[OrganizationKey]
									,[ResponseType]
									,[ResponseValue]
									,[GenderCode]
									,[FirstName] AS PatientFirstName
									,[LastName] AS PatientLastName
									,[DateOfBirth]
									,FLOOR((CAST (GetDate() AS INTEGER) - CAST(DateOfBirth AS INTEGER)) / 365.25) AS PatientAge
									,[LastModifiedTime]
                                    ,[ResponseType]
									,[IsCode]
									FROM [AssessmentModule].[AssessmentInstanceResponse]
									INNER JOIN PatientModule.Patient ON PatientModule.Patient.PatientKey = [AssessmentModule].[AssessmentInstanceResponse].PatientKey
									INNER JOIN [AssessmentModule].[AssessmentInstance] ON 
									[AssessmentModule].[AssessmentInstance].AssessmentInstanceKey = [AssessmentModule].[AssessmentInstanceResponse].AssessmentInstanceKey
									WHERE [AssessmentModule].[AssessmentInstanceResponse].OrganizationKey = '{0}'
									{1}
									{2}
									{3}
									{4}";

		private const string QueryOrder = @" ORDER BY AssessmentName, ItemDefinitionCode, LastName, FirstName";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly IDbConnectionFactory _connectionFactory;

		private readonly IRecentReportRepository _recentReportRepository;

		private readonly IReportTemplateRepository _reportTemplateRepository;

		private readonly IResourcesManager _resourcesManager;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartAcrossAssessmentsReportEngine" /> class.
		/// </summary>
		/// <param name="resourcesManager">The resources manager.</param>
		/// <param name="reportTemplateRepository">The report template repository.</param>
		/// <param name="reportHistoryRepository">The report history repository.</param>
		/// <param name="dbConnectionFactory">The database connection factory.</param>
		public ChartAcrossAssessmentsReportEngine(
			IResourcesManager resourcesManager,
			IReportTemplateRepository reportTemplateRepository,
			IRecentReportRepository reportHistoryRepository,
			IDbConnectionFactory dbConnectionFactory )
		{
			_resourcesManager = resourcesManager;
			_reportTemplateRepository = reportTemplateRepository;
			_recentReportRepository = reportHistoryRepository;
			_connectionFactory = dbConnectionFactory;
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>Generates the specified key.</summary>
		/// <param name="key">The key.</param>
		/// <param name="reportName">Name of the report.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns>
		///     A <see cref="IReport" />.
		/// </returns>
		/// <exception cref="System.ArgumentException">Invalid parameters.</exception>
		public IReport Generate ( Guid key, string reportName, object parameters = null )
		{
			var reportParams = (ChartAcrossAssessmentsParameters)parameters;
			if ( reportParams == null )
			{
				return null;
			}
			var data = GetData ( reportParams );
			if ( data == null )
			{
				return null;
			}

			SetStrings(data, reportParams, reportName);
			var reportDataCollection = new ChartAcrossAssessmentsDataCollection
									   {
										   data,
									   };
			var report = new ChartAcrossAssessmentsReport
						 {
							 DataSource = reportDataCollection,
						 };
			return report;
		}

		/// <summary>
		///     Gets the customization model.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="reportName">Name of the report.</param>
		/// <param name="patientKey">The patient key.</param>
		/// <returns>
		///     A <see cref="IReportModel" />.
		/// </returns>
		public IReportModel GetCustomizationModel ( Guid key, string reportName, Guid? patientKey = null )
		{
			if ( key != Guid.Empty )
			{
				var reportTemplate = _reportTemplateRepository.GetByKey ( key );
				if ( reportTemplate != null )
				{
					return reportTemplate.Parameters as ChartAcrossAssessmentsParameters;
				}
				var reporthistory = _recentReportRepository.GetByKey ( key );
				if ( reporthistory != null )
				{
					return reporthistory.Parameters as ChartAcrossAssessmentsParameters;
				}
			}

			return new ChartAcrossAssessmentsParameters { ReportName = reportName };
		}

		/// <summary>Updates the customization model.</summary>
		/// <param name="key">The key.</param>
		/// <param name="reportName">Name of the report.</param>
		/// <param name="name">The name.</param>
		/// <param name="shouldShow">The should show.</param>
		/// <param name="text">The text.</param>
		public void UpdateCustomizationModel ( Guid key, string reportName, string name, bool? shouldShow, string text )
		{
		}

		#endregion

		#region Methods

		private void SetStrings(ChartAcrossAssessmentsData data, ChartAcrossAssessmentsParameters reportParams, string reportName)
		{
			if ( reportParams == null || data == null )
			{
				return;
			}

			var start = reportParams.StartDate;
			var end = reportParams.EndDate;
			var dateRange = start.GetValueOrDefault().ToShortDateString() + " - " + end.GetValueOrDefault().ToShortDateString();
			if (reportParams.TimePeriod != null)
			{
				reportParams.TimePeriod.GetRange(out start, out end);
				dateRange = reportParams.TimePeriod.DisplayName;
			}
			var ageRange = _resourcesManager.GetResourceManagerByName(reportName).GetString("NA");
			if (reportParams.AgeRangeLow != null && reportParams.AgeRangeHigh != null)
			{
				ageRange = reportParams.AgeRangeLow + " - " + reportParams.AgeRangeHigh;
			}
			var gender = _resourcesManager.GetResourceManagerByName(reportName).GetString("NA");
			if (!string.IsNullOrWhiteSpace(reportParams.Gender))
			{
				gender = reportParams.Gender;
			}
			var assessment = _resourcesManager.GetResourceManagerByName(reportName).GetString("All");
			if ( !string.IsNullOrWhiteSpace ( reportParams.AssessmentName ) )
			{
				assessment = reportParams.AssessmentName;
			}
			data.AssessmentParameter = _resourcesManager.GetResourceManagerByName(reportName).GetString("Assessment") + ": " + assessment;
			data.DateRangeParameter = _resourcesManager.GetResourceManagerByName(reportName).GetString("DateRange") + ": " + dateRange;
			data.AgeGroupParameter = _resourcesManager.GetResourceManagerByName(reportName).GetString("AgeGroup") + ": " + ageRange;
			data.GenderParameter = _resourcesManager.GetResourceManagerByName(reportName).GetString("Gender") + ": " + gender;
			data.TotalQuestionsForQuery = _resourcesManager.GetResourceManagerByName(reportName).GetString("TotalQuestionsForQuery") + 
				": " + data.Data.GroupBy(a => a.ItemDefinitionCode).Select(group => group.First()).Count();
			data.TotalNumberOfSpecifiedResults = _resourcesManager.GetResourceManagerByName(reportName).GetString("TotalNumberOfSpecificResults") + 
				": " + data.Data.Count();
			data.TotalNumberOfAssessmentsDuringTimeFrame = _resourcesManager.GetResourceManagerByName(reportName).GetString("TotalNumberOfAssessmentsDuringTimeFrame") +
				": " + data.Data.GroupBy(a => a.PatientKey).Select(group => group.First()).Count();
			data.LocalizedQuestionResponses = GetQuestionResponsesFromResource(reportParams);

			data.ReportName = _resourcesManager.GetResourceManagerByName(reportName).GetString("ReportName");

			data.HeaderQuestion = _resourcesManager.GetResourceManagerByName ( reportName ).GetString ( "HeaderQuestion" );
			data.HeaderSpecificResponseValue = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderSpecificResponse");
			data.HeaderName = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderName");
			data.HeaderAge = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderAge");
			data.HeaderGender = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderGender");
			data.HeaderAssessmentName = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderAssessmentName");
			data.HeaderAssessmentDate = _resourcesManager.GetResourceManagerByName(reportName).GetString("AssessmentDate");
			data.HeaderGivenResponse = _resourcesManager.GetResourceManagerByName(reportName).GetString("GivenResponse");
			data.HeaderViewAssessment = _resourcesManager.GetResourceManagerByName(reportName).GetString("HeaderViewAssessment");
		}

		private List<QuestionResponse> GetQuestionResponsesFromResource(ChartAcrossAssessmentsParameters reportParams)
		{
			var returnResponses = reportParams.QuestionResponses;
			foreach (var response in returnResponses)
			{
				response.LocalizedResponses = new List<string> ();
				QuestionResponse response1 = response;
				foreach ( var r in response.Responses.Where ( r => response1.InputType == "MultipleSelect" ).ToList () )
				{
					var assessmentName = string.Empty;
					var assessment = GetAssessmentDefinitionForResponse(response.AssessmentDefinitionKey.ToString());
					if ( assessment != null )
					{
						assessmentName = assessment.AssessmentName;
					}
					response.LocalizedResponses.Add(_resourcesManager.GetResourceManagerByName(assessmentName).GetString("_" + r));
				}
				foreach (var r in response.Responses.Where(r => response1.InputType != "MultipleSelect").ToList())
				{
				    response.LocalizedResponses.Add(response1.InputType == "Height" ? GetHeightString(r) : r);
				}
			}
			return returnResponses;
		}

		private AssessmentDefinitionDto GetAssessmentDefinitionForResponse(string assessmentDefinitionKey)
		{
			const string queryActive = @"SELECT [AssessmentDefinitionKey]
									   ,[AssessmentName]
									   ,[AssessmentCode]
									   ,[ScoreType]
									   FROM [AssessmentModule].[AssessmentDefinition]
									   WHERE AssessmentDefinitionKey='{0}'";

			var completeQuery = string.Format(queryActive, assessmentDefinitionKey);
			using (var connection = _connectionFactory.CreateConnection())
			using (var multiQuery = connection.QueryMultiple(completeQuery))
			{
				return multiQuery.Read<AssessmentDefinitionDto>().FirstOrDefault();
			}
		}
		
		private string GetDateRange ( ChartAcrossAssessmentsParameters parameters )
		{
			const string dateRangeWhereString = " AND (LastModifiedTime >= '{0}' AND LastModifiedTime <= DATEADD(day,1, '{1}'))";
			var startDate = parameters.StartDate;
			var endDate = parameters.EndDate;
			if (parameters.TimePeriod != null)
			{
				parameters.TimePeriod.GetRange(out startDate, out endDate);
			}
			return string.Format(dateRangeWhereString, startDate.GetValueOrDefault().ToShortDateString(), endDate.GetValueOrDefault().ToShortDateString());
		}

		private string GetAgeRange ( ChartAcrossAssessmentsParameters parameters )
		{
			const string ageWhereString = @" FLOOR((CAST (GetDate() AS INTEGER) - CAST(DateOfBirth AS INTEGER)) / 365.25) >= {0} 
										 AND FLOOR((CAST (GetDate() AS INTEGER) - CAST(DateOfBirth AS INTEGER)) / 365.25) <= {1}";
			var ageWhere = string.Empty;
			if (parameters.AgeRangeLow != null && parameters.AgeRangeHigh != null)
			{
				ageWhere = " AND " + string.Format(ageWhereString, parameters.AgeRangeLow, parameters.AgeRangeHigh);
			}
			return ageWhere;
		}

		private string GetGender ( ChartAcrossAssessmentsParameters parameters )
		{
			const string genderWhereString = " PatientModule.Patient.GenderCode = '{0}'";
			var genderWhere = string.Empty;
			if (!string.IsNullOrWhiteSpace(parameters.Gender))
			{
				genderWhere = " AND " + string.Format(genderWhereString, parameters.Gender);
			}
			return genderWhere;
		}

		private string GetResponseValues ( ChartAcrossAssessmentsParameters parameters )
		{
			var responseValues = string.Empty;
			var or = string.Empty;
			foreach (QuestionResponse qr in parameters.QuestionResponses)
			{
				switch (qr.InputType)
				{
					case "IntRange":
						break;
					case "MultipleSelect":
						responseValues += GetMultiselectQuery(qr, or);
						break;
					default:
						responseValues += GetStringQuery(qr, or);
						break;
				}
				or = " OR ";
			}
			if ( responseValues.Length > 0 )
			{
				responseValues = " AND (" + responseValues + ")";
			}
			return responseValues;
		}

		private string GetResponseValuesInt(ChartAcrossAssessmentsParameters parameters)
		{
			var responseValues = string.Empty;
			foreach (QuestionResponse qr in parameters.QuestionResponses)
			{
				switch (qr.InputType)
				{
					case "IntRange":
						responseValues += GetIntRangeQuery(qr, string.Empty);
						break;
				}
			}
			if (responseValues.Length > 0)
			{
				responseValues = " AND (" + responseValues + ")";
			}
			return responseValues;
		}

		private string GetQuery(ChartAcrossAssessmentsParameters parameters)
		{
			var dateRangeWhere = GetDateRange ( parameters );
			var ageWhere = GetAgeRange(parameters);
			var genderWhere = GetGender(parameters);
			var responseValues = GetResponseValues ( parameters );
			var responseValuesInt = GetResponseValuesInt ( parameters );
		    if ( responseValues.Length == 0 )
		    {
		        responseValues = responseValuesInt;
		        responseValuesInt = string.Empty;
		    }
		    var union = string.Empty;
            if (responseValuesInt.Length > 0)
		    {
                union = " UNION " + string.Format(Query, parameters.OrganizationKey, ageWhere, responseValuesInt, genderWhere, dateRangeWhere);
		    }
			var finalQuery = string.Format(Query, parameters.OrganizationKey, ageWhere, genderWhere, responseValues, dateRangeWhere);
			if ( responseValuesInt.Length > 0 )
			{
				finalQuery += union;
			}
		    var query = finalQuery + QueryOrder;
            Logger.Info("ChartAcrossAssessments Query: {0}", query);
            return query;
		}

        private ChartAcrossAssessmentsData GetData(ChartAcrossAssessmentsParameters parameters)
		{
			var returnData = new ChartAcrossAssessmentsData();
			using (var connection = _connectionFactory.CreateConnection())
			{
				var reportDtos = connection.Query<ChartAcrossAssessmentsDto>(GetQuery(parameters)).ToList();
				if (!reportDtos.Any())
				{
					return null;
				}
				var data = reportDtos.Select(
					reportDto => new ChartAcrossAssessmentsDataObject
					{
                        ResponseType = reportDto.ResponseType,
						IsCode = reportDto.IsCode,
						Age = reportDto.PatientAge,
						Gender = reportDto.GenderCode.Substring ( 0, 1 ),
						PatientName = reportDto.PatientFirstName + " " + reportDto.PatientLastName,
						AssessmentDate = reportDto.LastModifiedTime.ToShortDateString(),
						PatientKey = reportDto.PatientKey,
						AssessmentInstanceKey = reportDto.AssessmentInstanceKey,
						AssessmentName = _resourcesManager.GetResourceManagerByName(reportDto.AssessmentName).GetString("_" + reportDto.AssessmentCode),
						ItemDefinitionCode = reportDto.ItemDefinitionCode,
						Question = _resourcesManager.GetResourceManagerByName(reportDto.AssessmentName).GetString("_" + reportDto.ItemDefinitionCode),
						Response = GetResponseValue ( reportDto)
					}).ToList();
				returnData.Data = data;
			}
			return returnData;
		}

        private string GetHeightString(string response)
        {
            int height;
            int.TryParse(response, out height);
            var feet = height / 12;
            var inches = height - (feet * 12);
            var feetUnit = _resourcesManager.GetResourceManagerByName("ChartAcrossAssessments").GetString("HeightUnit1");
            var inchUnit = _resourcesManager.GetResourceManagerByName("ChartAcrossAssessments").GetString("HeightUnit2");
            return string.Format("{0} {1} {2} {3}", feet, feetUnit, inches, inchUnit);
        }

		private string GetResponseValue (ChartAcrossAssessmentsDto reportDto)
		{
		    if ( reportDto.ResponseType == "Height" )
		    {
		        return GetHeightString ( reportDto.ResponseValue );
		    }
			return reportDto.IsCode ? 
				_resourcesManager.GetResourceManagerByName ( reportDto.AssessmentName ).GetString ( "_" + reportDto.ResponseValue ) : 
				reportDto.ResponseValue;
		}

		private string GetResponseValueAtIndex ( QuestionResponse questionResponse, int index )
		{
			var returnString = string.Empty;
			if (questionResponse.Responses.Count >= index + 1)
			{
				returnString = questionResponse.Responses.ElementAt ( index );
			}
			return returnString;
		}

		private string GetIntRangeQuery ( QuestionResponse questionResponse, string or)
		{
			var returnStr = string.Format(" {0} ([AssessmentModule].[AssessmentInstanceResponse].[AssessmentCode]='{1}' " +
										   " AND ItemDefinitionCode='{2}' " +
										   " AND ResponseType='Int32' " +
										   " AND CAST(ResponseValue AS INT) " +
										   "BETWEEN {3} AND {4})",
										   or,
										   questionResponse.AssessmentCode,
										   questionResponse.ItemDefinitionCode,
										   GetResponseValueAtIndex ( questionResponse, 0 ),
										   GetResponseValueAtIndex ( questionResponse, 1 ) );
			return returnStr;
		}

		private string GetStringQuery ( QuestionResponse questionResponse, string or )
		{
			var returnStr = string.Format(" {0} ([AssessmentModule].[AssessmentInstanceResponse].[AssessmentCode]='{1}' " +
										  "AND ItemDefinitionCode='{2}' " +
										  "AND ResponseValue='{3}')", 
										  or,
										  questionResponse.AssessmentCode, 
										  questionResponse.ItemDefinitionCode,
										  GetResponseValueAtIndex(questionResponse, 0));
			return returnStr;
		}

		private string GetMultiselectQuery ( QuestionResponse questionResponse, string or )
		{
			var returnStr = string.Format(" {0} ([AssessmentModule].[AssessmentInstanceResponse].[AssessmentCode]='{1}' " +
											"AND ItemDefinitionCode='{2}' " +
											"AND ResponseValue IN ({3}))",
											or,
											questionResponse.AssessmentCode, 
											questionResponse.ItemDefinitionCode, 
                                            string.Join(",", questionResponse.Responses.Select ( s =>  "'" + s + "'" )));
			return returnStr;
		}

		#endregion
	}
}