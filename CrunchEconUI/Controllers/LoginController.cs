using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using CrunchEconUI.Models;
using CrunchEconUI.Interfaces;
using System.Security.Claims;

namespace CrunchEconUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public LoginController(IHttpContextAccessor accessor)
        {
            this._contextAccessor = accessor;
        }

        [ResponseCache(NoStore = true, Duration = 0)]
        [HttpGet("~/signin"), HttpPost("~/signin")]
        public IActionResult SignIn()
        {
            var challenge = Challenge(new AuthenticationProperties
            {
                RedirectUri = "/index",
                IsPersistent = true,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            }, "Steam");
            return challenge;
        }

        [ResponseCache(NoStore = true, Duration = 0)]
        [HttpGet("~/signout"), HttpPost("~/signout")]
        public IActionResult LogOut()
        {
            return SignOut(new AuthenticationProperties
            {
                RedirectUri = "/index",
            }, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
