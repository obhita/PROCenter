namespace ProCenter.Mvc.Controllers
{
    using System.IdentityModel.Services;
    using System.Web.Mvc;
    using Infrastructure.Security;

    public class AccountController : Controller
    {
        private readonly ILogoutService _logoutService;

        public AccountController(ILogoutService logoutService)
        {
            _logoutService = logoutService;
        }

        public ActionResult Logout()
        {
            var signoutMessage = _logoutService.Logout();

            return Redirect(signoutMessage.WriteQueryString());
        }
    }
}