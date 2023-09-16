﻿using System;
namespace CrunchEconModels.Models
{
    public class ItemListing
    {
        public Guid ListingId { get; set; }
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
    }
}
