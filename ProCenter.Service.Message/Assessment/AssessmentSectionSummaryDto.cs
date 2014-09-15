namespace ProCenter.Service.Message.Assessment
{
    using System;
    using System.Collections.Generic;

    using ProCenter.Service.Message.Common;
    using ProCenter.Service.Message.Message;

    /// <summary>The assessment section summary dto class.</summary>
    public class AssessmentSectionSummaryDto : KeyedDataTransferObject
    {
        /// <summary>Initializes a new instance of the <see cref="AssessmentSectionSummaryDto"/> class.</summary>
        public AssessmentSectionSummaryDto ()
        {
            Sections = new List<SectionSummaryDto>();
        }

        /// <summary>
        ///     Gets or sets the assessment definition code.
        /// </summary>
        /// <value>
        ///     The assessment definition code.
        /// </value>
        public string AssessmentDefinitionCode { get; set; }

        /// <summary>Gets or sets the name of the assessment.</summary>
        /// <value>The name of the assessment.</value>
        public string AssessmentName { get; set; }

        /// <summary>
        ///     Gets or sets the assessment definition key.
        /// </summary>
        /// <value>
        ///     The assessment definition key.
        /// </value>
        public Guid AssessmentDefinitionKey { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [is complete].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [is complete]; otherwise, <c>false</c>.
        /// </value>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Gets or sets the percent complete.
        /// </summary>
        /// <value>
        /// The percent complete.
        /// </value>
        public double PercentComplete { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [is submitted].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [is submitted]; otherwise, <c>false</c>.
        /// </value>
        public bool IsSubmitted { get; set; }

        /// <summary>
        ///     Gets or sets the patient key.
        /// </summary>
        /// <value>
        ///     The patient key.
        /// </value>
        public Guid PatientKey { get; set; }

        /// <summary>
        ///     Gets or sets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public ScoreDto Score { get; set; }

        /// <summary>
        ///     Gets or sets the sub sections.
        /// </summary>
        /// <value>
        ///     The sub sections.
        /// </value>
        public IList<SectionSummaryDto> Sections { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public IEnumerable<IMessageDto> Messages { get; set; }
    }
}
