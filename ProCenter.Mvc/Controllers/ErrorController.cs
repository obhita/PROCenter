namespace ProCenter.Mvc.Controllers
{
    #region

    using System.Web.Mvc;

    #endregion

    public class ErrorController : Controller
    {
        public ActionResult HttpError()
        {
            return View();
        }

        public ActionResult Http404()
        {
            return View();
        }
    }
}