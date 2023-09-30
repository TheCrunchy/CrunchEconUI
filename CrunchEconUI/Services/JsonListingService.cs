﻿using CrunchEconModels.Models;
using CrunchEconUI.EntityFramework;
using CrunchEconUI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SteamWebAPI2.Models;
using System.Collections.Generic;
using System.Reflection;

namespace CrunchEconUI.Services
{
    public class IListingService
    {
        private Dictionary<Guid, ItemListing> ListedItems = new Dictionary<Guid, ItemListing>();

        public Action<ItemListing>? RefreshListings { get; set; }
        private EventService events { get; set; }
        private EconContext context { get; set; }

        public IListingService(EventService events, EconContext factory)
        {

            this.events = events;
            context = factory;

            context.Database.EnsureCreated();

            context.SaveChanges();

            foreach (var item in context.playeritemlistings)
            {
                ListedItems.TryAdd(item.Id, item);
            }
        }

        public Task ConfirmListingRequest(ulong steamId, ItemListing listing)
        {
            throw new NotImplementedException();
        }

        public async Task ModifySuspended(ItemListing item, bool suspended = true)
        {
            if (ListedItems.TryGetValue(item.Id, out var listed))
            {
                listed.Suspended = suspended;
                if (suspended)
                {
                    listed.SuspendedUntil = DateTime.Now.AddMinutes(0.1);
                }

                ListedItems[item.Id] = listed;
                context.playeritemlistings.Update(listed);
                context.SaveChanges();
                RefreshListings?.Invoke(listed);
            }
        }
        public async Task DeleteListing(ItemListing item)
        {
            if (ListedItems.TryGetValue(item.Id, out var listed))
            {
                listed.Deleted = true;
                ListedItems.Remove(listed.Id);
                context.playeritemlistings.Remove(listed);
                context.SaveChanges();
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
                var deleteThese = new List<ItemListing>();
                foreach (var item in ListedItems.Where(x => x.Value.SuspendedUntil.HasValue))
                {
                    if (DateTime.Now >= item.Value.SuspendedUntil)
                    {
                        deleteThese.Add(item.Value);

                    }
                }
                foreach (var item in deleteThese)
                {
                    if (item.EventId.HasValue)
                    {
                        events.RemoveEvent(0l, item.EventId.Value);
                    }

                    item.Suspended = false;
                    item.EventId = null;
                    item.SuspendedUntil = null;
                    StoreItem(item);
                    ModifySuspended(item, false);
                }
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
            //    var listing = new ItemListing()
            //    {
            //        ItemId = "MyObjectBuilder_Ingot/Iron",
            //        BuyPricePerItem = 50,
            //        SellPricePerItem = 75,
            //        Id = Id,
            //        IsBuying = false,
            //        IsSelling = true,
            //        Amount = 50,
            //        MaxAmountToBuy = 100,
            //        OwnerId = 76561198045390854,
            //        Suspended = false
            //    };
            //    ListedItems.Add(Id, listing);
            //    context.playeritemlistings.Add(listing);
            //}


            // context.SaveChanges();
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
                context.playeritemlistings.Update(listing);
                return;
            }

            ListedItems.Add(listing.Id, listing);

            context.playeritemlistings.Add(listing);
            context.SaveChanges();
            RefreshListings?.Invoke(listing);
        }
    }
}
