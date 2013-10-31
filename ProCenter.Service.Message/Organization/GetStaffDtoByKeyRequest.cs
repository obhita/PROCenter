namespace ProCenter.Service.Message.Organization
{
    using System;
    using Agatha.Common;

    public class GetStaffDtoByKeyRequest:Request
    {
        public Guid Key { get; set; }
    }
}