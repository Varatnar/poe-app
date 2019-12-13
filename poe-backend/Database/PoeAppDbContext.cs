using Microsoft.EntityFrameworkCore;
using poe_backend.Models.ItemData;
using poe_backend.Models.ItemData.Armours;
using poe_backend.Models.ItemData.Jewelery;
using poe_backend.Models.ItemData.Weapons;

namespace poe_backend.Database
{
    public class PoeAppDbContext : DbContext
    {
        public DbSet<OneHandedSword> OneHandedSwords { get; set; }
        public DbSet<TwoHandedSword> TwoHandedSwords { get; set; }
        public DbSet<Amulet> Amulets { get; set; }
        public DbSet<Talisman> Talismans { get; set; }
        public DbSet<BodyArmour> BodyArmours { get; set; }
        public DbSet<Boot> Boots { get; set; }
        public DbSet<Glove> Gloves { get; set; }
        public DbSet<Helmet> Helmets { get; set; }

        public DbSet<PoeTag> PoeTags { get; set; }

        public DbSet<ItemTag> ItemTags { get; set; }

        public PoeAppDbContext() : base(new DbContextOptionsBuilder<PoeAppDbContext>()
                                        .UseSqlite(@"Data Source=Data.db;")
                                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                        .EnableSensitiveDataLogging()
                                        .Options)
        {
        }

        public PoeAppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemTag>()
                        .HasKey(it => new {it.TagId, Key = it.ItemKey});

            modelBuilder.Entity<ItemTag>()
                        .HasOne(it => it.Tag)
                        .WithMany(tag => tag.BaseItemsLink)
                        .HasForeignKey(it => it.TagId);

            modelBuilder.Entity<ItemTag>()
                        .HasOne(it => it.Item)
                        .WithMany(bi => bi.PoeTagsLink)
                        .HasForeignKey(it => it.ItemKey);
        }
    }
}