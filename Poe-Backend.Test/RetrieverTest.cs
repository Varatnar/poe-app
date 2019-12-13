using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using poe_backend.Database;
using poe_backend.DataRetrieving;

namespace Poe_Backend.Test
{
    public class RetrieverTest
    {
        [Test]
        public void AddWeaponWithTags()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<PoeAppDbContext>()
                          .UseSqlite(connection)
                          .Options;
            try
            {
                using (var context = new PoeAppDbContext(options))
                {
                    context.Database.EnsureCreated();
                }

                var retriever = new Retriever();

                retriever.InitializeBaseItemTable(options);

                using (var context = new PoeAppDbContext(options))
                {
                    var data = context
                               .OneHandedSwords
                               .Include(weapon => weapon.PoeTagsLink);

                    foreach (var weapon in data)
                    {
                        Assert.IsNotEmpty(weapon.PoeTagsLink);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}