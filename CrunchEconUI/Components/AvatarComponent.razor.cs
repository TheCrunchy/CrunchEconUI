using CrunchEconModels.Models;
using CrunchEconUI.Models;
using CrunchEconUI.Services;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace CrunchEconUI.Components
{
    public partial class AvatarComponent : IAsyncDisposable
    {
        [CascadingParameter]
        public AuthenticatedUserService User { get; set; }
        [Inject]
        public PlayerBalanceAndNotifyService BalanceService { get; set; }

        public async ValueTask DisposeAsync()
        {
            BalanceService.RefreshListings -= RefreshBalance;
            BalanceService.SendNotification -= Notify;
        }

        protected override async Task OnInitializedAsync()
        {
            BalanceService.RefreshListings += RefreshBalance;
            BalanceService.SendNotification += Notify;
        }

        public async void RefreshBalance(ulong steamId)
        {
            if (User.UserInfo?.SteamId == steamId) {
                await InvokeAsync(StateHasChanged);
            }
        }

        public async void Notify(ulong steamId, string notification)
        {
            if (User.UserInfo?.SteamId == steamId)
            {
                await DialogService.Alert($"{notification}", "Notification", new ConfirmOptions() { OkButtonText = $"Close"});
            }
        }
    }
}
