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
namespace ProCenterJobScheduler.AssessmentReminder
{
    #region

    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using NLog;
    using Pillar.Common.InversionOfControl;
    using ProCenter.Common;
    using ProCenter.Domain.MessageModule;
    using ProCenter.Infrastructure;
    using ProCenter.Service.Message.Message;
    using Quartz;
    using Dapper;

    using Raven.Client.Linq;

    #endregion

    public class EmailReminderJob : IJob
    {
        private readonly IAssessmentReminderRepository _assessmentReminderRepository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string AlertTemplate = 
            @"Please check your PRO Center for upcoming assessment {0} on {1}.  To login, please click the link below:<br /><br />
            https://procenter-qa.obhita.org/ <br /><br /><br />
            Please DO NOT reply to this email, it is an automated mail system and not monitored.";


        public EmailReminderJob()
        {
            _assessmentReminderRepository = IoC.CurrentContainer.Resolve<IAssessmentReminderRepository>();
        }

        public void Execute(IJobExecutionContext context)
        {
            Logger.Info("Job executed at {0} ***", DateTime.Now);

            try
            {
                using (var connection = IoC.CurrentContainer.Resolve<IDbConnectionFactory>().CreateConnection())
                {
                    var reminders = connection.Query<AssessmentReminderDto>(
                        @"SELECT [AssessmentReminderKey]  AS 'Key' 
                          ,[PatientKey]
                          ,[PatientFirstName]
                          ,[PatientLastName]
                          ,[CreatedByStaffKey]
                          ,[AssessmentName]
                          ,[Title]
                          ,[Start]
                          ,[End]
                          ,[Recurrence] AS 'ReminderRecurrence'
                          ,[ReminderDays] AS 'ReminderTime'
                          ,[SendToEmail]
                          ,[AlertSentDate]                             
                        FROM MessageModule.AssessmentReminder 
                        WHERE (AlertSentDate IS NULL OR AlertSentDate <= [End])
                            AND SendToEmail IS NOT NULL 
                            AND Status = 'Default' 
                            AND GetDate() >= DATEADD(day, -ReminderDays, Start) 
                            AND GetDate() <= [End]").ToList();

                    Logger.Info("{0} reminders retrieved.", reminders.Count);
                    foreach (var assessmentReminderDto in reminders)
                    {
                        try
                        {
                            DateTime reminderDate;
                            if (ShouldSendEmailReminderForDto(assessmentReminderDto, out reminderDate))
                            {
                                var body = string.Format(AlertTemplate, assessmentReminderDto.Title,
                                                         reminderDate.ToString("D"));
                                SendEmail(body, assessmentReminderDto.SendToEmail);
                                var assessmentReminder =
                                    _assessmentReminderRepository.GetByKey(assessmentReminderDto.Key);
                                assessmentReminder.ReviseAlertSentDate(DateTime.Now);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("Sending email alert failed.", ex.Message);
                        }
                    }
                    var unitOfWorkProvider = IoC.CurrentContainer.Resolve<IUnitOfWorkProvider>();
                    unitOfWorkProvider.GetCurrentUnitOfWork().Commit();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex.Message, ex);
                if (ex.InnerException != null)
                {
                    Logger.ErrorException("innerException", ex.InnerException);
                }
            }
        }


        private bool ShouldSendEmailReminderForDto(AssessmentReminderDto assessmentReminderDto, out DateTime reminderDate)
        {
            reminderDate = assessmentReminderDto.Start;
            var thresholdDate = GetThresholdDateForAssessmentReminder(reminderDate, assessmentReminderDto.ReminderUnit, assessmentReminderDto.ReminderTime);
            Logger.Info("reminderDate is {0}.", reminderDate);
            Logger.Info("thresholdDate is {0}.", thresholdDate);
            bool sendReminder = false;

            Logger.Info("ReminderRecurrence is {0}.", assessmentReminderDto.ReminderRecurrence);
            if (assessmentReminderDto.ReminderRecurrence == AssessmentReminderRecurrence.OneTime)
            {
                Logger.Info("ReminderRecurrence for OneTime is returning {0}.", DateTime.Now > thresholdDate && DateTime.Now > assessmentReminderDto.AlertSentDate);
                return DateTime.Now > thresholdDate && DateTime.Now > assessmentReminderDto.AlertSentDate;
            }
            while (reminderDate < assessmentReminderDto.End && !sendReminder)
            {
                thresholdDate = GetThresholdDateForAssessmentReminder(reminderDate, assessmentReminderDto.ReminderUnit, assessmentReminderDto.ReminderTime);
                sendReminder = DateTime.Now > thresholdDate &&
                                       (thresholdDate > assessmentReminderDto.AlertSentDate ||
                                        assessmentReminderDto.AlertSentDate == null);

                reminderDate = AddDaysToReminderForRecurrence(reminderDate, assessmentReminderDto.ReminderRecurrence,
                                                              sendReminder);
            }
            Logger.Info("ShouldSendEmailReminderForDto is returning {0}.", sendReminder);
            return sendReminder;
        }

        private static DateTime GetThresholdDateForAssessmentReminder(DateTime reminderDate,
                                                               AssessmentReminderUnit reminderUnit,
                                                               double reminderTime)
        {
            return reminderUnit == AssessmentReminderUnit.Days ? reminderDate.AddDays(-Convert.ToInt32(reminderTime)) :
                                                                 reminderDate.AddDays(-Convert.ToInt32(reminderTime * 7));
        }

        private static DateTime AddDaysToReminderForRecurrence(DateTime reminderDate, AssessmentReminderRecurrence recurrence, bool sendReminder)
        {
            if (!sendReminder)
            {
                switch (recurrence)
                    {
                        case AssessmentReminderRecurrence.Daily:
                            reminderDate = reminderDate.AddDays(1);
                            break;
                        case AssessmentReminderRecurrence.Weekly:
                            reminderDate = reminderDate.AddDays(7);
                            break;
                        case AssessmentReminderRecurrence.Monthly:
                            reminderDate = reminderDate.AddMonths(1);
                            break;
                    }
            }
            return reminderDate;
        }

        private static void SendEmail(string body, string email)
        {
            try
            {
                Logger.Info ( "SendEmail body: {0} To: {1}", body, email );
                using (var message = new MailMessage
                {
                    Subject = ConfigurationManager.AppSettings["EmailReminderSubject"],
                    Body = body,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true,
                })
                {
                    message.To.Add(new MailAddress(email));
                    var cc = ConfigurationManager.AppSettings["EmailReminderCC"];
                    if (!string.IsNullOrWhiteSpace(cc))
                    {
                        Logger.Info("SendEmail cc: {0}", cc);
                        message.CC.Add(new MailAddress(cc));
                    }
                    var smtp = new SmtpClient();
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    smtp.Send(message);
                    Logger.Info("Email sent successfully.");
                }
            }
            catch ( Exception e)
            {
                Logger.Error ( "SendEmail error: " +  e.Message);
                throw;
            }
        }
    }
}