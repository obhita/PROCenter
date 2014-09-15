using System.Collections.Generic;
using ProCenter.Domain.CommonModule.Lookups;

namespace ProCenter.Domain.ReportsModule.PatientsWithSpecificResponseReport
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

        /// <summary>
        /// Gets or sets the name of the parent.
        /// </summary>
        /// <value>
        /// The name of the parent.
        /// </value>
        public string ParentName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is lookup.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is lookup; otherwise, <c>false</c>.
        /// </value>
        public bool IsLookup { get; set; }

        /// <summary>
        /// Gets or sets the name of the assessment.
        /// </summary>
        /// <value>
        /// The name of the assessment.
        /// </value>
        public string AssessmentName { get; set; }
    }
}
