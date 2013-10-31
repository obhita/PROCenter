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