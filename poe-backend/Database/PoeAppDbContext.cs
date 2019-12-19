using Microsoft.EntityFrameworkCore;
using poe_backend.Models.ItemData;
using poe_backend.Models.ItemData.Armours;
using poe_backend.Models.ItemData.Jewelery;
using poe_backend.Models.ItemData.Weapons;

namespace poe_backend.Database
{
    public class PoeAppDbContext : DbContext
    {
        //Armours
        public DbSet<BodyArmour> BodyArmours { get; set; }
        public DbSet<Boot> Boots { get; set; }
        public DbSet<Glove> Gloves { get; set; }
        public DbSet<Helmet> Helmets { get; set; }
        public DbSet<Quiver> Quivers { get; set; }
        public DbSet<Shield> Shields { get; set; }

        // Jewelery
        public DbSet<Amulet> Amulets { get; set; }
        public DbSet<Talisman> Talismans { get; set; }
        public DbSet<Ring> Rings { get; set; }
        public DbSet<Belt> Belts { get; set; }

        // Weapons
        public DbSet<Bow> Bows { get; set; }
        public DbSet<Claw> Claws { get; set; }
        public DbSet<Dagger> Daggers { get; set; }
        public DbSet<FishingRod> FishingRods { get; set; } // fishing rods o_O ok poe ..
        public DbSet<OneHandedAxe> OneHandedAxes { get; set; }
        public DbSet<OneHandedMace> OneHandedMaces { get; set; }
        public DbSet<OneHandedSword> OneHandedSwords { get; set; }
        public DbSet<OneHandedThrustingSword> OneHandedThrustingSwords { get; set; }
        public DbSet<Staff> Staves { get; set; }
        public DbSet<TwoHandedAxe> TwoHandedAxes { get; set; }
        public DbSet<TwoHandedMace> TwoHandedMaces { get; set; }
        public DbSet<TwoHandedSword> TwoHandedSwords { get; set; }
        public DbSet<Wand> Wands { get; set; }


        // Others
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