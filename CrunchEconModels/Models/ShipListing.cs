using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CrunchEconModels.Models
{
    public class ShipListing
    {
        public string ShipPrefabName { get; set; }
        public string Description { get; set; }
        public string ShipName { get; set; }
        public long Price { get; set; } = 1;
        public bool RequireReputation { get; set; } = false;
        public string FactionTag { get; set; }
        public int ReputationRequirement { get; set; } = 0;
        public string ImagePath { get; set; }
        public List<String> ImageUrls { get; set; }

        public bool Deleted { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
