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

    /// <summary>The question group attribute class.</summary>
    [AttributeUsage ( AttributeTargets.Property, AllowMultiple = true )]
    public class QuestionGroupAttribute : Attribute, IMetadataAware, IQuestionGroup
    {
        //this is needed to allow multiple instances of the same attribute???

        #region Constants

        public const string QuestionGroup = "QuestionGroup";

        #endregion

        #region Fields

        private readonly object _typeId = new object ();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionGroupAttribute"/> class.
        /// </summary>
        /// <param name="questionResourceName">Name of the question resource.</param>
        /// <param name="templateName">Name of the template.</param>
        public QuestionGroupAttribute ( string questionResourceName, string templateName )
            : this ( questionResourceName, 0, templateName )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionGroupAttribute"/> class.
        /// </summary>
        /// <param name="questionResourceName">Name of the question resource.</param>
        /// <param name="applyOrder">The apply order.</param>
        /// <param name="templateName">Name of the template.</param>
        public QuestionGroupAttribute (
            string questionResourceName,
            int applyOrder = 0,
            string templateName = "DefaultQuestionGroup" )
        {
            QuestionResourceName = questionResourceName;
            HeaderTemplateName = templateName;
            ApplyOrder = applyOrder;
            AdditionalViewData = new Dictionary<string, object> ();

            if ( HeaderTemplateName.EndsWith ( "Columns" ) )
            {
                AdditionalViewData.Add ( "Columns", HeaderTemplateName.Replace ( "Columns", string.Empty ).ToLower () );
                HeaderTemplateName = "Columns";
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the additional view data.
        /// </summary>
        /// <value>
        /// The additional view data.
        /// </value>
        public Dictionary<string, object> AdditionalViewData { get; private set; }

        /// <summary>
        /// Gets the apply order.
        /// </summary>
        /// <value>
        /// The apply order.
        /// </value>
        public int ApplyOrder { get; private set; }

        /// <summary>
        /// Gets the name of the header template.
        /// </summary>
        /// <value>
        /// The name of the header template.
        /// </value>
        public string HeaderTemplateName { get; private set; }

        /// <summary>
        /// Gets the name of the question resource.
        /// </summary>
        /// <value>
        /// The name of the question resource.
        /// </value>
        public string QuestionResourceName { get; private set; }

        /// <summary>
        /// When implemented in a derived class, gets a unique identifier for this <see cref="T:System.Attribute" />.
        /// </summary>
        /// <returns>An <see cref="T:System.Object" /> that is a unique identifier for the attribute.</returns>
        public override object TypeId
        {
            get { return _typeId; }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>When implemented in a class, provides metadata to the model metadata creation process.</summary>
        /// <param name="metadata">The model metadata.</param>
        public void OnMetadataCreated ( ModelMetadata metadata )
        {
            if ( !metadata.AdditionalValues.ContainsKey ( QuestionGroup ) )
            {
                metadata.AdditionalValues[QuestionGroup] = new List<IQuestionGroup> ();
            }
            ( metadata.AdditionalValues[QuestionGroup] as IList<IQuestionGroup> ).Add ( this );
        }

        #endregion
    }
}