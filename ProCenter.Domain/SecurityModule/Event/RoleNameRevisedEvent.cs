namespace ProCenter.Domain.SecurityModule.Event
{
    #region

    using System;
    using CommonModule;

    #endregion

    public class RoleNameRevisedEvent : CommitEventBase
    {
        public RoleNameRevisedEvent(Guid key, int version, string name) : base(key, version)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}