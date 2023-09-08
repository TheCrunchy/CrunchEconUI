using CrunchEconUI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using static System.Net.Mime.MediaTypeNames;
using Steam.Models.SteamCommunity;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;
using System.Security.Claims;
using CrunchEconUI.Services;
using System.Runtime.CompilerServices;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace CrunchEconUI.Helpers
{
    public class ValidationHelper
    {
        private const int SteamIdStartIndex = 37;

        public static async Task SignIn(CookieSignedInContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            string steamId = context.Principal.FindFirst(ClaimTypes.NameIdentifier).Value[SteamIdStartIndex..];
            IHttpClientFactory httpClientFactory = context.HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>();
            SteamWebInterfaceFactory steamFactory = context.HttpContext.RequestServices.GetRequiredService<SteamWebInterfaceFactory>();
            HttpClient httpClient = httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(3);
            ILogger<ValidationHelper> logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ValidationHelper>>();
            var auth = context.HttpContext.RequestServices.GetRequiredService<AuthenticatedUserService>();

            PlayerSummaryModel playerSummary = null;
            try
            {
                ISteamWebResponse<PlayerSummaryModel> steamWebResponse = await steamFactory.CreateSteamWebInterface<SteamUser>(httpClient).GetPlayerSummaryAsync(ulong.Parse(steamId));
                playerSummary = steamWebResponse.Data;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An exception occurated when downloading player summaries");
            }
            auth.UserInfo = new UserInfo()
            {
                SteamId = playerSummary.SteamId.ToString(),
                Name = playerSummary.Nickname,
                Role = RoleConstants.DefaultRoleId,
                AvatarUrl = playerSummary.AvatarFullUrl,

            };

            return;
        }

        public static async Task Validate(CookieValidatePrincipalContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            string steamId = context.Principal.FindFirst(ClaimTypes.NameIdentifier).Value[SteamIdStartIndex..];
            IHttpClientFactory httpClientFactory = context.HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>();
            SteamWebInterfaceFactory steamFactory = context.HttpContext.RequestServices.GetRequiredService<SteamWebInterfaceFactory>();
            HttpClient httpClient = httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(3);
            ILogger<ValidationHelper> logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ValidationHelper>>();
            var auth = context.HttpContext.RequestServices.GetRequiredService<AuthenticatedUserService>();

            PlayerSummaryModel playerSummary = null;
            try
            {
                ISteamWebResponse<PlayerSummaryModel> steamWebResponse = await steamFactory.CreateSteamWebInterface<SteamUser>(httpClient).GetPlayerSummaryAsync(ulong.Parse(steamId));
                playerSummary = steamWebResponse.Data;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An exception occurated when downloading player summaries");
            }
            auth.UserInfo = new UserInfo()
            {
                SteamId = playerSummary.SteamId.ToString(),
                Name = playerSummary.Nickname,
                Role = RoleConstants.DefaultRoleId,
                AvatarUrl = playerSummary.AvatarFullUrl,
            };

            return;
        }
    }
}
