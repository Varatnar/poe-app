using Microsoft.EntityFrameworkCore;
using poe_backend.Models.ItemData;

namespace poe_backend.Database
{
    public class PoeAppDbContext : DbContext
    {
        //Armours
        public DbSet<BaseItem> BaseItems { get; set; }


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