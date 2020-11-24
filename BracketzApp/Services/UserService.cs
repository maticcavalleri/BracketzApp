using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BracketzApp.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor _httpContext;

        public UserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string GetUserId()
        {
            return _httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}