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
    using ProCenter.Domain.MessageModule;
    using ProCenter.Infrastructure;
    using ProCenter.Infrastructure.Service.ReadSideService;
    using ProCenter.Service.Message.Message;
    using Quartz;
    using Dapper;

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
                          ,[SendToEmail]                            
                        FROM MessageModule.AssessmentReminder 
                        WHERE AlertSentDate IS NULL 
                            AND SendToEmail IS NOT NULL 
                            AND Status = 'Default' 
                            AND GetDate() >= DATEADD(day, -ReminderDays, Start) 
                            AND GetDate() <= Start").ToList();

                    Logger.Info("{0} reminders retrieved.", reminders.Count);
                    foreach (var assessmentReminderDto in reminders)
                    {
                        var body = string.Format(AlertTemplate, assessmentReminderDto.Title, assessmentReminderDto.Start.ToString("D"));
                        try
                        {
                            SendEmail(body, assessmentReminderDto.SendToEmail);
                            var assessmentReminder = _assessmentReminderRepository.GetByKey(assessmentReminderDto.Key);
                            assessmentReminder.ReviseAlertSentDate(DateTime.Now);
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


        private static void SendEmail(string body, string email)
        {
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
                    message.CC.Add(new MailAddress(cc));
                }

                var smtp = new SmtpClient();
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                smtp.Send(message);
                Logger.Info("Email sent successfully.");
            }
        }
    }
}