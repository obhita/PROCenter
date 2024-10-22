﻿#region License Header

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

namespace ProCenter.Domain.AssessmentModule.Event
{
    #region Using Statements

    using System;

    using ProCenter.Domain.CommonModule;

    #endregion

    /// <summary>Event for when an assessment definition is created.</summary>
    public class AssessmentDefinitionCreatedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentDefinitionCreatedEvent" /> class.
        /// </summary>
        /// <param name="assessmentDefinitionKey">The assessment definition key.</param>
        /// <param name="version">The version.</param>
        /// <param name="codedConcept">The coded concept.</param>
        /// <param name="scoreTypeEnum">The score type enum.</param>
        public AssessmentDefinitionCreatedEvent ( Guid assessmentDefinitionKey, int version, CodedConcept codedConcept, ScoreTypeEnum scoreTypeEnum )
            : base ( assessmentDefinitionKey, version )
        {
            CodedConcept = codedConcept;
            ScoreType = scoreTypeEnum;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the coded concept.
        /// </summary>
        /// <value>
        ///     The coded concept.
        /// </value>
        public CodedConcept CodedConcept { get; private set; }

        /// <summary>
        /// Gets or sets the type of the score.
        /// </summary>
        /// <value>
        /// The type of the score.
        /// </value>
        public ScoreTypeEnum ScoreType { get; set; }

        #endregion
    }
}