using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrunchEconModels.Models.Events
{
    public class BalanceUpdateEvent
    {
        public ulong OriginatingPlayerSteamId { get; set; }
        public long Balance { get; set; }
    }
}
