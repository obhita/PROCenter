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
namespace ProCenter.LocalSTS
{
    #region Using Statements

    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web.Security;
    using System.Web.UI;
    using STS;

    #endregion

    public partial class newLogin : Page
    {
        private UserLogin[] _authenticatedUsers;
        private UserLogin _configUser;

        private bool IsAuthentic(UserLogin userLogin)
        {
            if (userLogin == null)
                return false;

            bool valid = _authenticatedUsers.Any(u => (u.Login == userLogin.Login && u.Password == userLogin.Password));
            return valid;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (_authenticatedUsers == null)
            {
                _authenticatedUsers = new UserLogin[]
                    {
                        new UserLogin {Login = "cindy.thomas", Password = "P@$$w0rd"},
                        new UserLogin {Login = "leo.smith", Password = "P@$$w0rd"},
                        new UserLogin {Login = "system.admin", Password = "P@$$w0rd"},
                        new UserLogin {Login = "oren.ishii", Password = "P@$$w0rd"},
                        new UserLogin {Login = "lily.foley", Password = "P@$$w0rd"},
                        new UserLogin {Login = "chris.white", Password = "P@$$w0rd"},
                        new UserLogin {Login = "dennis.ladder", Password = "P@$$w0rd"},
                        new UserLogin {Login = "BillingClaimsProcessor", Password = "P@$$w0rd"},
                    };
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string user = UserName.Text;
            string password = Password.Text;


            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                var _userLoginSection = (UserLoginSection) ConfigurationManager.GetSection("userLoginGroup/userLogin");
                if (_userLoginSection != null)
                {
                    _configUser = new UserLogin()
                        {
                            Login = _userLoginSection.UserLogin,
                            Password = _userLoginSection.UserPassword
                        };
                }
            }
            else
            {
                _configUser = new UserLogin() {Login = user, Password = password};
            }

            // Note: Add code to validate user name, password. This code is for illustrative purpose only.
            // Do not use it in production environment.)
            if (IsAuthentic(_configUser))
            {
                if (Request.QueryString["ReturnUrl"] != null)
                {
                    FormsAuthentication.RedirectFromLoginPage(_configUser.Login, false);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(_configUser.Login, false);
                    Response.Redirect("default.aspx");
                }
            }
        }

        private class UserLogin
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }
    }
}