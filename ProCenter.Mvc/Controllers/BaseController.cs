namespace ProCenter.Mvc.Controllers
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Agatha.Common;
    using Infrastructure.Security;
    using Service.Message.Attribute;
    using Service.Message.Common.Lookups;
    using IAsyncRequestDispatcher = Infrastructure.Service.IAsyncRequestDispatcher;

    #endregion

    /// <summary>
    ///     Base class for MVC controllers.
    /// </summary>
    [OutputCache ( NoStore = true, Duration = 0, VaryByParam = "*" )]
    public abstract class BaseController : Controller
    {
        #region Fields

        private readonly IRequestDispatcherFactory _requestDispatcherFactory;

        #endregion

        #region Constructors and Destructors

        protected BaseController ( IRequestDispatcherFactory requestDispatcherFactory )
        {
            _requestDispatcherFactory = requestDispatcherFactory;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Creates the async request dispatcher.
        /// </summary>
        /// <returns></returns>
        public IAsyncRequestDispatcher CreateAsyncRequestDispatcher ()
        {
            var dispatcher = _requestDispatcherFactory.CreateRequestDispatcher ();
            return  dispatcher as IAsyncRequestDispatcher;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called after the action method is invoked.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuted ( ActionExecutedContext filterContext )
        {
            if ( TempData["ModelState"] != null && !ModelState.Equals ( TempData["ModelState"] ) )
            {
                ModelState.Merge ( (ModelStateDictionary) TempData["ModelState"] );
            }
            base.OnActionExecuted ( filterContext );
        }


        public void AddLookupRequests(IAsyncRequestDispatcher asyncRequestDispatcher, Type dtoType)
        {
            var lookupCategories =
                dtoType.GetProperties()
                       .Where(
                           p =>
                           p.PropertyType == typeof(LookupDto) || p.PropertyType == typeof(IEnumerable<LookupDto>))
                       .Select(p =>
                       {
                           var categoryAttribute =
                               (LookupCategoryAttribute)
                               p.GetCustomAttributes(typeof(LookupCategoryAttribute), false).FirstOrDefault();
                           return (string)(categoryAttribute == null ? p.Name : categoryAttribute.Category);
                       })
                       .Distinct();

            foreach (var category in lookupCategories)
            {
                asyncRequestDispatcher.Add(category, new GetLookupsByCategoryRequest { Category = category });
            }
        }

        public void AddLookupResponsesToViewData(IAsyncRequestDispatcher asyncRequestDispatcher)
        {
            foreach (GetLookupsByCategoryResponse categoryResponse in asyncRequestDispatcher.Responses.Where(r => r.GetType() == typeof(GetLookupsByCategoryResponse)))
            {
                ViewData[categoryResponse.Category + "LookupItems"] = categoryResponse.Lookups.ToList();
            }
        }

        #endregion
    }
}