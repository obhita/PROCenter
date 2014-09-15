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

namespace ProCenter.Mvc.Infrastructure.Security
{
    #region Using Statements

    using System.Web;
    using System.Web.Mvc;
    using Pillar.Security.AccessControl;

    #endregion

    /// <summary>The access control security filter attribute class.</summary>
    public class AccessControlSecurityFilterAttribute : AuthorizeAttribute
    {
        #region Constants

        private const string Label = "PROCenter.Mvc.ClaimsAuthorizeAttribute";

        #endregion

        #region Fields

        private readonly IAccessControlManager _accessControlManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessControlSecurityFilterAttribute"/> class.
        /// </summary>
        /// <param name="accessControlManager">The access control manager.</param>
        public AccessControlSecurityFilterAttribute ( IAccessControlManager accessControlManager )
        {
            _accessControlManager = accessControlManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Called when a process requests authorization.</summary>
        /// <param name="filterContext">The filter context, which encapsulates information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />.</param>
        public override void OnAuthorization ( AuthorizationContext filterContext )
        {
            filterContext.HttpContext.Items[Label] = filterContext;
            base.OnAuthorization ( filterContext );
        }

        #endregion

        #region Methods

        /// <summary>When overridden, provides an entry point for custom authorization checks.</summary>
        /// <param name="httpContext">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <returns>true if the user is authorized; otherwise, false.</returns>
        protected override bool AuthorizeCore ( HttpContextBase httpContext )
        {
            return CheckAccess ( httpContext.Items[Label] as AuthorizationContext );
        }

        /// <summary>Checks the access.</summary>
        /// <param name="filterContext">The filter context.</param>
        /// <returns>A <see cref="bool"/>.</returns>
        protected virtual bool CheckAccess ( AuthorizationContext filterContext )
        {
            var resourceRequest = new ResourceRequest
            {
                filterContext.Controller.GetType ().FullName,
                filterContext.ActionDescriptor.ActionName,
                filterContext.HttpContext.Request.HttpMethod
            };
            return _accessControlManager.CanAccess ( resourceRequest );
        }

        #endregion
    }
}