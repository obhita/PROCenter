namespace ProCenter.Domain.SecurityModule.Event
{
    using System;
    using CommonModule;

    public class AssignedPatientToSystemAccountEvent : CommitEventBase
    {
        public AssignedPatientToSystemAccountEvent(Guid key, int version, Guid patientKey)
            : base(key, version)
        {
            PatientKey = patientKey;
        }

        public Guid PatientKey { get; private set; }
    }
}
