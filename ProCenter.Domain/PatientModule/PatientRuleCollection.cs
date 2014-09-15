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

namespace ProCenter.Domain.PatientModule
{
    #region Using Statements

    using System;

    using Pillar.FluentRuleEngine;

    using ProCenter.Domain.PatientModule.Event;

    #endregion

    /// <summary>Rule collection for patient domain.</summary>
    public class PatientRuleCollection : AbstractRuleCollection<Patient>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PatientRuleCollection" /> class.
        /// </summary>
        public PatientRuleCollection ()
        {
            NewRule ( () => DateOfBirthRequiredRule )
                .OnContextObject<PatientChangedEvent> ()
                .WithProperty ( pce => pce.Value )
                .UseSubjectForRuleViolation ( p => p.DateOfBirth )
                .NotNull ();
            NewRule ( () => DateOfBirthNotGreaterThanCurrentRule )
                .OnContextObject<PatientChangedEvent> ()
                .WithProperty ( pce => pce.Value )
                .UseSubjectForRuleViolation ( p => p.DateOfBirth )
                .LessThen ( DateTime.Now );
            NewRule ( () => GenderRequiredRule )
                .OnContextObject<PatientChangedEvent> ()
                .WithProperty ( pce => pce.Value )
                .UseSubjectForRuleViolation ( p => p.Gender )
                .NotNull ();

            NewRuleSet ( () => ReviseDateOfBirthRuleSet, DateOfBirthRequiredRule, DateOfBirthNotGreaterThanCurrentRule );
            NewRuleSet ( () => ReviseGenderRuleSet, GenderRequiredRule );
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the date of birth not greater than current rule.
        /// </summary>
        /// <value>
        ///     The date of birth not greater than current rule.
        /// </value>
        public IRule DateOfBirthNotGreaterThanCurrentRule { get; set; }

        /// <summary>
        ///     Gets or sets the date of birth required rule.
        /// </summary>
        /// <value>
        ///     The date of birth required rule.
        /// </value>
        public IRule DateOfBirthRequiredRule { get; set; }

        /// <summary>
        ///     Gets or sets the gender required rule.
        /// </summary>
        /// <value>
        ///     The gender required rule.
        /// </value>
        public IRule GenderRequiredRule { get; set; }

        /// <summary>
        ///     Gets or sets the revise date of birth rule set.
        /// </summary>
        /// <value>
        ///     The revise date of birth rule set.
        /// </value>
        public IRuleSet ReviseDateOfBirthRuleSet { get; set; }

        /// <summary>
        ///     Gets or sets the revised gender rule set.
        /// </summary>
        /// <value>
        ///     The revised gender rule set.
        /// </value>
        public IRuleSet ReviseGenderRuleSet { get; protected set; }

        #endregion
    }
}