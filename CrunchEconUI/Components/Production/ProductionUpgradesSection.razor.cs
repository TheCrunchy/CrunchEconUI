using Blazorise.DataGrid;
using CrunchEconModels.Models;
using CrunchEconModels.Models.Upgrades;
using CrunchEconUI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CrunchEconUI.Components.Production
{
    public partial class ProductionUpgradesSection
    {
        [Inject]
        private ProductionUpgradeService Upgrades { get; set; }

        public DataGrid<Upgrade> GridRefAssembler { get; set; }
    }
}
