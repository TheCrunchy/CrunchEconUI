namespace CrunchEconUI.Services
{
    public class PlayerBalanceService
    {
        private Dictionary<ulong, long> PlayerBalances = new Dictionary<ulong, long>();

        public long GetBalance(ulong steamid)
        {
            if (PlayerBalances.ContainsKey(steamid))
            {
                return PlayerBalances[steamid];
            }
            return 88888888;  
        }

        public void SetBalance(ulong steamid, long balance)
        {
            if (PlayerBalances.ContainsKey(steamid))
            {
                PlayerBalances[steamid] = balance;
            }
            else
            {
                PlayerBalances.Add(steamid, balance);
            }
            
        }
    }
}
