using CrunchEconModels.Models;
using CrunchEconUI.Models;
using CrunchEconUI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
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
        public UserInfo? User { get; set; }

        private List<String> definitionIds = new List<String>();

        [Inject]
        public EventService eventService { get; set; }

        private ItemListing ListedItem = new ItemListing();

        protected override async Task OnInitializedAsync()
        {
            definitionIds = eventService.GetAllIds().OrderByDescending(x => x).ToList();
            ListedItem.OwnerId = User.SteamId;
            ListedItem.Suspended = true;
            ListedItem.ItemDefinition = new();
            return;
        }

        public async Task Submit()
        {

        }

        public async Task OnChange(string search)
        {
            definitionIds = eventService.GetAllIds().Where(x => x.ToLower().Contains(search.ToLower())).OrderByDescending(x => x).ToList();
        }
    }
}
