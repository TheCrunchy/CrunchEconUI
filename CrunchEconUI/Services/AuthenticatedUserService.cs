using CrunchEconUI.Interfaces;
using CrunchEconUI.Models;

namespace CrunchEconUI.Services
{
    public class AuthenticatedUserService
    {
        private readonly HttpClient httpClient;
      
        public AuthenticatedUserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public UserInfo UserInfo { get; set; }

        public async Task InitializeAsync()
        {
            await UpdateUserInfoAsync();
        }

        private async Task UpdateUserInfoAsync()
        {
            HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7116/api/login/me");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                UserInfo = await response.Content.ReadFromJsonAsync<UserInfo>();
            }
        }

        public bool IsAuthenticated => UserInfo != null;
        public int UserId => UserInfo?.Id ?? 0;
        public bool IsAdmin => UserInfo?.Role?.Equals(RoleConstants.AdminRoleId) ?? false;
    }
}
