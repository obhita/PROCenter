namespace ProCenter.Service.Message.Attribute
{
    #region Using Statements

    using System;
    using System.Web.Mvc;
    using Common;

    #endregion

    public class QuestionAttribute : Attribute, IMetadataAware
    {
        public const string Question = "Question";

        public QuestionAttribute(QuestionType type)
        {
            Type = type;
        }

        public QuestionType Type { get; private set; }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[Question] = Type;
        }
    }
}