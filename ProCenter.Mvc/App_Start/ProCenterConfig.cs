namespace ProCenter.Mvc.App_Start
{
    #region Using Statements

    using System.Web.Http;
    using System.Web.Mvc;
    using Infrastructure.Boostrapper;
    using Pillar.Common.InversionOfControl;

    #endregion

    /// <summary>
    ///     Config startup class for Pro Center
    /// </summary>
    public class ProCenterConfig
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Bootstraps this instance.
        /// </summary>
        public static void Bootstrap ()
        {
            var bootstrapper = new Bootstrapper ();
            bootstrapper.Run ();

            var dependencyResolver = new CustomDependencyResolver ( IoC.CurrentContainer );
            DependencyResolver.SetResolver ( dependencyResolver );
            GlobalConfiguration.Configuration.DependencyResolver = dependencyResolver;
        }

        #endregion
    }
}