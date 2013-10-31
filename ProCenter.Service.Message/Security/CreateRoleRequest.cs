namespace ProCenter.Service.Message.Security
{
    #region

    using System;
    using Agatha.Common;

    #endregion

    public class CreateRoleRequest : Request
    {
        public string  Name { get; set; }
        public Guid OrganizationKey { get; set; }
    }
}