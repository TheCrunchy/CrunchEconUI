using CrunchEconModels.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace CrunchEconUI.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        public string DbPath { get; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql($"Data Source={DbPath}");

        public DbSet<ItemListing> PlayerItemListings { get; set; }
        
    }
}
