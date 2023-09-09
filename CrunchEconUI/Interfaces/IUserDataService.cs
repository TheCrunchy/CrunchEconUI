using CrunchEconUI.Models;

namespace CrunchEconUI.Interfaces
{
    public interface IUserDataService
    {
        UserSession GetData(ulong id);
        void StoreData(UserSession package);
    }
}
