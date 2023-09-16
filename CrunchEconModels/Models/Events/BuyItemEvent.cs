using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrunchEconModels.Models.Events
{
    public class BuyItemEvent
    {
        public ulong OriginatingPlayerSteamId { get; set; }
        public bool IsAdminSale { get; set; } = false;
        public ulong SellerSteamId { get; set; }
        public ulong BuyerSteamId { get; set; }
        public long Price { get; set; }
        public int Amount { get; set; }
        public string DefinitionIdString { get; set; }
        public DateTime RaisedAt { get; set; }
        public Guid ListedItemId { get; set; }
        public EventResult Result { get; set; }
    }
}
