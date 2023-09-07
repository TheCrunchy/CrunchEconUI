using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CrunchEconUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        [ResponseCache(NoStore = true, Duration = 0)]
        [HttpGet("~/signin"), HttpPost("~/signin")]
        public IActionResult SignIn()
        {
            Console.WriteLine("BOB");
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/index",
                IsPersistent = true,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            }, "Steam");
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
