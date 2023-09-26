using CrunchEconModels.Models;
using CrunchEconUI.Models;
using CrunchEconUI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
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
        protected override async Task OnInitializedAsync()
        {
            definitionIds = eventService.GetAllIds().OrderByDescending(x => x).ToList();
            ListedItem.OwnerId = User.UserInfo.SteamId;
            ListedItem.Suspended = true;
            ListedItem.SellPricePerItem = 1;
            ListedItem.BuyPricePerItem = 1;
            return;
        }

        public async Task Submit()
        {
            DialogService.Close();
        }

        public async Task OnChange(string search)
        {
            definitionIds = eventService.GetAllIds().Where(x => x.ToLower().Contains(search.ToLower())).OrderByDescending(x => x).ToList();
        }
    }
}
