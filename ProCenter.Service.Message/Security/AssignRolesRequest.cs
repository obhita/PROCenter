namespace ProCenter.Service.Message.Security
{
    using System;
    using System.Collections.Generic;
    using Agatha.Common;

    public class AssignRolesRequest:Request
    {
        public Guid SystemAccoutnKey { get; set; }

        public bool AddRoles { get; set; }

        public IEnumerable<Guid> Roles { get; set; }
    }
}