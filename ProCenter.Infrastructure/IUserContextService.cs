using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCenter.Infrastructure
{
    public interface IUserContextService
    {
        string UserName { get; }
        string UserId { get; }
    }
}
