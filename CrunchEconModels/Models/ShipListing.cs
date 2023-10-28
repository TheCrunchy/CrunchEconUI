using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CrunchEconModels.Models
{
    [Table("shiplistings")]
    public class ShipListing
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ShipPrefabName { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Description { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ShipName { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Price { get; set; } = 1; 
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public bool RequireReputation { get; set; } = false; 
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string FactionTag { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ReputationRequirement { get; set; } = 0;
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ImagePath { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public List<String> ImageUrls { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public bool Deleted { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
