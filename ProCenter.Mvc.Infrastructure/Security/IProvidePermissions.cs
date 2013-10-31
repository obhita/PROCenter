namespace ProCenter.Mvc.Infrastructure.Security
{
    using System.Collections.Generic;
    using Pillar.Security.AccessControl;

    public interface IProvidePermissions
    {
        IReadOnlyCollection<Permission> Permissions { get; }
    }
}