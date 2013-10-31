#region Using Statements

#endregion

namespace ProCenter.Mvc.Controllers
{
    #region Using Statements

    using System;
    using System.Web.Mvc;
    using Agatha.Common;
    using Common;
    using Models;
    using Pillar.Security.AccessControl;

    #endregion

    public class HomeController : BaseController
    {
        private readonly IAccessControlManager _accessControlManager;

        #region Constructors and Destructors

        public HomeController(IRequestDispatcherFactory requestDispatcherFactory, IAccessControlManager accessControlManager)
            : base(requestDispatcherFactory)
        {
            _accessControlManager = accessControlManager;
        }

        #endregion

        #region Public Methods and Operators

        public ActionResult Index()
        {
            if ( !UserContext.Current.OrganizationKey.HasValue )
            {
                return RedirectToAction ( "Index", "SystemAdmin" );
            }
            if ( UserContext.Current.PatientKey.HasValue )
            {
                return RedirectToAction ( "Index", "Portal" );
            }
            return View();
        }

        #endregion
    }
}