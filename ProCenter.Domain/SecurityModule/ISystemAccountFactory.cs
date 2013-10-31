namespace ProCenter.Domain.SecurityModule
{
    using System;
    using Pillar.Domain.Primitives;

    internal interface ISystemAccountFactory
    {
        SystemAccount Create(Guid organizationKey, string identifier, Email email);
    }
}