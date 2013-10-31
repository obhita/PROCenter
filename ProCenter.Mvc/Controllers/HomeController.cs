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