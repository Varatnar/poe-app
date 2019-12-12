using System.ComponentModel.DataAnnotations.Schema;

namespace poe_backend.Models.ItemData.Weapons
{
    [Table("Two_Hand_Swords")]
    public class TwoHandedSword : BaseItem
    {
        public override ItemClass ItemClass => ItemClass.TwoHandedSword;
    }
}