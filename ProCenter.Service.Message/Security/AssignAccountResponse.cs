namespace ProCenter.Service.Message.Security
{
    using Agatha.Common;

    public class AssignAccountResponse : Response
    {
        public SystemAccountDto SystemAccountDto { get; set; }
    }
}