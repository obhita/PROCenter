namespace ProCenter.Infrastructure.Security
{
    using Pillar.Security.AccessControl;

    public interface IInternalPermissionDescriptor : IPermissionDescriptor
    {
        bool IsInternal { get; }
    }
}
