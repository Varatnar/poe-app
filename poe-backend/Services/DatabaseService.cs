using System.Collections.Generic;
using System.Linq;
using poe_backend.Database;
using poe_backend.Models;

namespace poe_backend.Services
{
    public class DatabaseService : IPoeAppDatabase
    {
        private readonly PoeAppDbContext _database;

        public DatabaseService(PoeAppDbContext database)
        {
            _database = database;
        }
        
    }
}