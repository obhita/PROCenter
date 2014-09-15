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

namespace ProCenter.Domain.OrganizationModule
{
    #region Using Statements

    using System.Linq;

    using Dapper;

    using Pillar.FluentRuleEngine;

    using ProCenter.Common;
    using ProCenter.Domain.OrganizationModule.Event;

    #endregion

    /// <summary>
    /// Rule collection for patient domain.
    /// </summary>
    public class StaffRuleCollection : AbstractRuleCollection<Staff>
    {
        private readonly IOrganizationRepository _organizationRepository;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StaffRuleCollection" /> class.
        /// </summary>
        /// <param name="organizationRepository">The organization repository.</param>
        public StaffRuleCollection (IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;

            NewRule ( () => NpiMinLengthRule )
                .When (
                    ( s, ctx ) =>
                    {
                        var staffChangeEvent = ctx.WorkingMemory.GetContextObject<StaffChangedEvent> ();
                        return staffChangeEvent.Value != null
                               && !string.IsNullOrWhiteSpace ( staffChangeEvent.Value.ToString () )
                               && staffChangeEvent.Value.ToString ().Length < 10;
                    } )
                .ThenReportRuleViolation ( OrganizationResource.RuleLength, null, "NPI");

            NewRule ( () => NpiMaxLengthRule )
                .OnContextObject<StaffChangedEvent> ()
                .WithProperty ( pce => pce.Value )
                .UseSubjectForRuleViolation ( p => p.NPI )
                .MaxLength ( 10 );

            NewRule ( () => NpiOnlyLettersRule )
                .OnContextObject<StaffChangedEvent> ()
                .WithProperty ( pce => pce.Value )
                .UseSubjectForRuleViolation ( p => p.NPI )
                .MatchesRegex ( "^[0-9]$" )
                .NotNullOrWhitespace ();

            NewRule ( () => NpiValidCheckSumRule )
                .When (
                    ( s, ctx ) =>
                    {
                        var staffChangeEvent = ctx.WorkingMemory.GetContextObject<StaffChangedEvent> ();
                        return IsNpiCheckSumInValid ( staffChangeEvent.Value.ToString () );
                    } )
                .ThenReportRuleViolation ( OrganizationResource.RuleLastDigitInvalid, null, "NPI" );

            NewRule(() => NpiUniqueRule)
                .When(
                    (s, ctx) =>
                    {
                        var staffChangeEvent = ctx.WorkingMemory.GetContextObject<StaffChangedEvent>();
                        return IsNpiNotUnique(staffChangeEvent.Value.ToString());
                    })
                .ThenReportRuleViolation(OrganizationResource.RuleUnique, null, "NPI");

            NewRule ( () => FirstNameRequiredRule )
                .OnContextObject<StaffChangedEvent> ()
                .WithProperty ( pce => pce.Value )
                .UseSubjectForRuleViolation ( p => p.Name.FirstName )
                .NotNullOrWhitespace ();

            NewRule(() => LastNameRequiredRule)
                .OnContextObject<StaffChangedEvent>()
                .WithProperty(pce => pce.Value)
                .UseSubjectForRuleViolation(p => p.Name.LastName)
                .NotNullOrWhitespace();

            NewRuleSet(() => ReviseNpiRuleSet, NpiMinLengthRule, NpiMaxLengthRule, NpiValidCheckSumRule, NpiUniqueRule );

            NewRuleSet ( () => ReviseFirstNameRuleSet, FirstNameRequiredRule );

            NewRuleSet(() => ReviseLastNameRuleSet, LastNameRequiredRule);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the npi maximum length rule.
        /// </summary>
        /// <value>
        ///     The npi maximum length rule.
        /// </value>
        public IRule NpiMaxLengthRule { get; protected set; }

        /// <summary>
        ///     Gets or sets the npi minimum length rule.
        /// </summary>
        /// <value>
        ///     The npi minimum length rule.
        /// </value>
        public IRule NpiMinLengthRule { get; protected set; }

        /// <summary>
        /// Gets or sets the npi unique rule.
        /// </summary>
        /// <value>
        /// The npi unique rule.
        /// </value>
        public IRule NpiUniqueRule { get; protected set; }

        /// <summary>
        ///     Gets or sets the npi only letters rule.
        /// </summary>
        /// <value>
        ///     The npi only letters rule.
        /// </value>
        public IRule NpiOnlyLettersRule { get; protected set; }

        /// <summary>
        ///     Gets or sets the npi valid check sum rule.
        /// </summary>
        /// <value>
        ///     The npi valid check sum rule.
        /// </value>
        public IRule NpiValidCheckSumRule { get; protected set; }

        /// <summary>
        ///     Gets or sets the revise npi rule set.
        /// </summary>
        /// <value>
        ///     The revise npi rule set.
        /// </value>
        public IRuleSet ReviseNpiRuleSet { get; protected set; }

        /// <summary>
        /// Gets or sets the revise first name rule set.
        /// </summary>
        /// <value>
        /// The revise first name rule set.
        /// </value>
        public IRuleSet ReviseFirstNameRuleSet { get; protected set; }

        /// <summary>
        /// Gets or sets the first name required rule.
        /// </summary>
        /// <value>
        /// The first name required rule.
        /// </value>
        public IRule FirstNameRequiredRule { get; protected set; }

        /// <summary>
        /// Gets or sets the revise last name rule set.
        /// </summary>
        /// <value>
        /// The revise last name rule set.
        /// </value>
        public IRuleSet ReviseLastNameRuleSet { get; protected set; }

        /// <summary>
        /// Gets or sets the last name required rule.
        /// </summary>
        /// <value>
        /// The last name required rule.
        /// </value>
        public IRule LastNameRequiredRule { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether [is npi check sum in valid] [the specified value].
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns>True if the CheckSum is invalid otherwise False.</returns>
        private static bool IsNpiCheckSumInValid ( string val )
        {
            if ( val == null || string.IsNullOrWhiteSpace ( val ) || val.Length != 10 )
            {
                return false;
            }
            //// verify the check digit on the end.
            //// step 1: double the value of alternate digits, beginning with the right most digit
            //// step 2: add 24 plus individual digits of doubling, plus the unaffected digits
            //// step 3: subtract from the next higher number ending in zero. If sum is 67 then use 70 and 70-67 = 3 for your check digit
            var lastDigit = val.Substring ( val.Length - 1 );
            var sumOfDouble = 0;
            var sumOfNotDouble = 0;

            // go in reverse and take every other digit
            for ( var i = val.Length - 2; i >= 0; i-- )
            {
                if ( i % 2 == 0 )
                {
                    var newDigits = ( int.Parse ( val[i].ToString () ) * 2 ).ToString ();

                    // take both digits and add them together and then add to sum
                    sumOfDouble += newDigits.Sum ( t => int.Parse ( t.ToString () ) );
                }
                else
                {
                    sumOfNotDouble += int.Parse ( val[i].ToString () );
                }
            }

            // now add all digits and 24
            var total = sumOfDouble + sumOfNotDouble + 24;
            var nextHighest = total;
            if ( nextHighest % 10 != 0 )
            {
                // get the first digit and add one to it and then append a zero
                nextHighest = int.Parse ( int.Parse ( nextHighest.ToString ().Substring ( 0, 1 ) ) + 1 + "0" );
            }
            var checkDigit = nextHighest - total;
            return !string.Equals ( checkDigit.ToString (), lastDigit );
        }

        /// <summary>
        /// Determines whether [is npi not unique] [the specified npi].
        /// </summary>
        /// <param name="npi">The npi.</param>
        /// <returns>Returns true if the NPI is already in use.</returns>
        private bool IsNpiNotUnique(string npi)
        {
            var staff = _organizationRepository.GetStaffByNpi ( npi );
            return staff != null && staff.Any ();
        }

        #endregion
    }
}