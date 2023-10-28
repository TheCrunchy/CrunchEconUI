using Blazorise.DataGrid;
using CrunchEconModels.Models;
using CrunchEconUI.Models;
using CrunchEconUI.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;

namespace CrunchEconUI.Components.ShipSales
{
    public partial class ShipListingsComponent : IAsyncDisposable
    {
        [Inject]
        public EventService eventService { get; set; }
        [Inject]
        DialogService DialogService { get; set; }
        [Inject] IListingService listingService { get; set; }
        List<ShipListing> Items = new();
        public DataGrid<ShipListing> GridRef { get; set; }
        [CascadingParameter]
        public AuthenticatedUserService User { get; set; }

        [Inject]
        private ILogger<PlayerListingsComponent> _Logger { get; set; }

        private string Bound = "Price Low to High";

        IEnumerable<string> Sorting = new List<string>()
        {
            "Price Low to High",
            "Price High to Low",
        };

        public async Task Changed()
        {
            Items = await listingService.GetShipListings();

            switch (Bound)
            {
                case "Price Low to High":
                    Items = Items.OrderBy(x => x.Price).ToList();
                    break;
                case "Price High to Low":
                    Items = Items.OrderByDescending(x => x.Price).ToList();
                    break;
            }
            if (GridRef != null)
            {
                await GridRef?.Reload();
                await GridRef?.Refresh();
            }

            await InvokeAsync(StateHasChanged);
            return;
        }

        protected override async Task OnInitializedAsync()
        {
            listingService.RefreshShipListings += Reload;
            Items = await listingService.GetShipListings();
            Items = Items.OrderBy(x => x.Price).ToList();
            return;
        }

        public async void Reload(ShipListing item)
        {

            await Changed();
        }

        public async Task CreateListing()
        {
            var ListedItem = new ItemListing();
            ListedItem.OwnerSteam = (long)User.UserInfo.SteamId;

            await DialogService.OpenAsync<NewShipListingComponent>($"New Listing",
                   new Dictionary<string, object>() { { "User", User } },
                   new DialogOptions() { Width = "90%", Height = "90%", Resizable = true, Draggable = true });
        }

        public async ValueTask DisposeAsync()
        {
            listingService.RefreshShipListings -= Reload;
        }
    }
}
