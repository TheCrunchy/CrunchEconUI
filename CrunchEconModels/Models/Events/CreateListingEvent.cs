using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrunchEconModels.Models.Events
{
    public class CreateListingEvent
    {
        public ItemListing Listing { get; set; }
        public EventResult Result { get; set; }
        public ulong OriginatingPlayerSteamId { get; set; }
    }
}
