namespace ProCenter.Service.Message.Attribute
{
    #region Using Statements

    using System.Collections.Generic;

    #endregion

    public interface IQuestionGroup
    {
        string QuestionResourceName { get; }
        string HeaderTemplateName { get; }
        int ApplyOrder { get; }
        Dictionary<string, object> AdditionalViewData { get; }
    }
}