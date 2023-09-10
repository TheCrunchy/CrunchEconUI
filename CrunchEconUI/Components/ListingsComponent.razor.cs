using Blazorise.DataGrid;
using CrunchEconModels.Models;
using CrunchEconUI.Models;
using CrunchEconUI.Services;
using Microsoft.AspNetCore.Components;

namespace CrunchEconUI.Components
{
    public partial class ListingsComponent : IAsyncDisposable
    {
        [Inject] IListingsService listingService { get; set; }
        List<ItemListing> Items = new();
        public DataGrid<ItemListing> GridRef { get; set; }
        [Parameter]
        public UserInfo? User { get; set; }
        protected override async Task OnInitializedAsync()
        {
            listingService.RefreshListings += Reload;
            Items = await listingService.GetListings();
        }

        public async void Reload(ItemListing item)
        {
            Items = Items.Where(x => x.ListingId != item.ListingId).ToList();
            await GridRef?.Reload();
            await GridRef?.Refresh();
            
            await InvokeAsync(StateHasChanged);
        }

        public async ValueTask DisposeAsync()
        {
            listingService.RefreshListings -= Reload;
        }
    }
}
