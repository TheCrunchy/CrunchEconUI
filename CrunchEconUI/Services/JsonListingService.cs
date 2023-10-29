using CrunchEconModels.Models;
using CrunchEconModels.Models.Events;
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
        }

        public async Task ModifySuspended(ItemListing item, bool suspended = true)
        {
            if (DBService.Context.playeritemlistings.FirstOrDefault(x => x.Id == item.Id) != null)
            {
                var listed = DBService.Context.playeritemlistings.FirstOrDefault(x => x.Id == item.Id);
                listed.Suspended = suspended;
                if (suspended)
                {
                    listed.SuspendedUntil = DateTime.Now.AddMinutes(0.1);
                }

                await DBService.Context.SaveChangesAsync();
                RefreshListings?.Invoke(listed);
            }
        }
        public async Task DeleteListing(ItemListing item)
        {
            if (DBService.Context.playeritemlistings.FirstOrDefault(x => x.Id == item.Id) != null)
            {
                var listed = DBService.Context.playeritemlistings.FirstOrDefault(x => x.Id == item.Id);
                listed.Deleted = true;
                ListedItems.Remove(listed.Id);
                DBService.Context.playeritemlistings.Remove(listed);
                await DBService.Context.SaveChangesAsync();
                RefreshListings?.Invoke(listed);
            }
        }
        public async Task<bool> IsSuspended(Guid itemId)
        {
            if (DBService.Context.playeritemlistings.FirstOrDefault(x => x.Id == itemId) != null)
            {
                var listed = DBService.Context.playeritemlistings.FirstOrDefault(x => x.Id == itemId);
                return listed.Suspended;
            }
            return true;
        }

        public async Task<List<ShipListing>> GetShipListings()
        {
            var data = DBService.Context.shiplistings.Where(x => !x.Deleted).ToList();
            return data;
        }

            public async Task<List<ItemListing>> GetListings()
        {
            if (DBService.Context.playeritemlistings.Any())
            {
                var deleteThese = new List<ItemListing>();
                foreach (var item in DBService.Context.playeritemlistings.Where(x => x.SuspendedUntil.HasValue))
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
                        var tevent = DBService.Context.ArchivedEvents.First(x => x.Id == item.EventId);
                        DBService.Context.ArchivedEvents.Remove(tevent);
                    }

                    item.Suspended = false;
                    item.EventId = null;
                    item.SuspendedUntil = null;
                    await StoreItem(item);
                    await ModifySuspended(item, false);
                }
                await DBService.Context.SaveChangesAsync();
                return DBService.Context.playeritemlistings.ToList();
            }
            await DBService.Context.SaveChangesAsync();
            return DBService.Context.playeritemlistings.ToList();
        }

        public async Task StoreShip(ShipListing listing)
        {
            DBService.Context.shiplistings.Add(listing);
            await DBService.Context.SaveChangesAsync();
            RefreshShipListings?.Invoke(listing);
        }

        public async Task DeleteShip(ShipListing listing)
        {
            listing.Deleted = true;
            await DBService.Context.SaveChangesAsync();
            RefreshShipListings?.Invoke(listing);
        }

        public async Task<ItemListing> GetUpdatedItem(Guid itemId)
        {
            if (DBService.Context.playeritemlistings.FirstOrDefault(x => x.Id == itemId) != null)
            {
                var listed = DBService.Context.playeritemlistings.FirstOrDefault(x => x.Id == itemId);
                return listed;
            }
            return new ItemListing() { Suspended = true };
        }

        public async Task StoreItem(ItemListing listing)
        {

            if (DBService.Context.playeritemlistings.FirstOrDefault(x => x.Id == listing.Id) != null)
            {
                var listed = DBService.Context.playeritemlistings.FirstOrDefault(x => x.Id == listing.Id);
                ListedItems[listing.Id] = listing;
                await DBService.Context.SaveChangesAsync();
                return;
            }

            DBService.Context.playeritemlistings.Add(listing);
            await DBService.Context.SaveChangesAsync();
            RefreshListings?.Invoke(listing);
        }
    }
}
