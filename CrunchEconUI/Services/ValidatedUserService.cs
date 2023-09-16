using CrunchEconUI.Interfaces;
using CrunchEconUI.Models;

namespace CrunchEconUI.Services
{
    public class ValidatedUserService 
    {
        private Dictionary<ulong, UserInfo> Data = new Dictionary<ulong, UserInfo>();
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
            if (!Data.ContainsKey(package.SteamId))
            {
                Data.Add(package.SteamId, package);
            }
            else
            {
                Data[package.SteamId] = package;
            }
        }

    }
}
