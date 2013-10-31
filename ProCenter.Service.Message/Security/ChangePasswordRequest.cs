namespace ProCenter.Service.Message.Security
{
    using System;
    using Agatha.Common;

    public class ChangePasswordRequest: Request
    {
        public Guid SystemAccountKey { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string BaseBaseIdentityServerUri { get; set; }

        public string Token { get; set; }
    }
}