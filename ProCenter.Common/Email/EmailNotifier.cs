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

namespace ProCenter.Common.Email
{
    #region Using Statements

    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Text;

    using NLog;

    #endregion

    /// <summary>
    /// Email notifier class.
    /// </summary>
    public class EmailNotifier : IEmailNotifier
    {
        #region Static Fields

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger ();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Sends the specified email message.
        /// </summary>
        /// <param name="emailMessage">The email message.</param>
        /// <returns>Returns true if email was sent successfully, else false.</returns>
        public bool Send ( EmailMessage emailMessage )
        {
            if ( !emailMessage.ToAddresses.Any () && !emailMessage.CcAddresses.Any ()
                 && !emailMessage.BccAddresses.Any () )
            {
                _logger.Error ( "Email notifier failed, there is no To or CC or BCC addresses" );
                return false;
            }

            try
            {
                using ( var message = new MailMessage
                                          {
                                              Subject = emailMessage.Subject,
                                              Body = emailMessage.Body,
                                              BodyEncoding = Encoding.UTF8,
                                              IsBodyHtml = emailMessage.IsHtml,
                                          } )
                {
                    emailMessage.ToAddresses.ForEach ( to => message.To.Add ( new MailAddress ( to ) ) );
                    emailMessage.CcAddresses.ForEach ( cc => message.CC.Add ( new MailAddress ( cc ) ) );
                    emailMessage.BccAddresses.ForEach ( bcc => message.Bcc.Add ( new MailAddress ( bcc ) ) );

                    var smtp = new SmtpClient ();
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    smtp.Send(message);
                    _logger.Info ( "Email sent successfully." );
                }
            }
            catch ( Exception ex )
            {
                _logger.ErrorException ( "Email notifier failed.", ex );
                return false;
            }

            return true;
        }

        #endregion
    }
}