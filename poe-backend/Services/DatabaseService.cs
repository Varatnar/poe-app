using System.Collections.Generic;
using poe_backend.Database;
using poe_backend.Models.ItemData.Weapons;

namespace poe_backend.Services
{
    public class DatabaseService : IPoeAppDatabase
    {
        private readonly PoeAppDbContext _database;

        public DatabaseService(PoeAppDbContext database)
        {
            _database = database;
        }

        public IEnumerable<TwoHandedSword> GetAllTwoHandedSwords()
        {
            return _database.TwoHandedSwords;
        }

        public void AddOneHandedWeapon(OneHandedSword weapon)
        {
            _database.OneHandedSwords.Add(weapon);
            _database.SaveChanges();
        }

        public void AddTwoHandedWeapon(TwoHandedSword weapon)
        {
            _database.TwoHandedSwords.Add(weapon);
            _database.SaveChanges();
        }

        public IEnumerable<OneHandedSword> GetAllOneHandedSword()
        {
            return _database.OneHandedSwords;
        }
    }
}