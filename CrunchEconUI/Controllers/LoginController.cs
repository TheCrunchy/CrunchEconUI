using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using CrunchEconUI.Models;

namespace CrunchEconUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpGet("me")]
        public async Task<IActionResult> GetMeUserAsync()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return null;
                //
              //  return Ok(await usersRepository.GetUserAsync<UserInfo>(int.Parse(User.Identity.Name)));
           }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
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
