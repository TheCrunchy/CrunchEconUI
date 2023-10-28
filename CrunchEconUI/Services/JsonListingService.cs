using CrunchEconModels.Models;
using CrunchEconModels.Models.Events;
using CrunchEconUI.EntityFramework;
using CrunchEconUI.Models;
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
            return Prefabs;
        }
        public void SetPrefabs(List<String> prefabs)
        {
            Prefabs = prefabs;
        }

        public IListingService(EventService events)
        {

            this.events = events;
            context = new EconContext(Program.DBString);
            context.SaveChanges();
        }

        public async Task ModifySuspended(ItemListing item, bool suspended = true)
        {
            if (context.playeritemlistings.FirstOrDefault(x => x.Id == item.Id) != null)
            {
                var listed = context.playeritemlistings.FirstOrDefault(x => x.Id == item.Id);
                listed.Suspended = suspended;
                if (suspended)
                {
                    listed.SuspendedUntil = DateTime.Now.AddMinutes(0.1);
                }

                ListedItems[item.Id] = listed;
              
                context.SaveChanges();
                RefreshListings?.Invoke(listed);
            }
        }
        public async Task DeleteListing(ItemListing item)
        {
            if (context.playeritemlistings.FirstOrDefault(x => x.Id == item.Id) != null)
            {
                var listed = context.playeritemlistings.FirstOrDefault(x => x.Id == item.Id);
                listed.Deleted = true;
                ListedItems.Remove(listed.Id);
                context.playeritemlistings.Remove(listed);
                context.SaveChanges();
                RefreshListings?.Invoke(listed);
            }
        }
        public async Task<bool> IsSuspended(Guid itemId)
        {
            if (context.playeritemlistings.FirstOrDefault(x => x.Id == itemId) != null)
            {
                var listed = context.playeritemlistings.FirstOrDefault(x => x.Id == itemId);
                return ListedItems[itemId].Suspended;
            }
            return true;
        }

        public async Task<List<ShipListing>> GetShipListings()
        {
            var data = context.shiplistings.Where(x => !x.Deleted).ToList();
            return data;
        }

            public async Task<List<ItemListing>> GetListings()
        {
            if (context.playeritemlistings.Any())
            {
                var deleteThese = new List<ItemListing>();
                foreach (var item in context.playeritemlistings.Where(x => x.SuspendedUntil.HasValue))
                {
                    if (DateTime.Now >= item.SuspendedUntil)
                    {
                        deleteThese.Add(item);

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
                    await StoreItem(item);
                    await ModifySuspended(item, false);
                }
                return context.playeritemlistings.ToList();
            }

            return context.playeritemlistings.ToList();
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
            context.SaveChanges();
            RefreshShipListings?.Invoke(listing);
        }

        public async Task<ItemListing> GetUpdatedItem(Guid itemId)
        {
            if (context.playeritemlistings.FirstOrDefault(x => x.Id == itemId) != null)
            {
                var listed = context.playeritemlistings.FirstOrDefault(x => x.Id == itemId);
                return ListedItems[itemId];
            }
            return new ItemListing() { Suspended = true };
        }

        public async Task StoreItem(ItemListing listing)
        {

            if (context.playeritemlistings.FirstOrDefault(x => x.Id == listing.Id) != null)
            {
                var listed = context.playeritemlistings.FirstOrDefault(x => x.Id == listing.Id);
                ListedItems[listing.Id] = listing;
                context.SaveChanges();
                return;
            }

            context.playeritemlistings.Add(listing);
            context.SaveChanges();
            RefreshListings?.Invoke(listing);
        }
    }
}
