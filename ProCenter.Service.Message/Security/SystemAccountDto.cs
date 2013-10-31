namespace ProCenter.Service.Message.Security
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common;

    public class SystemAccountDto : KeyedDataTransferObject
    {
        public Guid? StaffKey { get; set; }

        public string Identifier { get; set; }

        [Required]
        public string Email { get; set; }

        public IEnumerable<RoleDto> Roles { get; set; }

        [Required]
        public string Username { get; set; }

        public bool CreateNew { get; set; }
    }
}