namespace ProCenter.Service.Message.Security
{
    using System;
    using Agatha.Common;

    public class GetRoleDtoByKeyRequest : Request
    {
        public Guid Key { get; set; }
    }
}