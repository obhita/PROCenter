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

namespace ProCenter.Mvc.Infrastructure.Security
{
    #region Using Statements

    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Pillar.Security.AccessControl;
    using ProCenter.Infrastructure.Security;

    #endregion

    /// <summary>The pro center access control manager class.</summary>
    public class ProCenterAccessControlManager : AccessControlManager, IProvidePermissions, IAccessControlManager
    {
        #region Fields

        private readonly List<Permission> _permissions = new List<Permission> ();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProCenterAccessControlManager"/> class.
        /// </summary>
        /// <param name="currentUserPermissionService">The current user permission service.</param>
        public ProCenterAccessControlManager ( ICurrentUserPermissionService currentUserPermissionService )
            : base ( currentUserPermissionService )
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <value>
        /// The permissions.
        /// </value>
        public IReadOnlyCollection<Permission> Permissions
        {
            get { return new ReadOnlyCollection<Permission> ( _permissions ); }
        }

        #endregion

        #region Explicit Interface Methods

        void IAccessControlManager.RegisterPermissionDescriptor ( params IPermissionDescriptor[] permissionDescriptors )
        {
            var publicPermissionDescritpors = permissionDescriptors.OfType<IInternalPermissionDescriptor> ().Where ( pd => !pd.IsInternal );
            _permissions.AddRange ( publicPermissionDescritpors.SelectMany ( pd => pd.Resources.Select ( r => r.Permission ) ).Distinct () );
            foreach ( var resource in publicPermissionDescritpors.SelectMany ( permissionDescriptor => (List<Resource>) permissionDescriptor.Resources ) )
            {
                GetAllPermissionHelper ( resource );
            }
            RegisterPermissionDescriptor ( permissionDescriptors );
        }

        #endregion

        #region Methods

        private void GetAllPermissionHelper ( Resource resource )
        {
            if ( !_permissions.Contains ( resource.Permission ) )
            {
                _permissions.Add ( resource.Permission );
            }
            if ( resource.Resources != null )
            {
                foreach ( var r in resource.Resources )
                {
                    GetAllPermissionHelper ( r );
                }
            }
        }

        #endregion
    }
}