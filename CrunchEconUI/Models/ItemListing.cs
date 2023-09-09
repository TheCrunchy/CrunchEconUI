namespace CrunchEconUI.Models
{
    public class ItemListing
    {
        public string ItemId { get; set; }
        public long SellPricePerItem { get; set; }
        public long BuyPricePerItem { get; set; }
        public long Amount { get; set; }
        public long MaxAmountToBuy { get; set; }
        public bool IsSelling { get; set; }
        public bool IsBuying { get; set; }
        public ulong OwnerId { get; set; }
    }
}
