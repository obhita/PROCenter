namespace ProCenter.Service.Message.Security
{
    using System;
    using Agatha.Common;

    public class UpdateRoleRequest : Request
    {
        public string  Name { get; set; }
        public Guid Key { get; set; }
    }
}