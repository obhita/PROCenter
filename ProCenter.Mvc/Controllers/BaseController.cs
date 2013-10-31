#region Licence Header
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