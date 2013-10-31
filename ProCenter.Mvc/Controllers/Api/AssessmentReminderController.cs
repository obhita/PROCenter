namespace ProCenter.Mvc.Controllers.Api
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Dapper;
    using Infrastructure;
    using Models;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using Service.Message.Message;

    #endregion

    public class AssessmentReminderController : BaseApiController
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IResourcesManager _resourcesManager;

        public AssessmentReminderController(IDbConnectionFactory connectionFactory, IResourcesManager resourcesManager)
        {
            _connectionFactory = connectionFactory;
            _resourcesManager = resourcesManager;
        }

        public IEnumerable<CalendarEventModel> Get(double start, double end, string patientKey = null, string sSearch = null)
        {
            var startDate = UnixTimeStampToDateTime(start);
            var endDate = UnixTimeStampToDateTime(end);

            const string searchWhereConstraint = " AND (PatientFirstName LIKE @search+'%' OR PatientLastName LIKE @search+'%')";
            var patienKeyWhereConstraint = " AND PatientKey = @patientKey";
            const string organizationKeyWhereConstraint = " AND OrganizationKey = @organizationKey";

            const string query = @"                            
                                 SELECT AssessmentReminderKey AS 'Key', PatientFirstName, PatientLastName, AssessmentDefinitionKey, AssessmentName, AssessmentCode, Title, Start                
                                 FROM MessageModule.AssessmentReminder 
                                 WHERE Start BETWEEN @startDate AND @endDate AND Status<>'Cancelled'{0}{1}";

            if ( UserContext.Current.PatientKey != null && patientKey != UserContext.Current.PatientKey.ToString () )
            {
                return Enumerable.Empty<CalendarEventModel> ();
            }

            if ( UserContext.Current.PatientKey != null )
            {
                patienKeyWhereConstraint += " AND ForSelfAdministration=1";
            }

            var completeQuery = string.Format(query,
                                              string.IsNullOrWhiteSpace(patientKey) ? organizationKeyWhereConstraint : patienKeyWhereConstraint,
                                              string.IsNullOrWhiteSpace(sSearch) ? "" : searchWhereConstraint);

            using (var connection = _connectionFactory.CreateConnection())
            {
                var assessmentReminderDtos = connection.Query<AssessmentReminderDto>(completeQuery, new
                    {
                        startDate,
                        endDate,
                        search = sSearch,
                        organizationKey = UserContext.Current.OrganizationKey,
                        patientKey
                    }).ToList();

                return assessmentReminderDtos.Select(m => new CalendarEventModel
                    {
                        Key = m.Key.ToString(),
                        Title =
                            string.Format("{0}: {1} for {2} {3}", m.Title,
                                          _resourcesManager.GetResourceManagerByName(m.AssessmentName).GetString(SharedStringNames.ResourceKeyPrefix + m.AssessmentCode),
                                          m.PatientFirstName, m.PatientLastName),
                        Start = DateTimeToUnixTimestamp(m.Start),
                        AllDay = true
                    }).ToList();
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }
    }
}