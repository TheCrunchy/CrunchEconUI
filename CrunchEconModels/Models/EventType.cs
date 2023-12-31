﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrunchEconModels.Models
{
    public enum EventType
    {
        BalanceUpdate,
        BuyItemResult,
        SellItemResult,
        ListItemResult,
        ListItem,
        BuyItem,
        BuyShip,
        SellItem,
        TextureEvent,
        DeleteListing,
        SaveTexturesJson,
        BuyShipResult,
        PrefabEvent,
    }

    public enum EventSource
    {
        Web,
        Torch,
        Archived
    }
}
