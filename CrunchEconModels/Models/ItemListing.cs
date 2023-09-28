using System;
namespace CrunchEconModels.Models
{
    public class ItemListing
    {
        public Guid Id { get; set; } = Guid.NewGuid();  
        public string ItemId { get; set; }
        public string ItemIdImage { get; set; }
        public long SellPricePerItem { get; set; }
        public long BuyPricePerItem { get; set; }
        public int Amount { get; set; }
        public int MaxAmountToBuy { get; set; }
        public bool IsSelling { get; set; }
        public bool IsBuying { get; set; }
        public ulong OwnerId { get; set; }
        public bool IsAdminListing { get; set; }
        public bool Suspended { get; set; }
        public bool Deleted { get; set; }
        public ulong BuyerId { get; set; }
        public DateTime? SuspendedUntil { get; set; }
        public Guid? EventId { get; set; }

    }
}
