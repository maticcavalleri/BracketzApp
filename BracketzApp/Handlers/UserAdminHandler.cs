using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using BracketzApp.Services;
using BracketzApp.Requirements;

namespace BracketzApp.Handlers
{
    public class UserAdminHandler : AuthorizationHandler<UserAdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserAdminRequirement requirement)
        {
            var claim = context.User.FindFirst(c => c.Type == "IsAdmin" && c.Issuer == TokenService.Issuer);
            if (!context.User.HasClaim(c => c.Type == "IsAdmin" && c.Issuer == TokenService.Issuer))
            {
                return Task.CompletedTask;
            }

            string value = context.User.FindFirst(c => c.Type == "IsAdmin" && c.Issuer == TokenService.Issuer).Value;
            var isAdmin = Convert.ToBoolean(value);

            if (isAdmin == requirement.IsAdmin)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}