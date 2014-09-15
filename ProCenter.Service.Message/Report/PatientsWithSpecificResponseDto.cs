namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ProCenter.Service.Message.Attribute;
    using ProCenter.Service.Message.Common.Lookups;

    #endregion

    /// <summary>
    /// The PatientsWithSpecificResponseDto class.
    /// </summary>
    public class PatientsWithSpecificResponseDto : BaseParametersDto
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the assessment.
        /// </summary>
        /// <value>
        /// The name of the assessment.
        /// </value>
        public string AssessmentName { get; set; }

        /// <summary>
        /// Gets or sets the response values.
        /// </summary>
        /// <value>
        /// The response values.
        /// </value>
        public List<QuestionResponse> QuestionResponses { get; set; }

        /// <summary>
        ///     Gets or sets the assessment definition code.
        /// </summary>
        /// <value>
        ///     The assessment definition code.
        /// </value>
        public string AssessmentDefinitionCode { get; set; }

        /// <summary>
        /// Gets or sets the age range low.
        /// </summary>
        /// <value>
        /// The age range low.
        /// </value>
        public int? AgeRangeLow { get; set; }

        /// <summary>
        /// Gets or sets the age range high.
        /// </summary>
        /// <value>
        /// The age range high.
        /// </value>
        public int? AgeRangeHigh { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        [LookupCategory("Gender")]
        public LookupDto Gender { get; set; }

        #endregion
    }
}