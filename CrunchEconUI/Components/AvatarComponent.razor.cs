using CrunchEconModels.Models;
using CrunchEconUI.Models;
using CrunchEconUI.Services;
using Microsoft.AspNetCore.Components;

namespace CrunchEconUI.Components
{
    public partial class AvatarComponent : IAsyncDisposable
    {
        [Parameter]
        public UserInfo? User { get; set; }
        [Inject]
        public PlayerBalanceService BalanceService { get; set; }

        public async ValueTask DisposeAsync()
        {
            BalanceService.RefreshListings -= RefreshBalance;
        }

        protected override async Task OnInitializedAsync()
        {
            BalanceService.RefreshListings += RefreshBalance;
        }

        public async void RefreshBalance(ulong steamId)
        {
            if (User?.SteamId == steamId) {
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
