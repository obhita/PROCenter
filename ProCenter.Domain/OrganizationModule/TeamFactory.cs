namespace ProCenter.Domain.OrganizationModule
{
    using System;

    public class TeamFactory : ITeamFactory
    {
        public Team Create ( Guid organizationKey, string name )
        {
            return new Team(organizationKey, name);
        }
    }
}