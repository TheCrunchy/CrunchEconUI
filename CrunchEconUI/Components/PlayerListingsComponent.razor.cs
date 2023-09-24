using Blazorise.DataGrid;
using CrunchEconModels.Models;
using CrunchEconUI.Models;
using CrunchEconUI.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;

namespace CrunchEconUI.Components
{
    public partial class PlayerListingsComponent : IAsyncDisposable
    {
        [Inject]
        public EventService eventService { get; set; }
        [Inject]
         DialogService DialogService { get; set; }
        [Inject] IListingsService listingService { get; set; }
        List<ItemListing> Items = new();
        public DataGrid<ItemListing> GridRef { get; set; }
        [Parameter]
        public UserInfo? User { get; set; }
        private string Bound = "Sell Price Low to High";

        IEnumerable<string> Sorting = new List<string>()
        {
            "Sell Price Low to High",
            "Sell Price High to Low",
            "Buy Price Low to High",
            "Buy Price High to Low",
            "Amount Low to High",
            "Amount High to Low",
        };

        public async Task Changed()
        {
            switch (Bound)
            {
                case "Sell Price Low to High":
                    Items = Items.OrderBy(x => x.SellPricePerItem).ToList();
                    break;
                case "Sell Price High to Low":
                    Items = Items.OrderByDescending(x => x.SellPricePerItem).ToList();
                    break;
                case "Buy Price Low to High":
                    Items = Items.OrderBy(x => x.BuyPricePerItem).ToList();
                    break;
                case "Buy Price High to Low":
                    Items = Items.OrderByDescending(x => x.BuyPricePerItem).ToList();
                    break;
                case "Amount Low to High":
                    Items = Items.OrderBy(x => x.Amount).ToList();
                    break;
                case "Amount High to Low":
                    Items = Items.OrderByDescending(x => x.Amount).ToList();
                    break;
            }
            await GridRef?.Reload();
            await GridRef?.Refresh();

            await InvokeAsync(StateHasChanged);
            return;
        }

        protected override async Task OnInitializedAsync()
        {
            listingService.RefreshListings += Reload;
            Items = await listingService.GetListings();
            Items = Items.Where(x => !x.Suspended).OrderBy(x => x.SellPricePerItem).ToList();
            return;
        }

        public async void Reload(ItemListing item)
        {
            if (item.Suspended || item.Deleted)
            {
                Items = Items.Where(x => x.Id != item.Id).ToList();
            }
            else {
                Items.Add(item);
                Items = Items.Where(x => !x.Suspended).ToList();
            }

            await Changed();
        }

        public async Task CreateListing()
        {
            var ListedItem = new ItemListing();
            ListedItem.OwnerId = User.SteamId;
          
                await DialogService.OpenAsync<NewListingComponent>($"New Listing",
                       new Dictionary<string, object>() { { "User", User } },
                       new DialogOptions() { Width = "700px", Height = "512px", Resizable = true, Draggable = true });
            
        }

        public async ValueTask DisposeAsync()
        {
            listingService.RefreshListings -= Reload;
        }
    }
}
