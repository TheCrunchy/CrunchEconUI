using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrunchEconModels.Models.Events
{
    public class BuyShipEvent
    {
        public ulong OriginatingPlayerSteamId { get; set; }
        public ShipListing ShipListing { get; set; }
    }
}
