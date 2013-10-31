namespace ProCenter.Domain.PatientModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;
    using Primitive;

    #endregion

    /// <summary>
    ///     Event for when a patient is created.
    /// </summary>
    public class PatientCreatedEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientCreatedEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="organizationKey">Organization Key.</param>
        /// <param name="name">The name.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="uniqueIdentifier">The unique identifier.</param>
        public PatientCreatedEvent(Guid key, int version, Guid organizationKey, PersonName name, DateTime? dateOfBirth, Gender gender, string uniqueIdentifier)
            : base ( key, version )
        {
            OrganizationKey = organizationKey;
            Name = name;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            UniqueIdentifier = uniqueIdentifier;
        }

        #endregion

        #region Public Properties

        public Guid OrganizationKey { get; private set; }

        /// <summary>
        ///     Gets the date of birth.
        /// </summary>
        /// <value>
        ///     The date of birth.
        /// </value>
        public DateTime? DateOfBirth { get; private set; }

        /// <summary>
        ///     Gets the gender.
        /// </summary>
        /// <value>
        ///     The gender.
        /// </value>
        public Gender Gender { get; private set; }

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public string UniqueIdentifier { get; private set; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public PersonName Name { get; private set; }

        #endregion
    }
}