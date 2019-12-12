using Microsoft.EntityFrameworkCore;

namespace poe_backend.Database
{
    public class PoeAppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=Data.db;")
                          .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}