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