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