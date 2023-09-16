using CrunchEconModels.Models;

namespace CrunchEconUI.Services
{
    public class PlayerBalanceAndNotifyService
    {
        private Dictionary<ulong, long> PlayerBalances = new Dictionary<ulong, long>();
        public Action<ulong>? RefreshListings { get; set; }
        public Action<ulong, string>? SendNotification { get; set; }
        public long GetBalance(ulong steamid)
        {
            if (PlayerBalances.ContainsKey(steamid))
            {
                return PlayerBalances[steamid];
            }
            PlayerBalances.Add(steamid, 66666666);
            return 5555550;  
        }

        public void SetBalance(ulong steamid, long balance)
        {
            return;
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

        public void SendPlayerNotification(ulong steamId, string Notification)
        {
            SendNotification?.Invoke(steamId, Notification);
        }
    }
}
