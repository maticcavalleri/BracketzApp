using Microsoft.AspNetCore.Authorization;

namespace BracketzApp.Requirements
{
    public class UserAdminRequirement : IAuthorizationRequirement
    {
        public bool IsAdmin { get; }
        public UserAdminRequirement(bool isAdmin)
        {
            IsAdmin = isAdmin;
        }

    }
}