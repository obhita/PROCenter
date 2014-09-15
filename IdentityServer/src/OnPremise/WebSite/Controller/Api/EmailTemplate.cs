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
namespace Thinktecture.IdentityServer.Web.Controller.Api
{
    public static class EmailTemplate
    {
        public static string PasswordSetupMessage = @"
        <html>
        <head>
            <title th:remove='all'>Account Verification</title>
            <meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />
        </head>
        <body style='padding: 0 50px;'>
            <table cellpadding='0' cellspacing='0' border='0' align='center' width='70%' bgcolor='#ffffff' style='padding-top: 10px; font-family: Helvetica, Arial, sans-serif;'>
                <tr>
                    <td bgcolor='#fafafa' style='vertical-align: middle; border: 1px #d2d2d2 solid; font-size: 18px; line-height: 30px; font-weight: 600; color: #777777; padding: 12px'>Password Setup<span><img src='https://procenter-qa.obhita.org/Images/procenter.png' alt='pro-center' width='auto' height='32' align='right' /></span></td>
                </tr>
            </table>
            <table align='center' width='70%' border='0' cellspacing='0' cellpadding='0' style='padding-top: 10px; font-family: Helvetica, Arial, sans-serif;'>
                <tr>
                    <td style='padding-top: 10px;'>
                        <table style='color: #777777; padding-right: 15px; padding-left: 15px;'>
                            <tr>
                                <td style='font-size: 16px;'>Hello <span style='font-weight: 800'>{0}</span>,</td>
                            </tr>
                            <tr>
                                <td style='font-size: 16px; padding-top: 20px;'></td>
                            </tr>
                            <tr>
                                <td style='padding: 20px; background-color: #fcf8e3;'>Use the temporary password provided to <a href='https://procenter-qa.obhita.org' style='color: #006699;'><strong>Log in</strong></a> and update your account information with a new and unique password.
                                    <p style='font-weight: 700;'>Username: <span style='color: Black'>{1}</span></p>
                                    <p style='font-weight: 700;'>Password: <span style='color: Black'>{2}</span></p>
                                </td>
                            </tr>
                            <tr>
                                <td style='font-size: 16px; padding-top: 10px;'>The System Administrator has created an account for you and issued a temporary password. For security reasons, we recommend that you log in to the PROCenter system using the temporary password and update your account with a new and unique password. For more information please visit our <a style='color: #006699' href=''>Privacy Policy</a> page. </td>
                            </tr>
                            <tr>
                                <td style='font-size: 16px; padding-top: 40px;'></td>
                            </tr>
                            <tr>
                                <td style='font-size: 14px; padding-top: 15px;'>Thank you for using PROCenter,</td>
                            </tr>
                            <tr>
                                <td style='font-size: 14px; padding-top: 10px;'><em>--The PROCenter Team</em></td>
                            </tr>
                            <tr>
                                <td style='font-size: 12px; margin-top: -7px;'><a href='' style='color: #006699;'>https://procenter-qa.obhita.org/</a></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <table align='center' width='70%' border='0' cellspacing='0' cellpadding='0' style='padding-top: 80px; font-family: Helvetica, Arial, sans-serif;'>
                <tr>
                    <td>
                        <table width='1' border='0' cellspacing='0' cellpadding='0' class=''>
                            <tr>
                                <td>
                                    <div style='height: 10px; font-size: 10px; line-height: 10px;'>&nbsp;</div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align='left' style='font-size: 11px; font-family: Arial,sans-serif; color: #999999;'>This email is intended for <span style='font-weight: 800;'>{0}</span> and is considered confidential information. If you are not the intended recipient, please disregard this email and delete it from your system. For more information visit our <a style='color: #006699' href=''>Privacy Policy</a> page. &copy; 2013, PROCenter is a Public Service of U.S. DEPARTMENT OF HEALTH AND HUMAN SERVICES Substance Abuse and Mental Health Services Administration <a style='color: #006699' href=''>www.samhsa.org</a>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width='1' border='0' cellspacing='0' cellpadding='0' class=''>
                            <tr>
                                <td>
                                    <div style='height: 10px; font-size: 10px; line-height: 10px;'>&nbsp;</div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </body>
        </html>     
        ";
    }
}