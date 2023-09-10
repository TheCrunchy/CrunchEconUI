using CrunchEconModels.Models;
using CrunchEconUI.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace CrunchEconUI.Services
{
    public class SteamAuthProvider : AuthenticationStateProvider
    {
        private readonly AuthenticatedUserService userService;

        public SteamAuthProvider(AuthenticatedUserService userService)
        {
            this.userService = userService;
        }

        private static IEnumerable<Claim> GetClaims(UserInfo userInfo)
        {
            return new[]
            {
                new Claim(ClaimTypes.Name, userInfo.Name),
                new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
                new Claim("SteamId", userInfo.SteamId.ToString()),
                new Claim(ClaimTypes.Role, userInfo.Role)
            };
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity;
            if (userService.IsAuthenticated)
            {
                IEnumerable<Claim> claims = GetClaims(userService.UserInfo);
                identity = new(claims, "Steam");
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            ClaimsPrincipal user = new(identity);
            AuthenticationState authenticationState = new(user);
            return Task.FromResult(authenticationState);
        }
    }
}
