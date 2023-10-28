using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrunchEconModels.Models
{
    [Table("playeritemlistings")]
    public class ItemListing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } = Guid.NewGuid();  
        public string ItemId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ItemIdImage { get; set; } = "";
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SellPricePerItem { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long BuyPricePerItem { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Amount { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaxAmountToBuy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public bool IsSelling { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public bool IsBuying { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public long OwnerSteam { get; set; } 
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public bool IsAdminListing { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public bool Suspended { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public bool Deleted { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong BuyerId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DateTime? SuspendedUntil { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid? EventId { get; set; }

    }
}
