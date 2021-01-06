using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using BracketzApp.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using BracketzApp.Data;
using BracketzApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
namespace BracketzApp.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IConfiguration _config;
        private readonly ApplicationDbContext _context;
        private TokenService _tokenHelper;
        private readonly ILogger<AuthenticationApiController> _logger;
        public AuthenticationApiController(
            ApplicationDbContext context, 
            IConfiguration config, 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            TokenService tokenHelper,
            ILogger<AuthenticationApiController> logger)
        {
            _context = context;
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHelper = tokenHelper;
            _logger = logger;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid) {
                var signIn = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
                _logger.LogInformation(User.Identity.Name);

                if (signIn.Succeeded) {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                    {
                        string token = await _tokenHelper.GenerateToken(user);
                        user.RefreshToken = Guid.NewGuid().ToString();
                        await _userManager.UpdateAsync(user);

                        CookieOptions cookieOptions = new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict };

                        Response?.Cookies.Append("X-Access-Token", token, cookieOptions);
                        Response?.Cookies.Append("X-Username", user.UserName, cookieOptions);
                        Response?.Cookies.Append("X-Refresh-Token", user.RefreshToken, cookieOptions);

                        return Ok(new
                        {
                            username = user.UserName,
                            email = user.Email,
                            token = token
                        });
                    } else {
                        return BadRequest(ModelState);
                    }
                }
            }
            
            return Unauthorized();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpGet]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!(Request.Cookies.TryGetValue("X-Username", out var userName) && Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken)))
                return BadRequest();

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return BadRequest();

            var token = await _tokenHelper.GenerateToken(user);
            user.RefreshToken = Guid.NewGuid().ToString();
            await _userManager.UpdateAsync(user);

            Response?.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response?.Cookies.Append("X-Username", user.UserName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response?.Cookies.Append("X-Refresh-Token", user.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok();
        }
    }
}