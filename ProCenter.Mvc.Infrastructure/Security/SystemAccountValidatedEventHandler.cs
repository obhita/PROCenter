namespace ProCenter.Mvc.Infrastructure.Security
{
    using System.Security.Claims;
    using System.Threading;
    using Domain.SecurityModule.Event;
    using Pillar.Domain.Event;

    public class SystemAccountValidatedEventHandler : IDomainEventHandler<SystemAccountValidatedEvent>
    {
        private readonly IPermissionClaimsManager _permissionClaimsManager;

        public SystemAccountValidatedEventHandler ( IPermissionClaimsManager permissionClaimsManager)
        {
            _permissionClaimsManager = permissionClaimsManager;
        }

        public void Handle ( SystemAccountValidatedEvent systemAccountValidatedEvent )
        {
            _permissionClaimsManager.IssueSystemAccountValidationClaim ( Thread.CurrentPrincipal as ClaimsPrincipal );
        }
    }
}
