using Common.Entities;
using Microsoft.AspNetCore.Authorization;

namespace MGSUCore.Authentification
{
    public class IsInRole : IAuthorizationRequirement
    {
        public UserRole AccountRole { get; private set; }

        public IsInRole(UserRole accountRole)
        {
            AccountRole = accountRole;
        }
    }

    public class IsAuthentificated : IAuthorizationRequirement
    {
    }
}