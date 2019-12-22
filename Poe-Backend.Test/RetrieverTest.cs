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
        public void BaseItemWithTagsNotEmptyOrNullTest()
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
                               .BaseItems
                               .Include(item => item.PoeTagsLink);

                    foreach (var baseItem in data)
                    {
                        Assert.IsNotNull(baseItem.PoeTagsLink);
                        Assert.IsNotEmpty(baseItem.PoeTagsLink);

                        Assert.IsNotNull(baseItem.PoeTagsLink.First());

                        Assert.IsTrue(baseItem.PoeTagsLink.First().ItemKey != "");
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