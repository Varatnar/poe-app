using Microsoft.EntityFrameworkCore;
using poe_backend.Models.ItemData.Weapons;

namespace poe_backend.Database
{
    public class PoeAppDbContext : DbContext
    {
        public DbSet<OneHandedSword> OneHandedSwords { get; set; }
        public DbSet<TwoHandedSword> TwoHandedSwords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=Data.db;")
                          .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Place Holder
        }
    }
}