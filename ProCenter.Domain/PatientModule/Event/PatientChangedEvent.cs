namespace ProCenter.Domain.PatientModule.Event
{
    #region Using Statements

    using System;
    using System.Linq.Expressions;
    using CommonModule;
    using Pillar.Common.Utility;

    #endregion

    /// <summary>
    ///     Event for when a patients property has changed.
    /// </summary>
    public class PatientChangedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PatientChangedEvent" /> class.
        /// </summary>
        public PatientChangedEvent ()
            : base ( Guid.Empty, -1 )
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PatientChangedEvent" /> class.
        /// </summary>
        /// <param name="patientKey">The patient key.</param>
        /// <param name="version">The version.</param>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="value">The value.</param>
        public PatientChangedEvent ( Guid patientKey, int version, Expression<Func<Patient, object>> propertyExpression, object value )
            : base ( patientKey, version )
        {
            // if (propertyExpression == null) return;
            Property = PropertyUtil.ExtractPropertyName ( propertyExpression );
            Value = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the property.
        /// </summary>
        /// <value>
        ///     The property.
        /// </value>
        public string Property { get; private set; }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        public object Value { get; private set; }

        #endregion
    }
}