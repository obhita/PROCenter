namespace ProCenter.Service.Message.Organization
{
    using System;
    using Agatha.Common;

    public class UpdateStaffRequest:Request
    {
        public enum StaffUpdateType
        {
            Name,
            Email,
            Location,
            NPI
        }

        public Guid StaffKey { get; set; }
        public StaffUpdateType UpdateType { get; set; }
        public object Value { get; set; }
    }
}