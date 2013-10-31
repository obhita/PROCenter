namespace TestEHR.Controllers
{
    #region Using Statements

    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using Dapper;
    using Models;

    #endregion

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Click a patient to start PRO Center.";

            var connectionString = WebConfigurationManager.ConnectionStrings["ProCenterSqlDatabase"].ToString();
            const string query = @"SELECT FirstName +' '+ LastName as Name, 
                                          GenderCode as Gender,  
                                          PatientKey   
                                 FROM PatientModule.Patient";

            using (var connection = new SqlConnection(connectionString))
            {
                var patients = connection.Query<PatientViewModel>(query).ToList();

                var patientList = new PatientList() {Patients = patients};

                return View(patientList);
            }
        }


        public ActionResult About()
        {
            ViewBag.Message = "Test EHR system for PRO Center integration.";
            return View();
        }
    }
}