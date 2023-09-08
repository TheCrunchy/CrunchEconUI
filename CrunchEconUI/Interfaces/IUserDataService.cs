using CrunchEconUI.Models;

namespace CrunchEconUI.Interfaces
{
    public interface IUserDataService
    {
        UserInfo GetData(ulong id);
        void StoreData(UserInfo package);
    }
}
