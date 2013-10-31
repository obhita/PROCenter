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
namespace ProCenter.Mvc.Infrastructure.Filter
{
    #region

    using System.Web;
    using System.Web.Mvc;
    using NLog;

    #endregion

    public class ExtendedHandleErrorAttribute : HandleErrorAttribute
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Called when an exception occurs.
        /// </summary>
        /// <param name="exceptionContext">The exception context.</param>
        public override void OnException(ExceptionContext exceptionContext)
        {
            if (exceptionContext.ExceptionHandled /* || !exceptionContext.HttpContext.IsCustomErrorEnabled*/)
            {
                return;
            }

            if (new HttpException(null, exceptionContext.Exception).GetHttpCode() != 500)
            {
                return;
            }

            if (!ExceptionType.IsInstanceOfType(exceptionContext.Exception))
            {
                return;
            }

            // if the request is AJAX return JSON else view. // http://stackoverflow.com/questions/4707755/asp-net-mvc-ajax-error-handling
            if (exceptionContext.HttpContext.Request.IsAjaxRequest() && exceptionContext.Exception != null)
            {
                exceptionContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        error = true,
                        message = exceptionContext.Exception.Message,
                        // todo: replace the message with more general message like http://stackoverflow.com/questions/9120002/jquery-ajax-error-handling 
                        //stackTrace = exceptionContext.Exception.StackTrace
                    }
                };
                exceptionContext.ExceptionHandled = true;
            }
            else
            {
                var controllerName = (string)exceptionContext.RouteData.Values["controller"];
                var actionName = (string)exceptionContext.RouteData.Values["action"];
                var model = new HandleErrorInfo(exceptionContext.Exception, controllerName, actionName);

                exceptionContext.Result = new ViewResult
                {
                    ViewName = View,
                    MasterName = Master,
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                    TempData = exceptionContext.Controller.TempData
                };
            }

            _logger.Fatal(exceptionContext.Exception.Message, exceptionContext.Exception);

            exceptionContext.ExceptionHandled = true;
            exceptionContext.HttpContext.Response.Clear();
            exceptionContext.HttpContext.Response.StatusCode = 500;

            exceptionContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}