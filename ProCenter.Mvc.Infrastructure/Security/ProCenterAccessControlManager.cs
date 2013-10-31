using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCenter.Mvc.Infrastructure.Security
{
    using System.Collections.ObjectModel;
    using Pillar.Security.AccessControl;
    using ProCenter.Infrastructure.Security;

    public class ProCenterAccessControlManager : AccessControlManager, IProvidePermissions, IAccessControlManager
    {
        private readonly List<Permission> _permissions = new List<Permission> (); 

        public ProCenterAccessControlManager ( ICurrentUserPermissionService currentUserPermissionService )
            : base ( currentUserPermissionService )
        {
        }

        public IReadOnlyCollection<Permission> Permissions { get { return new ReadOnlyCollection<Permission> (_permissions);} }

        void IAccessControlManager.RegisterPermissionDescriptor ( params IPermissionDescriptor[] permissionDescriptors )
        {
            var publicPermissionDescritpors = permissionDescriptors.OfType<IInternalPermissionDescriptor> ().Where ( pd => !pd.IsInternal );
            _permissions.AddRange(publicPermissionDescritpors.SelectMany(pd => pd.Resources.Select(r => r.Permission)).Distinct());
            foreach (var resource in publicPermissionDescritpors.SelectMany(permissionDescriptor => (List<Resource>)permissionDescriptor.Resources))
            {
                GetAllPermissionHelper ( resource );
            }
            base.RegisterPermissionDescriptor ( permissionDescriptors );
        }

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
    }
}
