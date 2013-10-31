#region Licence Header
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
namespace ProCenter.Domain.SecurityModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using CommonModule;
    using Event;
    using Pillar.Common.InversionOfControl;
    using Pillar.Common.Utility;
    using Pillar.Domain.Primitives;

    #endregion

    /// <summary>
    ///     System account aggregate.
    /// </summary>
    public class SystemAccount : AggregateRootBase
    {
        private readonly List<Guid> _roleKeys = new List<Guid>();

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SystemAccount" /> class.
        /// </summary>
        /// <param name="organizationKey">The organization key.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="email">The email.</param>
        public SystemAccount(Guid organizationKey, string identifier, Email email)
        {
            Check.IsNotNullOrWhitespace(identifier, () => Identifier);

            Key = CombGuid.NewCombGuid();
            RaiseEvent(new SystemAccountCreatedEvent(Key, Version, organizationKey, identifier, email));
        }

        public SystemAccount()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public string Identifier { get; private set; }

        /// <summary>
        ///     Gets the organization key.
        /// </summary>
        /// <value>
        ///     The organization key.
        /// </value>
        public Guid OrganizationKey { get; private set; }

        /// <summary>
        ///     Gets the email.
        /// </summary>
        /// <value>
        ///     The email.
        /// </value>
        public Email Email { get; private set; }

        /// <summary>
        ///     Gets the staff key.
        /// </summary>
        /// <value>
        ///     The staff key.
        /// </value>
        public Guid? StaffKey { get; private set; }

        /// <summary>
        ///     Gets the patient key.
        /// </summary>
        /// <value>
        ///     The patient key.
        /// </value>
        public Guid? PatientKey { get; private set; }

        /// <summary>
        ///     Gets the role keys.
        /// </summary>
        /// <value>
        ///     The role keys.
        /// </value>
        public IEnumerable<Guid> RoleKeys
        {
            get { return _roleKeys; }
        }

        /// <summary>
        /// Gets the last login time.
        /// </summary>
        /// <value>
        /// The last login time.
        /// </value>
        public DateTime? LastLoginTime { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="SystemAccount" /> is validated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if validated; otherwise, <c>false</c>.
        /// </value>
        public bool Validated { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is locked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is locked; otherwise, <c>false</c>.
        /// </value>
        public bool IsLocked { get; private set; }

        /// <summary>
        /// Gets the locked time.
        /// </summary>
        /// <value>
        /// The locked time.
        /// </value>
        public DateTime? LockedTime { get; private set; }

        #endregion

        public void LogIn ()
        {
            RaiseEvent ( new SystemAccountLoggedInEvent ( Key, Version, DateTime.Now ) );
        }

        public void Validate ()
        {
            RaiseEvent( new SystemAccountValidatedEvent(Key, Version, true));
        }

        public void Lock ()
        {
            if ( !IsLocked )
            {
                RaiseEvent ( new SystemAccountLockedEvent ( Key, Version, DateTime.Now ) );
            }
        }

        public void UnLock ()
        {
            if ( IsLocked )
            {
                RaiseEvent ( new SystemAccountUnLockedEvent ( Key, Version ) );
            }
        }

        public void AddRole(Guid roleKey)
        {
            RaiseEvent(new SystemAccountRoleAddedEvent(Key, Version, roleKey));
        }

        public void RemoveRole(Guid roleKey)
        {
            RaiseEvent(new SystemAccountRoleRemovedEvent(Key, Version, roleKey));
        }

        public void AssignToStaff(Guid staffKey)
        {
            if ( PatientKey == null )
            {
                RaiseEvent ( new AssignedStaffToSystemAccountEvent ( Key, Version, staffKey ) );
            }
            else
            {
                //todo: raise domain error event
            }
        }

        public void AssignToPatient(Guid patientKey)
        {
            if ( StaffKey == null )
            {
                RaiseEvent ( new AssignedPatientToSystemAccountEvent ( Key, Version, patientKey ) );
            }
            else
            {
                //todo: raise domain error event
            }
        }

        #region Methods

        private void Apply ( SystemAccountLockedEvent systemAccountLockedEvent )
        {
            IsLocked = true;
            LockedTime = systemAccountLockedEvent.Time;
        }

        private void Apply(SystemAccountUnLockedEvent systemAccountUnLockedEvent)
        {
            IsLocked = false;
            LockedTime = null;
        }

        private void Apply ( SystemAccountValidatedEvent systemAccountValidatedEvent )
        {
            Validated = systemAccountValidatedEvent.Validated;
        }

        private void Apply ( SystemAccountLoggedInEvent systemAccountLoggedInEvent )
        {
            LastLoginTime = systemAccountLoggedInEvent.Time;
        }

        private void Apply(AssignedStaffToSystemAccountEvent assignedStaffToSystemAccountEvent)
        {
            StaffKey = assignedStaffToSystemAccountEvent.StaffKey;
        }

        private void Apply(AssignedPatientToSystemAccountEvent assignedPatientToSystemAccountEvent)
        {
            PatientKey = assignedPatientToSystemAccountEvent.PatientKey;
        }

        private void Apply(SystemAccountRoleAddedEvent systemAccountRoleAddedEvent)
        {
            _roleKeys.Add(systemAccountRoleAddedEvent.RoleKey);
        }

        private void Apply(SystemAccountRoleRemovedEvent systemAccountRoleRemovedEvent)
        {
            _roleKeys.Remove(systemAccountRoleRemovedEvent.RoleKey);
        }

        private void Apply(SystemAccountCreatedEvent systemAccountCreatedEvent)
        {
            OrganizationKey = systemAccountCreatedEvent.OrganizationKey;
            Identifier = systemAccountCreatedEvent.Identifier;
            Email = systemAccountCreatedEvent.Email;
        }

        #endregion
    }
}