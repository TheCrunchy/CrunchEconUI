using CrunchEconModels.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace CrunchEconUI.EntityFramework
{
    public class EconContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Program.DBString);
        }

        public EconContext()
        {
            this.Database.EnsureCreated();
            
        }

        public DbSet<ItemListing> playeritemlistings { get; set; }
        
    }
}
