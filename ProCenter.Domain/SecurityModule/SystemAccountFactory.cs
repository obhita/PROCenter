namespace ProCenter.Domain.SecurityModule
{
    using System;
    using Pillar.Domain.Primitives;

    public class SystemAccountFactory : ISystemAccountFactory
    {
        public SystemAccount Create(Guid organizationKey, string identifier, Email email)
        {
            return new SystemAccount(organizationKey, identifier, email);
        }
    }
}