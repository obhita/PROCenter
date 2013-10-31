namespace ProCenter.Service.Message.Assessment
{
    #region Using Statements

    using Common;

    #endregion

    public class AssessmentDefinitionDto : KeyedDataTransferObject
    {
        public string AssessmentName { get; set; }
        public string AssessmentCode { get; set; }
    }
}