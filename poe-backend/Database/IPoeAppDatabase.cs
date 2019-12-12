using System.Collections.Generic;
using poe_backend.Models.ItemData.Weapons;

namespace poe_backend.Database
{
    public interface IPoeAppDatabase
    {
        IEnumerable<OneHandedSword> GetAllOneHandedSword();
        IEnumerable<TwoHandedSword> GetAllTwoHandedSwords();
        void AddOneHandedWeapon(OneHandedSword weapon);
        void AddTwoHandedWeapon(TwoHandedSword weapon);
    }
}