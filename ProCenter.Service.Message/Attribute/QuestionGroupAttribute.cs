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