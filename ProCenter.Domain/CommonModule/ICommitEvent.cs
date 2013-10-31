namespace ProCenter.Domain.CommonModule
{
    #region Using Statements

    using System;
    using Pillar.Domain.Event;

    #endregion

    /// <summary>
    ///     Interface for events that need to be commited.
    /// </summary>
    public interface ICommitEvent : IDomainEvent
    {
        Guid Key { get; }
        Guid? OrganizationKey { get; }
    }
}