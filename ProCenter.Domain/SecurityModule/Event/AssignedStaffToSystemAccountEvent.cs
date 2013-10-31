namespace ProCenter.Domain.SecurityModule.Event
{
    using System;
    using CommonModule;

    public class AssignedStaffToSystemAccountEvent : CommitEventBase
    {
        public AssignedStaffToSystemAccountEvent(Guid key, int version, Guid staffKey) : base(key, version)
        {
            StaffKey = staffKey;
        }

        public Guid StaffKey { get; private set; }
    }
}