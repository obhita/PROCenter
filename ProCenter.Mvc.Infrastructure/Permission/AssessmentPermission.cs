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
namespace ProCenter.Mvc.Infrastructure.Permission
{
    using Pillar.Security.AccessControl;

    public class AssessmentPermission
    {
        #region Static Fields

        /// <summary>
        ///     The assessment edit permission
        /// </summary>
        public static Permission AssessmentEditPermission = new Permission {Name = "assessmentmodule/assessmentedit"};

        /// <summary>
        ///     The assessment view permission
        /// </summary>
        public static Permission AssessmentViewPermission = new Permission { Name = "assessmentmodule/assessmentview" };

        /// <summary>
        ///     The assessment edit permission
        /// </summary>
        public static Permission AssessmentReminderEditPermission = new Permission { Name = "assessmentmodule/assessmentreminderedit" };

        /// <summary>
        ///     The assessment view permission
        /// </summary>
        public static Permission AssessmentReminderViewPermission = new Permission { Name = "assessmentmodule/assessmentreminderview" };

        /// <summary>
        ///     The report edit permission
        /// </summary>
        public static Permission ReportEditPermission = new Permission { Name = "assessmentmodule/reportedit" };

        /// <summary>
        ///     The report view permission
        /// </summary>
        public static Permission ReportViewPermission = new Permission { Name = "assessmentmodule/reportview" };

        #endregion
    }
}