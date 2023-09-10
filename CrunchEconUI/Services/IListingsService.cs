﻿using CrunchEconModels.Models;
using CrunchEconUI.Models;

namespace CrunchEconUI.Services
{
    public interface IListingsService
    {
       Task<List<ItemListing>> GetListings();
       Task<bool> IsSuspended(Guid itemId);
       Task<ItemListing> GetUpdatedItem(Guid itemId);
       Task<List<ItemListing>> GetUsersOwnListings(ulong steamId);

        Task CreateListingRequest(ulong steamId, ItemListing listing);
        Task ConfirmListingRequest(ulong steamId, ItemListing listing);
        Task RemoveListingRequest(ulong steamId, ItemListing listing);
        Task SuspendListing(ItemListing item);
        Action<ItemListing>? RefreshListings { get; set; }
    }
}
