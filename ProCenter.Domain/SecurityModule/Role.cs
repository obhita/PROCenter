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

namespace ProCenter.Domain.SecurityModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;

    using Pillar.Common.Utility;
    using Pillar.Security.AccessControl;

    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.SecurityModule.Event;

    #endregion

    /// <summary>
    ///     Role aggregate root.
    /// </summary>
    public class Role : AggregateRootBase
    {
        #region Fields

        private readonly List<Permission> _permissions = new List<Permission> ();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Role" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="organizationKey">The organization key.</param>
        /// <param name="roleType">Type of the role.</param>
        public Role ( string name, Guid? organizationKey, RoleType roleType = RoleType.UserDefined )
        {
            Check.IsNotNullOrWhitespace ( name, () => Name );

            Key = CombGuid.NewCombGuid ();
            RaiseEvent ( new RoleCreatedEvent ( Key, Version, name, organizationKey, roleType ) );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        public Role ()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        ///     Gets the organization key.
        /// </summary>
        /// <value>
        ///     The organization key.
        /// </value>
        public Guid? OrganizationKey { get; private set; }

        /// <summary>
        ///     Gets the permissions.
        /// </summary>
        /// <value>
        ///     The permissions.
        /// </value>
        public IEnumerable<Permission> Permissions
        {
            get { return _permissions; }
        }

        /// <summary>
        ///     Gets the type of the role.
        /// </summary>
        /// <value>
        ///     The type of the role.
        /// </value>
        public RoleType RoleType { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Adds the permision.
        /// </summary>
        /// <param name="permission">The permission.</param>
        public void AddPermision ( Permission permission )
        {
            RaiseEvent ( new PermissionAddedEvent ( Key, Version, permission ) );
        }

        /// <summary>
        ///     Removes the permision.
        /// </summary>
        /// <param name="permission">The permission.</param>
        public void RemovePermision ( Permission permission )
        {
            RaiseEvent ( new PermissionRemovedEvent ( Key, Version, permission ) );
        }

        /// <summary>Revises the name.</summary>
        /// <param name="name">The name.</param>
        public void ReviseName ( string name )
        {
            RaiseEvent ( new RoleNameRevisedEvent ( Key, Version, name ) );
        }

        #endregion

        #region Methods

        private void Apply ( RoleNameRevisedEvent roleNameRevisedEvent )
        {
            Name = roleNameRevisedEvent.Name;
        }

        private void Apply ( PermissionAddedEvent permissionAddedEvent )
        {
            _permissions.Add ( permissionAddedEvent.Permission );
        }

        private void Apply ( PermissionRemovedEvent permissionRemovedEvent )
        {
            _permissions.Remove ( permissionRemovedEvent.Permission );
        }

        private void Apply ( RoleCreatedEvent roleCreatedEvent )
        {
            OrganizationKey = roleCreatedEvent.OrganizationKey;
            Name = roleCreatedEvent.Name;
            RoleType = roleCreatedEvent.RoleType;
        }

        #endregion
    }
}