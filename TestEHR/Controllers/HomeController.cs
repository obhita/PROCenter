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