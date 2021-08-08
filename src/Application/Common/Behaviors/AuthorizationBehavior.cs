using Application.Common.Attributes;
using Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviors
{
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public readonly ICurrentUserService _currentUserService;
        public readonly IIdentityService _identityService;

        public AuthorizationBehavior(ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requiresAttributes = request.GetType().GetCustomAttributes<RequiresAttribute>();

            if (requiresAttributes.Any())
            {
                // Check that there is a logged in user
                if (string.IsNullOrEmpty(_currentUserService.UserId))
                    throw new UnauthorizedAccessException();

                // Complete any role based authorizations
                var authWithRoles = requiresAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));
                if (authWithRoles.Any())
                {
                    foreach (var roles in authWithRoles.Select(a => a.Roles.Split(',')))
                    {
                        bool authorized = await HasRole(roles);

                        if (!authorized)
                            throw new UnauthorizedAccessException();
                    }
                }

                // Complete policy base authorizations
                var authWithPolicy = requiresAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));
                if (authWithPolicy.Any())
                {
                    foreach (var policy in authWithPolicy.Select(a => a.Policy))
                    {
                        var authorized = await _identityService.AuthorizeAsync(_currentUserService.UserId, policy);
                        if (!authorized)
                            throw new UnauthorizedAccessException();
                    }
                }
            }

            // User is authorized to complete this request
            return await next();
        }

        private async Task<bool> HasRole(string[] roles)
        {
            var authorized = false;
            foreach (var role in roles)
            {
                var isInRole = await _identityService.IsInRoleAsync(_currentUserService.UserId, role);
                if (isInRole)
                {
                    authorized = true;
                    break;
                }
            }

            return authorized;
        }
    }
}
