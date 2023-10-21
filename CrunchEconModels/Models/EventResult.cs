using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrunchEconModels.Models
{
    public enum EventResult
    {
        Success,
        Failure,
        Expired,
        NotEnoughSpaceInCargo,
        NotEnoughItems,
        BuyerCannotAfford,
        ItemIdDoesntExist,
        NotOnline,
        NotEnoughReputation,
    }
}
