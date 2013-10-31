namespace ProCenter.Domain.PatientModule
{
    #region Using Statements

    using System;
    using Event;
    using Pillar.FluentRuleEngine;

    #endregion

    /// <summary>
    ///     Rule collection for patient domain.
    /// </summary>
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
                .WithProperty(pce => pce.Value)
                .UseSubjectForRuleViolation(p => p.DateOfBirth)
                .LessThen ( DateTime.Now );

            NewRuleSet ( () => ReviseDateOfBirthRuleSet, DateOfBirthRequiredRule, DateOfBirthNotGreaterThanCurrentRule );
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
        ///     Gets or sets the revise date of birth rule set.
        /// </summary>
        /// <value>
        ///     The revise date of birth rule set.
        /// </value>
        public IRuleSet ReviseDateOfBirthRuleSet { get; set; }

        #endregion
    }
}