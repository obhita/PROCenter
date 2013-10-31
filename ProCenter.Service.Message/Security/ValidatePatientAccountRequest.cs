namespace ProCenter.Service.Message.Security
{
    using System;
    using Agatha.Common;

    public class ValidatePatientAccountRequest : Request
    {
        public Guid SystemAccountKey { get; set; }
        public string PatientIdentifier { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
