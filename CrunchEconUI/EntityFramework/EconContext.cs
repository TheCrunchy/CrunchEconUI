using CrunchEconModels.Models;
using CrunchEconModels.Models.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
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
            try
            {
               var script = this.Database.GenerateCreateScript();
                script = script.Replace("CREATE TABLE", "CREATE TABLE IF NOT EXISTS");
                this.Database.ExecuteSqlRaw(script);
                return;
            }
            catch (Exception e)
            {
                //A SqlException will be thrown if tables already exist. So simply ignore it.
                return;
            }
            this.Database.EnsureCreated();
            
        }

        public DbSet<ItemListing> playeritemlistings { get; set; }
        public DbSet<ShipListing> shiplistings { get; set; }
        public DbSet<Event> ArchivedEvents { get; set; }
        
    }
}
