namespace ProCenter.Service.Message.Security
{
    using System;
    using System.Collections.Generic;
    using Agatha.Common;

    public class AssignPermissionRequest : Request
    {
        public Guid  Key { get; set; }
        public bool  Add { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}