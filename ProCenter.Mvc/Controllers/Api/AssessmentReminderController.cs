#region License Header

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

#endregion

namespace ProCenter.Mvc.Controllers.Api
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Dapper;
    using Domain.MessageModule;
    using Infrastructure;
    using Models;
    using Service.Message.Message;

    #endregion

    /// <summary>The assessment reminder controller class.</summary>
    public class AssessmentReminderController : BaseApiController
    {
        #region Fields

        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IResourcesManager _resourcesManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentReminderController"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="resourcesManager">The resources manager.</param>
        public AssessmentReminderController ( IDbConnectionFactory connectionFactory, IResourcesManager resourcesManager )
        {
            _connectionFactory = connectionFactory;
            _resourcesManager = resourcesManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Dates the time to unix timestamp.</summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>A <see cref="double"/>.</returns>
        public static double DateTimeToUnixTimestamp ( DateTime dateTime )
        {
            return ( dateTime - new DateTime ( 1970, 1, 1 ).ToLocalTime () ).TotalSeconds;
        }

        /// <summary>Unixes the time stamp to date time.</summary>
        /// <param name="unixTimeStamp">The unix time stamp.</param>
        /// <returns>A <see cref="DateTime"/>.</returns>
        public static DateTime UnixTimeStampToDateTime ( double unixTimeStamp )
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime ( 1970, 1, 1, 0, 0, 0, 0 );
            dtDateTime = dtDateTime.AddSeconds ( unixTimeStamp ).ToLocalTime ();
            return dtDateTime;
        }

        /// <summary>Adds the reminders to calendar list.</summary>
        /// <param name="assessmentReminderDtoList">The assessment reminder dto list.</param>
        /// <returns>A <see cref="IEnumerable{CalendarEventModel}"/>.</returns>
        public IEnumerable<CalendarEventModel> AddRemindersToCalendarList ( IEnumerable<AssessmentReminderDto> assessmentReminderDtoList )
        {
            var calList = new List<CalendarEventModel> ();
            foreach ( var assessmentReminderDto in assessmentReminderDtoList )
            {
                var tempDate = assessmentReminderDto.Start;
                if ( assessmentReminderDto.ReminderRecurrence == AssessmentReminderRecurrence.OneTime )
                {
                    calList.Add ( GetCalenderEventModelFromAssessmentReminderDto ( assessmentReminderDto, tempDate ) );
                    continue;
                }
                while ( tempDate < assessmentReminderDto.End )
                {
                    calList.Add ( GetCalenderEventModelFromAssessmentReminderDto ( assessmentReminderDto, tempDate ) );
                    switch ( assessmentReminderDto.ReminderRecurrence )
                    {
                        case AssessmentReminderRecurrence.Daily:
                            tempDate = tempDate.AddDays ( 1 );
                            break;
                        case AssessmentReminderRecurrence.Weekly:
                            tempDate = tempDate.AddDays ( 7 );
                            break;
                        case AssessmentReminderRecurrence.Monthly:
                            tempDate = tempDate.AddMonths ( 1 );
                            break;
                    }
                }
            }
            return calList;
        }

        /// <summary>Gets the specified start.</summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="sSearch">The s search.</param>
        /// <returns>A <see cref="IEnumerable{CalendarEventModel}"/>.</returns>
        public IEnumerable<CalendarEventModel> Get ( double start, double end, string patientKey = null, string sSearch = null )
        {
            var startDate = UnixTimeStampToDateTime ( start );
            var endDate = UnixTimeStampToDateTime ( end );

            const string SearchWhereConstraint = " AND (PatientFirstName LIKE '{0}%' OR PatientLastName LIKE '{1}%')";
            var patienKeyWhereConstraint = " AND PatientKey = @patientKey";
            const string OrganizationKeyWhereConstraint = " AND OrganizationKey = @organizationKey";

            const string Query = @"                            
                SELECT AssessmentReminderKey AS 'Key', RecurrenceKey, AssessmentReminderKey, PatientFirstName, PatientLastName, AssessmentDefinitionKey, AssessmentName, 
                       AssessmentCode, Title, Start, [End], Recurrence AS 'ReminderRecurrence'
                FROM MessageModule.AssessmentReminder 
                WHERE ( Start BETWEEN @startDate AND @endDate OR [End] > @startDate ) AND Status<>'Cancelled'{0}{1}";

            if ( UserContext.Current.PatientKey != null && patientKey != UserContext.Current.PatientKey.ToString () )
            {
                return Enumerable.Empty<CalendarEventModel> ();
            }

            if ( UserContext.Current.PatientKey != null )
            {
                patienKeyWhereConstraint += " AND ForSelfAdministration=1";
            }

            var completeQuery = string.Format ( Query,
                string.IsNullOrWhiteSpace ( patientKey ) ? OrganizationKeyWhereConstraint : patienKeyWhereConstraint,
                string.IsNullOrWhiteSpace ( sSearch ) ? string.Empty : string.Format(SearchWhereConstraint, sSearch, sSearch) );

            using ( var connection = _connectionFactory.CreateConnection () )
            {
                var assessmentReminderDtos = connection.Query<AssessmentReminderDto> ( completeQuery,
                    new
                    {
                        startDate,
                        endDate,
                        search = sSearch,
                        organizationKey = UserContext.Current.OrganizationKey,
                        patientKey,
                    } ).ToList ();

                return AddRemindersToCalendarList ( assessmentReminderDtos );
            }
        }

        #endregion

        #region Methods

        private CalendarEventModel GetCalenderEventModelFromAssessmentReminderDto ( AssessmentReminderDto reminderDto,
            DateTime date )
        {
            var reminderTitleDisplayLength = 50;
            return new CalendarEventModel
            {
                Key = reminderDto.Key.ToString (),
                Title =
                    string.Format ( "{0}: {1} for {2} {3}",
                        reminderDto.Title.Length > reminderTitleDisplayLength ? reminderDto.Title.Substring(0, reminderTitleDisplayLength - 1) + "..." : reminderDto.Title,
                        _resourcesManager.GetResourceManagerByName ( reminderDto.AssessmentName ).GetString ( SharedStringNames.ResourceKeyPrefix + reminderDto.AssessmentCode ),
                        reminderDto.PatientFirstName,
                        reminderDto.PatientLastName ),
                Start = DateTimeToUnixTimestamp ( date ),
                AllDay = true,
                RecurrenceKey = reminderDto.RecurrenceKey.GetValueOrDefault().ToString()
            };
        }

        #endregion
    }
}