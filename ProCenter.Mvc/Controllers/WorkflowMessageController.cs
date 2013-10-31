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
namespace ProCenter.Mvc.Controllers
{
    #region Using Statements

    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Agatha.Common;
    using Common;
    using Service.Message.Message;

    #endregion

    public class WorkflowMessageController : BaseController
    {
        public WorkflowMessageController(IRequestDispatcherFactory requestDispatcherFactory) : base(requestDispatcherFactory)
        {
        }

        public async Task<ActionResult> AdministerAssessment(Guid key, Guid patientKey, Guid assessmentDefinitionKey, Guid workflowKey)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new AdministerAssessmentWorkflowMessageRequest {WorkflowMessageKey = key});
            var response = await requestDispatcher.GetAsync<WorkflowMessageStatusChangedResponse>();
            //TODO:check for errors
            if ( UserContext.Current.PatientKey.HasValue )
            {
                return RedirectToAction ( "CreateForSelfAdministration", "Assessment", new {patientKey, assessmentDefinitionKey, administerNow = true, workflowKey} );
            }
            return RedirectToAction("Create", "Assessment", new {patientKey, assessmentDefinitionKey, workflowKey});
        }

        public async Task<ActionResult> Reject(Guid key, Guid assessmentKey, Guid patientKey)
        {
            var requestDispatcher = CreateAsyncRequestDispatcher();
            requestDispatcher.Add(new RejectWorkflowMessageRequest {WorkflowMessageKey = key});
            var response = await requestDispatcher.GetAsync<WorkflowMessageStatusChangedResponse>();
            //TODO:check for errors

            return RedirectToAction("Edit", "Assessment", new { key = assessmentKey, patientKey });
        }
    }
}