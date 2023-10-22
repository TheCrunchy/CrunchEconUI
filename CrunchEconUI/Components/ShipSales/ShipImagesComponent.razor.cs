using CrunchEconModels.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CrunchEconUI.Components.ShipSales
{
    public partial class ShipImagesComponent
    {
        [Parameter]
        public ShipListing ListedItem { get; set; }
    }
}
