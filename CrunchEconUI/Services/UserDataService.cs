//using CrunchEconUI.Interfaces;
//using CrunchEconUI.Models;

//namespace CrunchEconUI.Services
//{
//    public class UserDataService : IUserDataService
//    {
//        public Dictionary<ulong, UserSession> Data = new Dictionary<ulong, UserSession>();
//        public UserSession GetData(ulong id)
//        {
//            if (Data.TryGetValue(id, out var package))
//            {
//                return package;
//            }
//            return null;
//        }

//        public void StoreData(UserSession package)
//        {
//            if (!Data.ContainsKey(package.SteamId))
//            {
//                Data.Add(package.SteamId, package);
//            }
//            else
//            {
//                Data[package.SteamId] = package;
//            }
//        }

//    }
//}
