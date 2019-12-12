using System.Collections.Generic;
using poe_backend.Database;
using poe_backend.Models.ItemData;

namespace poe_backend.Services
{
    public class DatabaseService : IPoeAppDatabase
    {
        private readonly PoeAppDbContext _database;

        public DatabaseService(PoeAppDbContext database)
        {
            _database = database;
        }

        public IEnumerable<BaseItem> GetAll()
        {
            return _database.BaseItems;
        }
    }
}