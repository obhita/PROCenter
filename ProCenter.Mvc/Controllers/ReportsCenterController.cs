using System.Web.Mvc;

namespace ProCenter.Mvc.Controllers
{
    using System.Threading.Tasks;

    using Agatha.Common;

    /// <summary>
    /// The ReportsCenterController class.
    /// </summary>
    public class ReportsCenterController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportsCenterController"/> class.
        /// </summary>
        /// <param name="requestDispatcherFactory">The request dispatcher factory.</param>
        public ReportsCenterController ( IRequestDispatcherFactory requestDispatcherFactory ) : base ( requestDispatcherFactory )
        {
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Returns and ActionResult.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
