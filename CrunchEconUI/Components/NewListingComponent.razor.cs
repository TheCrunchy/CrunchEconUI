using CrunchEconModels.Models;
using CrunchEconModels.Models.Events;
using CrunchEconUI.Models;
using CrunchEconUI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Newtonsoft.Json;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace CrunchEconUI.Components
{
    public partial class NewListingComponent
    {
        [Parameter]
        public AuthenticatedUserService User { get; set; }
        [Inject]
        DialogService DialogService { get; set; }
        private List<String> definitionIds = new List<String>();

        [Inject]
        public EventService eventService { get; set; }

        private ItemListing ListedItem = new ItemListing();
        Variant variant = Variant.Filled;

        [Inject]
        private IListingService service { get; set; }
        protected override async Task OnInitializedAsync()
        {
            definitionIds = eventService.GetAllIds().OrderByDescending(x => x).ToList();
            ListedItem.OwnerSteam = (long)User.UserInfo.SteamId;
            ListedItem.Suspended = true;
            ListedItem.SellPricePerItem = 1;
            ListedItem.BuyPricePerItem = 1;
            ListedItem.Id = Guid.NewGuid();
            return;
        }

        public async Task Submit()
        {
            if (string.IsNullOrWhiteSpace(ListedItem.ItemId))
            {
                await DialogService.Alert($"You must select an item.", "Error");
                return;

            }
            ListedItem.IsSelling = ListedItem.Amount > 0;
            ListedItem.IsBuying = ListedItem.MaxAmountToBuy > 0;
            if (!ListedItem.IsBuying && !ListedItem.IsSelling)
            {
                await DialogService.Alert($"You must buy or sell at least 1 item.", "Error");
                return;
            }
            if (ListedItem.IsAdminListing)
            {
                await service.StoreItem(ListedItem);
                await service.ModifySuspended(ListedItem, false);
                DialogService.Close(); 
                return;
            }

            var final = new Event();
            var create = new CreateListingEvent();
            create.Listing = ListedItem;
            create.OriginatingPlayerSteamId = (ulong)ListedItem.OwnerSteam;
            final.EventType = EventType.ListItem;
            final.JsonEvent = JsonConvert.SerializeObject(create);

            await eventService.AddEvent(User.UserInfo.SteamId, final);

            DialogService.Close();
        }

        public async Task OnChange(string search)
        {
            definitionIds = eventService.GetAllIds().Where(x => x.ToLower().Contains(search.ToLower())).OrderByDescending(x => x).ToList();
        }
    }
}
