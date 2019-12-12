using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using poe_backend.Models.ItemData;

namespace poe_backend.Database
{
    public class PoeAppDbContext : DbContext
    {
        public DbSet<BaseItem> BaseItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=Data.db;")
                          .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseItem>()
                        .Property(item => item.Implicits)
                        .HasConversion(
                            v => string.Join(';', v),
                            v => new List<string>(
                                v.Split(';', StringSplitOptions.RemoveEmptyEntries)));
            
            modelBuilder.Entity<BaseItem>()
                        .Property(item => item.Tags)
                        .HasConversion(
                            v => string.Join(';', v),
                            v => new List<string>(
                                v.Split(';', StringSplitOptions.RemoveEmptyEntries)));
        }
    }
}