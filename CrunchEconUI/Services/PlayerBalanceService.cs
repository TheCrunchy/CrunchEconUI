using CrunchEconModels.Models;

namespace CrunchEconUI.Services
{
    public class PlayerBalanceService
    {
        private Dictionary<ulong, long> PlayerBalances = new Dictionary<ulong, long>();
        public Action<ulong>? RefreshListings { get; set; }
        public long GetBalance(ulong steamid)
        {
            if (PlayerBalances.ContainsKey(steamid))
            {
                return PlayerBalances[steamid];
            }
            PlayerBalances.Add(steamid, 0);
            return 0;  
        }

        public void SetBalance(ulong steamid, long balance)
        {
            if (PlayerBalances.ContainsKey(steamid))
            {
                PlayerBalances[steamid] = balance;
                RefreshListings?.Invoke(steamid);
            }
            else
            {
                PlayerBalances.Add(steamid, balance);
                RefreshListings?.Invoke(steamid);
            }
        }
    }
}
