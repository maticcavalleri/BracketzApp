using Microsoft.IdentityModel.Tokens;
using BracketzApp.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace BracketzApp.Services
{
    public class TokenService
    {
        private string Secret;
        public static string Issuer;
        public static string Audience;
        private IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        //The secret is a base64-encoded string, always make sure to use a secure long string so no one can guess it. ever!.
        //a very recommended approach to use is through the HMACSHA256() class, to generate such a secure secret, you can refer to the below function
        // you can run a small test by calling the GenerateSecureSecret() function to generate a random secure secret once, grab it, and use it as the secret above 
        // or you can save it into appsettings.json file and then load it from them, the choice is yours

        public TokenService(IConfiguration config, UserManager<ApplicationUser> userManager) {
            _config = config;
            _userManager = userManager;

            Secret = _config["JWT:Key"];
            Issuer = _config["JWT:ValidIssuer"];
            Audience = _config["JWT:ValidAudience"];
        }
        public string GenerateSecureSecret()
        {
            var hmac = new HMACSHA256();
            return Convert.ToBase64String(hmac.Key);
        }

        public async Task<string> GenerateToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    authClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));

                var token = new JwtSecurityToken(
                    issuer: Issuer,
                    audience: Audience,
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}