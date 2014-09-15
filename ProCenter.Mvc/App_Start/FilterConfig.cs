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

namespace ProCenter.Mvc.App_Start
{
    #region Using Statements

    using System.Web.Http;
    using System.Web.Mvc;
    using Infrastructure.Filter;
    using Infrastructure.Security;
    using NLog;
    using Pillar.Common.InversionOfControl;
    using AuthorizeAttribute = System.Web.Mvc.AuthorizeAttribute;

    #endregion

    /// <summary>The filter configuration class.</summary>
    public class FilterConfig
    {
        #region Public Methods and Operators

        /// <summary>
        /// Registers the global filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public static void RegisterGlobalFilters ( GlobalFilterCollection filters )
        {
            filters.Add ( new ExtendedHandleErrorAttribute () );
            filters.Add ( new AuthorizeAttribute () );
            filters.Add ( new RequireHttpsAttribute () );
            filters.Add ( IoC.CurrentContainer.Resolve<AccessControlSecurityFilterAttribute> () );
            if ( LogManager.GetCurrentClassLogger ().IsDebugEnabled )
            {
                filters.Add ( new LogAccessFilterAttribute () );
            }
        }

        /// <summary>
        /// Registers the web API global filters.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void RegisterWebApiGlobalFilters ( HttpConfiguration config )
        {
            config.Filters.Add ( new ExtendedExceptionFilterAttribute () );
            if ( LogManager.GetCurrentClassLogger ().IsDebugEnabled )
            {
                config.Filters.Add ( new LogAccessFilterAttribute () );
            }
        }

        #endregion
    }
}