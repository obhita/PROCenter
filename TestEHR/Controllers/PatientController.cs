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
namespace TestEHR.Controllers
{
    #region Using Statements

    using System;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    #endregion

    public class PatientController : Controller
    {
        public ActionResult Index(Guid key, Guid? assessmentKey = null)
        {
            var issuer = WebConfigurationManager.AppSettings["WsFederationIssuer"];
            var realm = WebConfigurationManager.AppSettings["WsFederationRealm"];
            var baseAddress = WebConfigurationManager.AppSettings["IdentityServerExternalSignInUrl"];
            //var passiveRedirectEnabled = WebConfigurationManager.AppSettings["WsFederationPassiveRedirectEnabled"];
            //var requireHttps = WebConfigurationManager.AppSettings["WsFederationRequireHttps"];
            var ehrId = WebConfigurationManager.AppSettings["EHRId"];

            var now = DateTime.UtcNow;
            // "/IdsrvDev/issue/wsfed?wa=wsignin1.0&wtrealm=https://localhost:44302/&wctx=rm=0&id=passive&ru=/Patient/Index/73FD9AF1-6341-4533-A772-A1BF00E5678E&wct=2013-05-19T02:28:00Z"
            //var returnUrl =@"/IdsrvDev/issue/wsfed?wa=wsignin1.0&wtrealm=https%3a%2f%2flocalhost%3a44302%2f&wctx=rm%3d0%26id%3dpassive%26ru%3d%252fPatient%252fIndex%252f73FD9AF1-6341-4533-A772-A1BF00E5678E&wct=2013-05-19T02%3a28%3a00Z";

            var returnUrl = CertSignService.BuildReturnUrl(issuer, realm, "/Patient/Index/", key.ToString(), now);
            var testString = string.Format("PatientId={0}&EhrId={1}&ReturnUrl={2}&UserId={3}&UserName={4}&UserEmail={5}&Timestamp={6}&AssessmentId={7}",
                                           key, ehrId, returnUrl, "Leo.Smith", "Leo Smith", "Leo.Smith@blah.com", now.ToString(CultureInfo.InvariantCulture),
                                           (assessmentKey.HasValue ? assessmentKey.ToString() : string.Empty));
            var signature = CertSignService.SignCertificate(testString, ehrId + "Cert"); // Note: by convention, signing certificate name is the EHRId+"Cert"
            var model = new RequestModel
                {
                    Url = baseAddress,
                    EhrId = ehrId,
                    ReturnUrl = returnUrl,
                    PatientId = key,
                    AssessmentId = assessmentKey,
                    UserId = "Leo.Smith",
                    UserName = "Leo Smith",
                    UserEmail = "Leo.Smith@blah.com",
                    Timestamp = now.ToString(CultureInfo.InvariantCulture),
                    Token = Convert.ToBase64String(signature)
                };
            return View(model);
        }

        public ActionResult Get(Guid key)
        {
            var baseAddress = new Uri(WebConfigurationManager.AppSettings["WsFederationRealm"]);
            var patientDto = CallService<PatientDto>(baseAddress, JwtTokenContext.Current.Token, "api/patient/get/" + key);
            return Json ( patientDto, JsonRequestBehavior.AllowGet );
        }

        private static T CallService<T>(Uri baseAddress, string token, string path)
        {
            ServicePointManager
                .ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;
            using (var httpClient = new HttpClient() {BaseAddress = baseAddress})
            {
                httpClient.SetToken("Session", token);

                var response = httpClient.GetAsync(path).Result;
                response.EnsureSuccessStatusCode();

                var s = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(s);
                // todo: modify PROCenter app to support both SAML2.0 and JWT (for web api only). When PROCenter app calls PROCenter Web api, no JWT? 
            }
        }
    }

    public class LookupDto {
        public string Code { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }

        public bool IsDefault { get; set; }

        public int? SortOrder { get; set; }

    }

    public class PersonName
    {
        public PersonName()
        {
        }

        public PersonName(string firstName, string lastName)
            : this(null, firstName, null, lastName, null)
        {
        }

        public PersonName(string firstName, string middleName, string lastName)
            : this(null, firstName, middleName, lastName, null)
        {
        }

        public PersonName(string prefix, string firstName, string middleName, string lastName, string suffix)
        {
            Prefix = prefix;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Suffix = suffix;
        }

        public virtual string Prefix { get; private set; }

        public virtual string FirstName { get; private set; }

        public virtual string MiddleName { get; private set; }

        public virtual string LastName { get; private set; }

        public virtual string Suffix { get; private set; }

        public virtual string FullName
        {
            get { return FirstName + " " + LastName; }
        }

    }

    public class PatientDto 
    {
        public PersonName Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public LookupDto Gender { get; set; }
        public LookupDto Ethnicity { get; set; }
        public LookupDto Religion { get; set; }
        public Guid OrganizationKey { get; set; }
    }
}