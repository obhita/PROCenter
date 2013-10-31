namespace ProCenter.Domain.SecurityModule
{
    public class RoleFactory : IRoleFactory
    {
        public Role Create(string name, RoleType roleType = RoleType.UserDefined)
        {
            return new Role(name, roleType);
        }
    }
}