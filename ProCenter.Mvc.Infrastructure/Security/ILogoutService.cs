namespace ProCenter.Mvc.Infrastructure.Security
{
    using System.IdentityModel.Services;

    public interface ILogoutService
    {
        SignOutRequestMessage Logout();
    }
}