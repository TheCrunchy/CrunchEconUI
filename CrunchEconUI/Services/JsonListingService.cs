using CrunchEconModels.Models;
using CrunchEconModels.Models.Events;
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
        public Action<ShipListing>? RefreshShipListings { get; set; }
        private EventService events { get; set; }
        private EconContext context { get; set; }

        private List<string> Prefabs { get; set; } = new();

        public List<String> GetPrefabs()
        {
            return new List<string>()
            {
                "Ship 1",
                "Ship 2",
                "Ship 3"
            };
            return Prefabs;
        }
        public void SetPrefabs(List<String> prefabs)
        {
            Prefabs = prefabs;
        }

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

        public async Task<List<ShipListing>> GetShipListings()
        {
            return context.shiplistings.Where(x => !x.Deleted).ToList();
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

            return ListedItems.ToList().Select(x => x.Value).ToList();
        }

        public async Task ArchiveEvent(Event ev)
        {
            context.ArchivedEvents.Add(ev);
            await context.SaveChangesAsync();
        }

        public async Task StoreShip(ShipListing listing)
        {
            context.shiplistings.Add(listing);
            context.SaveChanges();
            RefreshShipListings?.Invoke(listing);
        }

        public async Task DeleteShip(ShipListing listing)
        {
            listing.Deleted = true;
            context.shiplistings.Update(listing);
            context.SaveChanges();
            RefreshShipListings?.Invoke(listing);
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
