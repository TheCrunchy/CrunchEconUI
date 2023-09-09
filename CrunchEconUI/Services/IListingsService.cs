using CrunchEconUI.Models;

namespace CrunchEconUI.Services
{
    public interface IListingsService
    {
       Task<List<ItemListing>> GetListings();
       Task<List<ItemListing>> GetUsersOwnListings(ulong steamId);

        Task CreateListingRequest(ulong steamId, ItemListing listing);
        Task ConfirmListingRequest(ulong steamId, ItemListing listing);
        Task RemoveListingRequest(ulong steamId, ItemListing listing);
    }
}
