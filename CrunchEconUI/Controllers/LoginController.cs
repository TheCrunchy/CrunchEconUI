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
        private readonly IUserDataService _dataService;
        private readonly IHttpContextAccessor _contextAccessor;

        public LoginController(IHttpContextAccessor accessor, IUserDataService service)
        {
            this._contextAccessor = accessor;
            this._dataService = service;
        }

        [ResponseCache(NoStore = true, Duration = 0)]
        [HttpGet("~/signin"), HttpPost("~/signin")]
        public IActionResult SignIn()
        {
            Console.WriteLine("BOB");
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
            Console.WriteLine("BOB 2");
            return SignOut(new AuthenticationProperties
            {
                RedirectUri = "/index",
            }, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
