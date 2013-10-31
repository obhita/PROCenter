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
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Common;
    using CommonModule;
    using Event;
    using Pillar.Common.InversionOfControl;
    using Pillar.Common.Utility;
    using Pillar.Domain.Primitives;
    using Pillar.FluentRuleEngine;
    using Primitive;
    using SecurityModule;

    #endregion

    /// <summary>
    ///     Patient class.
    /// </summary>
    public class Patient : AggregateRootBase
    {
        #region Fields

        private static Dictionary<string, PropertyInfo> _propertyCache;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Patient" /> class.
        /// </summary>
        public Patient ()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Patient" /> class.
        /// </summary>
        /// <param name="orgaizationKey">Organization Key.</param>
        /// <param name="name">The name.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="gender">The gender.</param>
        internal Patient (Guid orgaizationKey, PersonName name, DateTime? dateOfBirth, Gender gender )
        {
            Check.IsNotNull(orgaizationKey, () => OrganizationKey);
            Check.IsNotNull ( name, () => Name );
            Check.IsNotNull ( dateOfBirth, () => DateOfBirth );
            Check.IsNotNull ( gender, () => Gender );

            Key = CombGuid.NewCombGuid();

            var patientUniqueIdentifierGenerator = IoC.CurrentContainer.Resolve<IPatientUniqueIdentifierGenerator>();
            var uniqueIdentifier = patientUniqueIdentifierGenerator.GenerateUniqueIdentifier(Key, name.LastName, gender, dateOfBirth.Value);
            RaiseEvent(new PatientCreatedEvent(Key, Version, orgaizationKey, name, dateOfBirth, gender, uniqueIdentifier ));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public string UniqueIdentifier { get; protected set; }

        /// <summary>
        /// Gets or sets the organization key.
        /// </summary>
        /// <value>
        /// The organization key.
        /// </value>
        public Guid OrganizationKey { get; protected set; }

        /// <summary>
        ///     Gets the date of birth.
        /// </summary>
        /// <value>
        ///     The date of birth.
        /// </value>
        public DateTime? DateOfBirth { get; protected set; }

        /// <summary>
        ///     Gets the ethnicity.
        /// </summary>
        /// <value>
        ///     The ethnicity.
        /// </value>
        public Ethnicity Ethnicity { get; protected set; }

        /// <summary>
        ///     Gets the gender.
        /// </summary>
        /// <value>
        ///     The gender.
        /// </value>
        public Gender Gender { get; protected set; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public PersonName Name { get; protected set; }


        /// <summary>
        ///     Gets the email.
        /// </summary>
        /// <value>
        ///     The email.
        /// </value>
        public Email Email { get; protected set; }

        /// <summary>
        ///     Gets the religion.
        /// </summary>
        /// <value>
        ///     The religion.
        /// </value>
        public Religion Religion { get; protected set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Revises the date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        public virtual void ReviseDateOfBirth ( DateTime? dateOfBirth )
        {
            var patientChangedEvent = new PatientChangedEvent ( Key, Version, p => p.DateOfBirth, dateOfBirth );
            new RuleEngineExecutor<Patient> ( this )
                .ForCallingMethodRuleSet ()
                .WithContext ( patientChangedEvent )
                .Execute ( () => RaiseEvent (patientChangedEvent) );

        }

        /// <summary>
        ///     Revises the ethnicity.
        /// </summary>
        /// <param name="ethnicity">The ethnicity.</param>
        public virtual void ReviseEthnicity ( Ethnicity ethnicity )
        {
            Check.IsNotNull ( ethnicity, () => Ethnicity );

            RaiseEvent(new PatientChangedEvent(Key, Version, p => p.Ethnicity, ethnicity));
        }

        /// <summary>
        ///     Revises the gender.
        /// </summary>
        /// <param name="gender">The gender.</param>
        public virtual void ReviseGender ( Gender gender )
        {
            Check.IsNotNull ( gender, () => Gender );

            RaiseEvent(new PatientChangedEvent(Key, Version, p => p.Gender, gender));
        }

        /// <summary>
        ///     Revises the name.
        /// </summary>
        /// <param name="name">The name.</param>
        public virtual void ReviseName ( PersonName name )
        {
            Check.IsNotNull ( name, () => Name );

            RaiseEvent(new PatientChangedEvent(Key, Version, p => p.Name, name));
        }

        /// <summary>
        ///     Revises the email.
        /// </summary>
        /// <param name="email">The email.</param>
        public virtual void ReviseEmail(Email email)
        {
            RaiseEvent(new PatientChangedEvent(Key, Version, p => p.Email, email));
        }

        /// <summary>
        ///     Revises the religion.
        /// </summary>
        /// <param name="religion">The religion.</param>
        public virtual void ReviseReligion ( Religion religion )
        {
            Check.IsNotNull ( religion, () => Religion );

            RaiseEvent(new PatientChangedEvent(Key, Version, p => p.Religion, religion));
        }

        public ValidationStatus ValidateInfo(SystemAccount systemAccount, string patientIdentifier, DateTime dateOfBirth)
        {
            if ( string.Equals ( patientIdentifier, UniqueIdentifier ) && DateOfBirth.Value == dateOfBirth )
            {
                systemAccount.Validate ();
                UserContext.Current.RefreshValidationAttempts ();
                return ValidationStatus.Valid;
            }
            if( UserContext.Current.ValidationAttempts >= 3 )
            {
                systemAccount.Lock();
                return ValidationStatus.Locked;
            }
            UserContext.Current.FailedValidationAttempt ();
            return ValidationStatus.AttemptFailed;
        }

        #endregion

        #region Methods

        private static Type GetConvertToType ( Type propertyType )
        {
            var convertToType = propertyType;
            var underlyingType = Nullable.GetUnderlyingType ( convertToType );
            if ( underlyingType != null )
            {
                convertToType = underlyingType;
            }
            return convertToType;
        }

        private void Apply ( PatientCreatedEvent patientCreatedEvent )
        {
            OrganizationKey = patientCreatedEvent.OrganizationKey;
            Name = patientCreatedEvent.Name;
            DateOfBirth = patientCreatedEvent.DateOfBirth;
            Gender = patientCreatedEvent.Gender;
            UniqueIdentifier = patientCreatedEvent.UniqueIdentifier;
        }

        private void Apply ( PatientChangedEvent patientChangedEvent )
        {
            var propertyName = patientChangedEvent.Property;
            var value = patientChangedEvent.Value;
            if ( _propertyCache == null )
            {
                _propertyCache =
                    GetType ()
                        .GetProperties ( BindingFlags.Public | BindingFlags.Instance |
                                         BindingFlags.FlattenHierarchy )
                        .ToDictionary ( pi => pi.Name );
            }
            var property = _propertyCache.ContainsKey ( propertyName ) ? _propertyCache[propertyName] : null;
            if ( property == null )
            {
                throw new InvalidOperationException ( string.Format ( "Invalid property name {0}", propertyName ) );
            }

            if ( value != null && !property.PropertyType.IsInstanceOfType ( value ) )
            {
                var convertToType = GetConvertToType ( property.PropertyType );
                value = Convert.ChangeType ( value, convertToType );
            }

            property.SetValue ( this, value );
        }

        #endregion
    }
}