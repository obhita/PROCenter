using System.Collections.Generic;

namespace ProCenter.Domain.ReportsModule.ChartAcrossAssessments
{
    using System;

    /// <summary>
    /// The QuestionResponse class.
    /// </summary>
    public class QuestionResponse
    {
        /// <summary>
        /// Gets or sets the type of the input.
        /// </summary>
        /// <value>
        /// The type of the input.
        /// </value>
        public string InputType { get; set; }

        /// <summary>
        /// Gets or sets the assessment code.
        /// </summary>
        /// <value>
        /// The assessment code.
        /// </value>
        public string AssessmentCode { get; set; }

        /// <summary>
        /// Gets or sets the assessment definition key.
        /// </summary>
        /// <value>
        /// The assessment definition key.
        /// </value>
        public Guid AssessmentDefinitionKey { get; set; }

        /// <summary>
        /// Gets or sets the item definition code.
        /// </summary>
        /// <value>
        /// The item definition code.
        /// </value>
        public string ItemDefinitionCode { get; set; }

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>
        /// The question.
        /// </value>
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets the responses.
        /// </summary>
        /// <value>
        /// The responses.
        /// </value>
        public List<string> Responses { get; set; }

        /// <summary>
        /// Gets or sets the localized responses.
        /// </summary>
        /// <value>
        /// The localized responses.
        /// </value>
        public List<string> LocalizedResponses { get; set; } 
    }
}
