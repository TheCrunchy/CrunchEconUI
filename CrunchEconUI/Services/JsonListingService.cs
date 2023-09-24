using CrunchEconModels.Models;
using CrunchEconUI.Models;
using SteamWebAPI2.Models;
using System.Collections.Generic;

namespace CrunchEconUI.Services
{
    public class IListingService : IListingsService
    {
        private Dictionary<Guid, ItemListing> ListedItems = new Dictionary<Guid, ItemListing>();

        public Action<ItemListing>? RefreshListings { get; set; }

        public Task ConfirmListingRequest(ulong steamId, ItemListing listing)
        {
            throw new NotImplementedException();
        }

        public async Task ModifySuspended(ItemListing item, bool suspended = true)
        {
            if (ListedItems.TryGetValue(item.Id, out var listed))
            {
                listed.Suspended = suspended;
                ListedItems[item.Id] = listed;

                RefreshListings?.Invoke(listed);
            }
        }
        public async Task DeleteListing(ItemListing item)
        {
            if (ListedItems.TryGetValue(item.Id, out var listed))
            {
                listed.Deleted = true;
                ListedItems.Remove(listed.Id);

                RefreshListings?.Invoke(listed);
            }
        }
        public async Task<bool> IsSuspended(Guid itemId)
        {
            if (ListedItems.ContainsKey(itemId))
            {
                return ListedItems[itemId].Suspended;
            }
            return true;
        }
        public Task CreateListingRequest(ulong steamId, ItemListing listing)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ItemListing>> GetListings()
        {
            if (ListedItems.Any())
            {
                return ListedItems.ToList().Select(x => x.Value).ToList();
            }
            //ListedItems.Clear();
            //var Id1 = Guid.NewGuid();
            //var Id2 = Guid.NewGuid();
            //var Id3 = Guid.NewGuid();
            //ListedItems.Add(Id1, new ItemListing()
            //{
            //    ItemId = "MyObjectBuilder_Ingot/Iron",
            //    BuyPricePerItem = 50,
            //    SellPricePerItem = 5,
            //    Id = Id1,
            //    IsBuying = true,
            //    IsSelling = true,
            //    Amount = 50,
            //    MaxAmountToBuy = 100,
            //    OwnerId = 76561198045390854,
            //    Suspended = false,
            //});
            //ListedItems.Add(Id2, new ItemListing()
            //{
            //    ItemId = "MyObjectBuilder_Component/PlasmaCredit",
            //    BuyPricePerItem = 55,
            //    SellPricePerItem = 7,
            //    Id = Id2,
            //    IsBuying = true,
            //    IsSelling = true,
            //    Amount = 50,
            //    MaxAmountToBuy = 100,
            //    OwnerId = 76561198045390854,
            //    Suspended = false,
            //});
            //ListedItems.Add(Id3, new ItemListing()
            //{
            //    ItemId = "MyObjectBuilder_Ingot/Gold",
            //    BuyPricePerItem = 500,
            //    SellPricePerItem = 3,
            //    Id = Id3,
            //    IsBuying = false,
            //    IsSelling = true,
            //    Amount = 50,
            //    MaxAmountToBuy = 100,
            //    OwnerId = 76561198045390854,
            //    Suspended = false
            //});
            //for (int i = 0; i < 50; i++)
            //{
            //    Guid Id = Guid.NewGuid();
            //    ListedItems.Add(Id, new ItemListing()
            //    {
            //        ItemId = "ExampleId3",
            //        BuyPricePerItem = 50,
            //        SellPricePerItem = 75,
            //        Id = Id,
            //        IsBuying = false,
            //        IsSelling = true,
            //        Amount = 50,
            //        MaxAmountToBuy = 100,
            //        OwnerId = 76561198045390854,
            //        Suspended = false
            //    });
            //}
            return ListedItems.ToList().Select(x => x.Value).ToList();
        }

        public Task<List<ItemListing>> GetUsersOwnListings(ulong steamId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveListingRequest(ulong steamId, ItemListing listing)
        {
            throw new NotImplementedException();
        }

        public async Task<ItemListing> GetUpdatedItem(Guid itemId)
        {
            if (ListedItems.ContainsKey(itemId))
            {
                return ListedItems[itemId];
            }
            return new ItemListing() { Suspended = true };
        }

        public async Task StoreItem(ItemListing listing)
        {
            if (ListedItems.ContainsKey(listing.Id))
            {
                ListedItems[listing.Id] = listing;
                return;
            }
            ListedItems.Add(listing.Id, listing);

            RefreshListings?.Invoke(listing);
        }
    }
}
