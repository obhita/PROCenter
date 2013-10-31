namespace ProCenter.LocalSTS.STS
{
    #region Using Statements

    using System.Configuration;

    #endregion

    public class UserRoleSection : ConfigurationSection
    {
        [ConfigurationProperty("Roles", DefaultValue = "", IsRequired = true)]
        public string Roles
        {
            get { return (string) this["Roles"]; }
            set { this["Roles"] = value; }
        }
    }
}