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

namespace ProCenter.Service.Message.Report
{
    #region Using Statements

    using System;
    using System.ComponentModel.DataAnnotations;

    using ProCenter.Service.Message.Attribute;
    using ProCenter.Service.Message.Common.Lookups;

    #endregion

    /// <summary>
    /// Base class for all the report parameters dto.
    /// </summary>
    public abstract class BaseParametersDto
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the end date.
        /// </summary>
        /// <value>
        ///     The end date.
        /// </value>
        [Display ( Name = "End Date" )]
        [DisplayFormat ( DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true )]
        [Required]
        public DateTime? EndDate { get; set; }

        /// <summary>
        ///     Gets or sets the patient key.
        /// </summary>
        /// <value>
        ///     The patient key.
        /// </value>
        [Required]
        [Display ( Name = "Patient" )]
        public Guid? PatientKey { get; set; }

        /// <summary>
        ///     Gets or sets the start date.
        /// </summary>
        /// <value>
        ///     The start date.
        /// </value>
        [Display ( Name = "Start Date" )]
        [DisplayFormat ( DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true )]
        [Required]
        public DateTime? StartDate { get; set; }

        /// <summary>
        ///     Gets or sets the time period.
        /// </summary>
        /// <value>
        ///     The time period.
        /// </value>
        [Required]
        [LookupCategory ( "ReportTimePeriod" )]
        [Display ( Name = "Time Period" )]
        public LookupDto TimePeriod { get; set; }


        /// <summary>
        /// Gets the controller action.
        /// </summary>
        /// <value>
        /// The controller action.
        /// </value>
        public virtual string ControllerAction
        {
            get
            {
                return GetType().Name + "ReportBuilder";
            }
        }
        
        /// <summary>
        /// Gets or sets the type of the score.
        /// </summary>
        /// <value>
        /// The type of the score.
        /// </value>
        public IScoreTypeDto ScoreType { get; set; }

        #endregion
    }
}