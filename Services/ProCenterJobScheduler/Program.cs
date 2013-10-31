namespace ProCenterJobScheduler
{
    #region

    using System;
    using System.IO;
    using System.ServiceProcess;
    using System.Threading;
    using NLog;
    using ProCenter.Mvc.Infrastructure.Boostrapper;

    #endregion

    internal static class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            
            var bootstrapper = new Bootstrapper();
            try
            {
                bootstrapper.Run();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

#if(!DEBUG)
            var servicesToRun = new ServiceBase[]
                {
                    new ProCenterJobSchedulerService()
                };
            ServiceBase.Run(servicesToRun);
#else
            //http://www.codeproject.com/Articles/10153/Debugging-Windows-Services-under-Visual-Studio-NET
            new ProCenterJobSchedulerService().StartJobScheduler();
            Thread.Sleep(Timeout.Infinite);
#endif
        }
    }
}