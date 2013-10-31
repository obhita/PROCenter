namespace ProCenter.Service.Message.Security
{
    using Agatha.Common;

    public class ValidatePatientAccountResponse : Response
    {
        public bool Validated { get; set; }
        public bool IsLocked { get; set; }
    }
}
