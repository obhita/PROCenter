using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCenter.Domain.OrganizationModule
{
    public interface ITeamFactory
    {
        Team Create ( Guid organizationKey, string name );
    }
}
