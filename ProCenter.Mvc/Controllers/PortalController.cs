namespace ProCenter.Mvc.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Threading.Tasks;
    using Agatha.Common;
    using Common;
    using Infrastructure.Security;
    using Models;
    using Service.Message.Patient;
    using Service.Message.Security;

    public class PortalController : BaseController
    {
        private readonly ILogoutService _logoutService;
        //
        // GET: /Portal/

        public PortalController ( IRequestDispatcherFactory requestDispatcherFactory, ILogoutService logoutService )
            : base ( requestDispatcherFactory )
        {
            _logoutService = logoutService;
        }

        public async Task<ActionResult> Index()
        {
            if ( UserContext.Current.PatientKey.HasValue )
            {
                if ( UserContext.Current.Validated )
                {
                    var requestDispatcher = CreateAsyncRequestDispatcher ();
                    requestDispatcher.Add ( new GetPatientDtoByKeyRequest {PatientKey = UserContext.Current.PatientKey.Value} );
                    var response = await requestDispatcher.GetAsync<GetPatientDtoResponse> ();

                    if ( response.DataTransferObject == null )
                    {
                        throw new HttpException ( 404, "Patient record not found." );
                    }

                    return View ( response.DataTransferObject );
                }
                return ValidateLogin();
            }
            return RedirectToAction ( "Index", "Home" );
        }

        public ActionResult ValidateLogin ()
        {
            return View ("ValidateLogin");
        }

        [HttpPost]
        public async Task<ActionResult> ValidateLogin (string patientId, DateTime dateOfBirth)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add ( new ValidatePatientAccountRequest { SystemAccountKey = UserContext.Current.SystemAccountKey.Value, PatientIdentifier = patientId, DateOfBirth = dateOfBirth } );
            var response = await requestDispatcher.GetAsync<ValidatePatientAccountResponse> ();
            if ( response.IsLocked )
            {
                var signoutMessage = _logoutService.Logout ();
                return Redirect ( signoutMessage.WriteQueryString () );
            }
            if ( response.Validated )
            {
                return RedirectToAction ( "Index" );
            }
            ModelState.AddModelError ( "validation-error", "Invalid information." );
            return View();
        }
    }
}
