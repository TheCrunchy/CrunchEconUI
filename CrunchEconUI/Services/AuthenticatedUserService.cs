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

        public bool IsAuthenticated => UserInfo != null;
        public int UserId => UserInfo?.Id ?? 0;
        public bool IsAdmin => UserInfo?.Role?.Equals(RoleConstants.AdminRoleId) ?? false;
    }
}
