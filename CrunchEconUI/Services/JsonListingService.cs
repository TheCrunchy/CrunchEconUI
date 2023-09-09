using CrunchEconUI.Models;

namespace CrunchEconUI.Services
{
    public class JsonListingService : IListingsService
    {
        public Task ConfirmListingRequest(ulong steamId, ItemListing listing)
        {
            throw new NotImplementedException();
        }

        public Task CreateListingRequest(ulong steamId, ItemListing listing)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ItemListing>> GetListings()
        {
            var items = new List<ItemListing>();
            items.Add(new ItemListing()
            {
                ItemId = "ExampleId",
                BuyPricePerItem = 50,
                SellPricePerItem = 75,
                ListingId = Guid.NewGuid(),
                IsBuying = true,
                IsSelling = true,
                Amount = 50,
                MaxAmountToBuy = 100,
                OwnerId = 76561198045390854
            });
            items.Add(new ItemListing()
            {
                ItemId = "ExampleId2",
                BuyPricePerItem = 50,
                SellPricePerItem = 75,
                ListingId = Guid.NewGuid(),
                IsBuying = true,
                IsSelling = true,
                Amount = 50,
                MaxAmountToBuy = 100,
                OwnerId = 76561198045390854
            });
            items.Add(new ItemListing()
            {
                ItemId = "ExampleId3",
                BuyPricePerItem = 50,
                SellPricePerItem = 75,
                ListingId = Guid.NewGuid(),
                IsBuying = true,
                IsSelling = true,
                Amount = 50,
                MaxAmountToBuy = 100,
                OwnerId = 76561198045390854
            });
            return items;
        }

        public Task<List<ItemListing>> GetUsersOwnListings(ulong steamId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveListingRequest(ulong steamId, ItemListing listing)
        {
            throw new NotImplementedException();
        }
    }
}
