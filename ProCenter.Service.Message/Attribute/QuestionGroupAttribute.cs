namespace ProCenter.Service.Message.Attribute
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    #endregion

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class QuestionGroupAttribute : Attribute, IMetadataAware, IQuestionGroup
    {
        //this is needed to allow multiple instances of the same attribute???
        public const string QuestionGroup = "QuestionGroup";
        private readonly object _typeId = new object();

        public QuestionGroupAttribute(string questionResourceName, string templateName)
            : this(questionResourceName, 0, templateName)
        {
        }

        public QuestionGroupAttribute(string questionResourceName, int applyOrder = 0,
                                      string templateName = "DefaultQuestionGroup")
        {
            QuestionResourceName = questionResourceName;
            HeaderTemplateName = templateName;
            ApplyOrder = applyOrder;
            AdditionalViewData = new Dictionary<string, object>();

            if (HeaderTemplateName.EndsWith("Columns"))
            {
                AdditionalViewData.Add("Columns", HeaderTemplateName.Replace("Columns", "").ToLower());
                HeaderTemplateName = "Columns";
            }
        }

        public override object TypeId
        {
            get { return _typeId; }
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            if (!metadata.AdditionalValues.ContainsKey(QuestionGroup))
            {
                metadata.AdditionalValues[QuestionGroup] = new List<IQuestionGroup>();
            }
            (metadata.AdditionalValues[QuestionGroup] as IList<IQuestionGroup>).Add(this);
        }

        public string QuestionResourceName { get; private set; }
        public string HeaderTemplateName { get; private set; }
        public int ApplyOrder { get; private set; }
        public Dictionary<string, object> AdditionalViewData { get; private set; }
    }
}