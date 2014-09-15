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

namespace ProCenter.Service.Message.Message
{
    #region Using Statements

    using System;

    using ProCenter.Service.Message.Assessment;
    using ProCenter.Service.Message.Common;

    #endregion

    /// <summary>The workflow message dto class.</summary>
    public class WorkflowMessageDto : KeyedDataTransferObject, IMessageDto
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the initiating assessment code.
        /// </summary>
        /// <value>
        /// The initiating assessment code.
        /// </value>
        public string InitiatingAssessmentCode { get; set; }

        /// <summary>
        /// Gets or sets the initiating assessment key.
        /// </summary>
        /// <value>
        /// The initiating assessment key.
        /// </value>
        public Guid InitiatingAssessmentKey { get; set; }

        /// <summary>
        /// Gets or sets the initiating assessment score.
        /// </summary>
        /// <value>
        /// The initiating assessment score.
        /// </value>
        public ScoreDto InitiatingAssessmentScore { get; set; }

        /// <summary>
        /// Gets or sets the patient key.
        /// </summary>
        /// <value>
        /// The patient key.
        /// </value>
        public Guid PatientKey { get; set; }

        /// <summary>
        /// Gets or sets the recommended assessment definition code.
        /// </summary>
        /// <value>
        /// The recommended assessment definition code.
        /// </value>
        public string RecommendedAssessmentDefinitionCode { get; set; }

        /// <summary>
        /// Gets or sets the recommended assessment definition key.
        /// </summary>
        /// <value>
        /// The recommended assessment definition key.
        /// </value>
        public Guid RecommendedAssessmentDefinitionKey { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>Determines whether the specified <see cref="System.Object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>True</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>False</c>.</returns>
        public override bool Equals ( object obj )
        {
            if ( ReferenceEquals ( null, obj ) )
            {
                return false;
            }
            if ( ReferenceEquals ( this, obj ) )
            {
                return true;
            }
            if ( obj.GetType () != this.GetType () )
            {
                return false;
            }
            return Equals ( (WorkflowMessageDto)obj );
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. </returns>
        public override int GetHashCode ()
        {
            return Key.GetHashCode ();
        }

        #endregion

        #region Methods

        /// <summary>Equalses the specified other.</summary>
        /// <param name="other">The other.</param>
        /// <returns><c>True</c> if the specified <see cref="WorkflowMessageDto" /> is equal to this instance; otherwise, <c>False</c>.</returns>
        protected bool Equals ( WorkflowMessageDto other )
        {
            return Key.Equals ( other.Key );
        }

        #endregion
    }
}