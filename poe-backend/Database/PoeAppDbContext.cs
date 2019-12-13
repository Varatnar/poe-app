using Microsoft.EntityFrameworkCore;
using poe_backend.Models.ItemData;
using poe_backend.Models.ItemData.Weapons;

namespace poe_backend.Database
{
    public class PoeAppDbContext : DbContext
    {
        public DbSet<OneHandedSword> OneHandedSwords { get; set; }
        public DbSet<TwoHandedSword> TwoHandedSwords { get; set; }
        public DbSet<PoeTag> PoeTags { get; set; }
        
        public DbSet<ItemTag> ItemTags { get; set; }
        
        public DbSet<BaseItem> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=Data.db;")
                          .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                          .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemTag>()
                        .HasKey(it => new {it.TagKey, Key = it.ItemKey});

            modelBuilder.Entity<ItemTag>()
                        .HasOne(it => it.Tag)
                        .WithMany(tag => tag.ItemTags)
                        .HasForeignKey(it => it.TagKey);

            modelBuilder.Entity<ItemTag>()
                        .HasOne(it => it.Item)
                        .WithMany(bi => bi.ItemTags)
                        .HasForeignKey(it => it.ItemKey);

            modelBuilder.Entity<PoeTag>()
                        .HasMany(tag => tag.ItemTags)
                        .WithOne(tag => tag.Tag)
                        .HasForeignKey(tag => tag.TagKey);
        }
    }
}