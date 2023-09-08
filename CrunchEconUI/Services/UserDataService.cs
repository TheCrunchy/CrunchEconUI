using CrunchEconUI.Interfaces;
using CrunchEconUI.Models;

namespace CrunchEconUI.Services
{
    public class UserDataService : IUserDataService
    {
        public Dictionary<ulong, UserInfo> Data = new Dictionary<ulong, UserInfo>();
        public UserInfo GetData(ulong id)
        {
            if (Data.TryGetValue(id, out var package))
            {
                return package;
            }
            return null;
        }

        public void StoreData(UserInfo package)
        {
            if (!Data.ContainsKey(ulong.Parse(package.SteamId)))
            {
                Data.Add(ulong.Parse(package.SteamId), package);
            }
            else
            {
                Data[ulong.Parse(package.SteamId)] = package;
            }
        }

    }
}
