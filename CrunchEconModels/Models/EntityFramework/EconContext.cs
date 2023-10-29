using CrunchEconModels.Models;
using CrunchEconModels.Models.Events;
using System;
using System.Data.Entity;
namespace CrunchEconModels.Models.EntityFramework
{
    public class EconContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
        }
        public EconContext(string connectionString) : base(connectionString)
        {
            try
            {
                //var script = this.Database.Connection..GenerateCreateScript();
                // script = script.Replace("CREATE TABLE", "CREATE TABLE IF NOT EXISTS");
                // this.Database.ExecuteSqlCommand(script);
            }
            catch (Exception e)
            {
                //A SqlException will be thrown if tables already exist. So simply ignore it.
                return;
            }
            var exists = Database.Exists();
            Database.CreateIfNotExists();
            Database.Connection.Open();
            //  Database.CreateIfNotExists();

        }

        public DbSet<ItemListing> playeritemlistings { get; set; }
        public DbSet<ShipListing> shiplistings { get; set; }
        public DbSet<Event> ArchivedEvents { get; set; }

    }
}
