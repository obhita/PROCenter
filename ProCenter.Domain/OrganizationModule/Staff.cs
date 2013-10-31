namespace ProCenter.Domain.OrganizationModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using CommonModule;
    using Event;
    using Pillar.Common.Utility;
    using Pillar.Domain.Primitives;
    using Primitive;

    #endregion

    /// <summary>
    ///     Staff aggregate
    /// </summary>
    public class Staff : AggregateRootBase
    {

        private static Dictionary<string, PropertyInfo> _propertyCache;

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Staff" /> class.
        /// </summary>
        /// <param name="organizationKey">The organization key.</param>
        /// <param name="name">The name.</param>
        public Staff ( Guid organizationKey, PersonName name )
        {
            Check.IsNotNull ( name, () => Name );

            Key = CombGuid.NewCombGuid ();
            RaiseEvent ( new StaffCreatedEvent ( Key, Version, organizationKey, name ) );
        }

        public Staff()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the email.
        /// </summary>
        /// <value>
        ///     The email.
        /// </value>
        public Email Email { get; private set; }

        /// <summary>
        ///     Gets the location.
        /// </summary>
        /// <value>
        ///     The location.
        /// </value>
        public string Location { get; private set; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public PersonName Name { get; private set; }

        /// <summary>
        ///     Gets the organization key.
        /// </summary>
        /// <value>
        ///     The organization key.
        /// </value>
        public Guid OrganizationKey { get; private set; }


        /// <summary>
        ///     Gets the National Practive Id.
        /// </summary>
        /// <value> 
        ///     The NPI number.
        /// </value>
        public string NPI { get; private set; }

        #endregion

        public virtual void ReviseName(PersonName name)
        {
            Check.IsNotNull(name, () => Name);
            RaiseEvent(new StaffChangedEvent(Key, Version, s => s.Name, name));
        }

        public virtual void ReviseEmail(Email email)
        {
            RaiseEvent(new StaffChangedEvent(Key, Version, s=>s.Email, email));
        }

        public virtual void ReviseLocation(string location)
        {
            RaiseEvent(new StaffChangedEvent(Key, Version, s=>s.Location, location));
        }

        public virtual void ReviseNpi(string npi)
        {
            RaiseEvent(new StaffChangedEvent(Key, Version, s=>s.NPI, npi));
        }

        #region Methods

        private void Apply ( StaffCreatedEvent staffCreatedEvent )
        {
            OrganizationKey = staffCreatedEvent.OrganizationKey;
            Name = staffCreatedEvent.Name;
        }

        private void Apply(StaffChangedEvent staffChangedEvent)
        {
            var propertyName = staffChangedEvent.Property;
            var value = staffChangedEvent.Value;
            if (_propertyCache == null)
            {
                _propertyCache =
                    GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                        .ToDictionary(pi => pi.Name);
            }
            var property = _propertyCache.ContainsKey(propertyName) ? _propertyCache[propertyName] : null;
            if (property == null)
            {
                throw new InvalidOperationException(string.Format("Invalid property name {0}", propertyName));
            }
            
            if (value != null && !property.PropertyType.IsInstanceOfType(value))
            {
                var convertToType = GetConvertToType(property.PropertyType);
                value = Convert.ChangeType(value, convertToType);
            }
            property.SetValue(this, value);
        }

        private static Type GetConvertToType(Type propertyType)
        {
            var convertToType = propertyType;
            var underlyingType = Nullable.GetUnderlyingType(convertToType);
            if (underlyingType != null)
            {
                convertToType = underlyingType;
            }
            return convertToType;
        }

        #endregion
    }
}