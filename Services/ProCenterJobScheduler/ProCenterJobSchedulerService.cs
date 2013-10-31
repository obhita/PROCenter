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
namespace ProCenterJobScheduler
{
    #region

    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.ServiceProcess;
    using System.Threading;
    using AssessmentReminder;
    using NLog;
    using Quartz;
    using Quartz.Impl;

    #endregion

    public partial class ProCenterJobSchedulerService : ServiceBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private IScheduler _scheduler;

        public ProCenterJobSchedulerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //http://stackoverflow.com/questions/11140597/iis-app-pool-recycle-quartz-scheduling
            //http://stackoverflow.com/questions/2870562/pros-and-cons-of-running-quartz-net-embedded-or-as-a-windows-service
            Logger.Info("*** Service Starting ***");
            _scheduler = StartJobScheduler();
        }

        protected override void OnContinue()
        {
            Logger.Info("*** Service Resuming ***");
            _scheduler.ResumeAll();
        }

        protected override void OnPause()
        {
            Logger.Info("*** Service Pausing ***");
            _scheduler.PauseAll();
        }

        protected override void OnStop()
        {
            Logger.Info("*** Service Stopping ***");
            StopJobScheduler(_scheduler);
        }

        internal IScheduler StartJobScheduler()
        {
            Logger.Info("------- Initializing ----------------------");
            var stdSchedulerFactory = new StdSchedulerFactory();
            var scheduler = stdSchedulerFactory.GetScheduler();
            Logger.Info("------- Initialization Complete -----------");

            var runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow);

            Logger.Info("------- Scheduling Email Reminder Job  -------------------");
            var job = JobBuilder.Create<EmailReminderJob>().WithIdentity("emailReminderJob", "group1").Build();

            ITrigger trigger;
            const string triggerName = "dailyTrigger";
            const string triggerGroup = "group1";

#if DEBUG
            trigger = TriggerBuilder.Create() .WithIdentity(triggerName, triggerGroup).StartAt(runTime).WithSimpleSchedule(s=>s.WithIntervalInMinutes(1).RepeatForever()).Build();
#else
            var triggerSimpleSchedulerIntervalInSeconds = ConfigurationManager.AppSettings["TriggerSimpleSchedulerIntervalInSeconds"];
            if (string.IsNullOrWhiteSpace(triggerSimpleSchedulerIntervalInSeconds))
            {
                var triggerCronSchedulerExpression = ConfigurationManager.AppSettings["TriggerCronSchedulerExpression"];
                trigger = TriggerBuilder.Create().WithIdentity(triggerName, triggerGroup).WithCronSchedule(triggerCronSchedulerExpression).Build();
            }
            else
            {
                int repeatInterval;
                if (!int.TryParse(triggerSimpleSchedulerIntervalInSeconds, out repeatInterval))
                {
                    repeatInterval = 60 * 5; //default to 5 minutes
                }
                trigger =
                    TriggerBuilder.Create()
                                  .WithIdentity(triggerName, triggerGroup)
                                  .StartAt(runTime)
                                  .WithSimpleSchedule(s => s.WithIntervalInSeconds(repeatInterval).RepeatForever())
                                  .Build();
            }
#endif
            scheduler.ScheduleJob(job, trigger);
            Logger.Info("{0} will run at: {1}", job.Key, runTime.ToString("r"));

            scheduler.Start();
            Logger.Info("------- Started Scheduler -----------------");

            return scheduler;
        }

        internal void StopJobScheduler(IScheduler scheduler)
        {
            Logger.Info("------- Shutting Down ---------------------");
            scheduler.Shutdown(true);
            Logger.Info("------- Shutdown Complete -----------------");
        }

    }
}