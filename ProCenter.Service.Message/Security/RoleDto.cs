namespace ProCenter.Service.Message.Security
{
    #region

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common;

    #endregion

    public class RoleDto : KeyedDataTransferObject
    {
        [Required]
        public string Name { get; set; }

        public bool IsBuiltIn { get; set; }

        public IEnumerable<string> Permissions { get; set; }
    }
}