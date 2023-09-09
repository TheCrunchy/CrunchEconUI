using System.Numerics;

namespace CrunchEconUI.Models
{
    public class UserSession
    {
        public ulong SteamId { get; set; }
        public DateTime SessionStart { get; set; }

        public Dictionary<string, StoredItem> StoredItems { get; set; }

    }
}
