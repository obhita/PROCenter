namespace Thinktecture.IdentityServer.Web.ViewModels
{
    #region

    using System;

    #endregion

    public class MembershipUserDto
    {
        public string Username { get; set; }
        public string NameIdentifier { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime LastLockoutDate { get; set; }
    }
}