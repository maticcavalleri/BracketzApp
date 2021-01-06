
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace BracketzApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [DefaultValue(false)]
        public bool IsAdmin { get; set; }
        public string RefreshToken { get; set; }
    }
}
